using Microsoft.AspNetCore.Mvc;
using MongoDb.UserInterface.Dtos.CartItemDto;
using MongoDb.UserInterface.Entities;
using MongoDb.UserInterface.Services.Abstractions.CartService;
using MongoDb.UserInterface.Services.Abstractions.CustomerServices;
using MongoDb.UserInterface.Services.Abstractions.ProductServices;

namespace MongoDb.UserInterface.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ICustomerService _customerService;

        public CartController(ICartService cartService, ICustomerService customerService)
        {
            _cartService = cartService;
            _customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            var customerId = await _customerService.GetByIdCustomerAsync("66736111cd2dfff3d08a7c32");
            var cart = await _cartService.GetCartByCustomerIdAsync(customerId.Id);
            return View(new ResultCartDto()
            {
                Id = cart.Id,
                ResultCartItemDtos = cart.CartItems.Select(i => new ResultCartItemDto()
                {
                    //Id = i.Id,
                    ProductId = i.ProductId,
                    //Name = i.Product.Name,
                    //Description = i.Product.Description,
                    //Price = i.Product.Price,
                    Quantity = i.Quantity

                }).ToList()
            });

        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(string productId, int quantity)
        {
            string staticCustomerId = "66736111cd2dfff3d08a7c32";
            await _cartService.InitializeCart(staticCustomerId);

            var cart = await _cartService.GetCartByCustomerIdAsync(staticCustomerId);
            if (cart != null)
            {
                await _cartService.AddToCartAsync(cart.Id, productId, quantity);
            }

            return RedirectToAction("Index");
        }
    }
}
