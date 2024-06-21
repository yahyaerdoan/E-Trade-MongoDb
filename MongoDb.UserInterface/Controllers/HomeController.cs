using Microsoft.AspNetCore.Mvc;
using MongoDb.UserInterface.Models;
using MongoDb.UserInterface.Services.Abstractions.ProductServices;
using System.Diagnostics;

namespace MongoDb.UserInterface.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _productService.GetAllProductWithCategoryAsync();
            return View(values);
        }
    }
}