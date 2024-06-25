using Microsoft.AspNetCore.Mvc;
using MongoDb.UserInterface.Models;
using MongoDb.UserInterface.Services.Abstractions.CartService;
using MongoDb.UserInterface.Services.Abstractions.CustomerServices;
using MongoDb.UserInterface.Services.Abstractions.ProductServices;
using System.Diagnostics;

namespace MongoDb.UserInterface.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private  readonly ICartService _cartService;
        private  readonly ICustomerService _customerService;

        public HomeController(IProductService productService, ICartService cartService, ICustomerService customerService)
        {
            _productService = productService;
            _cartService = cartService;
            _customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {           
            var values = await _productService.GetAllProductWithCategoryAsync();
            return View(values);
        }
    }
}