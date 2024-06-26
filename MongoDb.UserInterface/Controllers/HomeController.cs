using Microsoft.AspNetCore.Mvc;
using MongoDb.UserInterface.Dtos.CartItemDto;
using MongoDb.UserInterface.Models;
using MongoDb.UserInterface.Services.Abstractions.CartService;
using MongoDb.UserInterface.Services.Abstractions.CustomerServices;
using MongoDb.UserInterface.Services.Abstractions.ProductServices;
using MongoDb.UserInterface.Services.Concretions.CartService;
using MongoDb.UserInterface.Services.Concretions.CustomerServices;
using System.Diagnostics;

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

        public async Task<IActionResult> Index()
        {
            var staticCustomerId = _customerService.GetCustomerByStaticId();
            var cart = await _cartService.GetCartByCustomerIdAsync(staticCustomerId);

            var cartQuantities = new Dictionary<string, int>();

            foreach (var cartItem in cart.CartItems)
            {
                cartQuantities[cartItem.ProductId.ToString()] = cartItem.Quantity;
            }

            ViewBag.CartQuantities = cartQuantities;

            var values = await _productService.GetAllProductWithCategoryAsync();
            return View(values);
        }
    }
}