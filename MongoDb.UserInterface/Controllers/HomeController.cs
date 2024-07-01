using Microsoft.AspNetCore.Mvc;
using MongoDb.UserInterface.Services.Abstractions.CartService;
using MongoDb.UserInterface.Services.Abstractions.CustomerServices;
using MongoDb.UserInterface.Services.Abstractions.ProductServices;

namespace MongoDb.UserInterface.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly ICartService _cartService;

        public HomeController(IProductService productService, ICustomerService customerService, ICartService cartService)
        {
            _productService = productService;
            _customerService = customerService;
            _cartService = cartService;
        }
        private string GetStaticCustomerId()
        {
            return _customerService.GetCustomerByStaticId();
        }
        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartByCustomerIdAsync(GetStaticCustomerId());

            // Check if cart or cart.CartItems is null
            if (cart == null || cart.CartItems == null)
            {
                ViewBag.CartQuantities = new Dictionary<string, int>();
            }
            else
            {
                var cartQuantities = new Dictionary<string, int>();
                foreach (var cartItem in cart.CartItems)
                {
                    cartQuantities[cartItem.ProductId.ToString()] = cartItem.Quantity;
                }
                ViewBag.CartQuantities = cartQuantities;
            }

            var values = await _productService.GetAllProductWithCategoryAsync();
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(string productId, int change)
        {
            var cart = await _cartService.GetCartByCustomerIdAsync(GetStaticCustomerId());
            await _cartService.UpdateQuantityAsync(cart.Id, productId, change);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrDeleteQuantity(string productId, int change)
        {
            var userId = GetStaticCustomerId();
            var cart = await _cartService.GetCartByCustomerIdAsync(userId);
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem != null)
            {
                int newQuantity = cartItem.Quantity + change;
                if (newQuantity <= 0)
                {
                    await _cartService.DeleteCartItemAsync(userId, productId);
                }
                else
                {
                    await _cartService.UpdateQuantityAsync(cart.Id, productId, change);
                }
            }

            return RedirectToAction("Index");
        }

    }
}