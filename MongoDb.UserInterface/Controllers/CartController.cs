using Microsoft.AspNetCore.Mvc;
using MongoDb.UserInterface.Dtos.CartItemDto;
using MongoDb.UserInterface.Services.Abstractions.CartService;
using MongoDb.UserInterface.Services.Abstractions.CustomerServices;
using MongoDb.UserInterface.Services.Abstractions.ProductServices;

namespace MongoDb.UserInterface.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;

        public CartController(ICartService cartService, ICustomerService customerService, IProductService productService)
        {
            _cartService = cartService;
            _customerService = customerService;
            _productService = productService;
        }
        private string GetStaticCustomerId()
        {
            return _customerService.GetCustomerByStaticId();
        }
        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartByCustomerIdAsync(GetStaticCustomerId());

            var resultCartItemDtos = new List<ResultCartItemDto>();

            var cartQuantities = new Dictionary<string, int>();

            foreach (var cartItem in cart.CartItems)
            {
                var product = await _productService.GetByIdProductAsync(cartItem.ProductId);
                resultCartItemDtos.Add(new ResultCartItemDto
                {
                    ProductId = cartItem.ProductId,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Quantity = cartItem.Quantity  
                });

                cartQuantities[cartItem.ProductId.ToString()] = cartItem.Quantity;
            }

            ViewBag.CartQuantities = cartQuantities;

            return View(new ResultCartDto {Id = cart.Id, ResultCartItemDtos = resultCartItemDtos });
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(string productId, int quantity)
        {
            await _cartService.InitializeCart(GetStaticCustomerId());

            var cart = await _cartService.GetCartByCustomerIdAsync(GetStaticCustomerId());
            if (cart != null)
            {
                await _cartService.AddToCartAsync(cart.Id, productId, quantity);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(string productId, int change)
        {           
            var cart = await _cartService.GetCartByCustomerIdAsync(GetStaticCustomerId());
            await _cartService.UpdateQuantityAsync(cart.Id, productId, change);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCartItem(string productId)
        {
            var customer = await _customerService.GetByIdCustomerAsync(GetStaticCustomerId());
            await _cartService.DeleteCartItemAsync(customer.Id, productId);

            return RedirectToAction("Index");
        }
    }
}