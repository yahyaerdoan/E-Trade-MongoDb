using Microsoft.AspNetCore.Mvc;
using MongoDb.UserInterface.Dtos.CategoryDto;
using MongoDb.UserInterface.Dtos.ProductDto;
using MongoDb.UserInterface.Services.Abstractions.ProductServices;

namespace MongoDb.UserInterface.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _productService.GetAllAsync();
            return View(values);
        }
        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {            
            await _productService.CreateProductAsync(createProductDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(string id)
        {
            var values = await _productService.GetByIdProductAsync(id);
            return View(values);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        {
            await _productService.UpdateProductAsync(updateProductDto);
            return RedirectToAction("Index");
        }
    }
}
