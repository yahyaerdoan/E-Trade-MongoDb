using Microsoft.AspNetCore.Mvc;
using MongoDb.UserInterface.Dtos.ProductDto;
using MongoDb.UserInterface.GoogleCloudStorage.Services;
using MongoDb.UserInterface.Services.Abstractions.CategoryServices;
using MongoDb.UserInterface.Services.Abstractions.ProductServices;

namespace MongoDb.UserInterface.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICloudStorageService _cloudStorageService;


        public ProductController(IProductService productService, ICategoryService categoryService, ICloudStorageService cloudStorageService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _cloudStorageService = cloudStorageService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _productService.GetAllProductWithCategoryAsync();
            return View(values);
        }
        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var categoryValues = await _categoryService.GetAllAsync();
            ViewBag.Categories = categoryValues;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {            
            await _productService.CreateProductAsync(createProductDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction("Index");
        }

     
        [HttpGet]
        public async Task<IActionResult> UpdateProduct(string id)
        {
            await PopulateCategoriesAsync();
            var values = await _productService.GetByIdProductAsync(id);
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto, int? removeIndex)
        {
            if (removeIndex.HasValue)
            {                
              return  await RemoveImageUrlByIndexAsync(updateProductDto.Id, removeIndex.Value);
            }

            if (ModelState.IsValid)
            {
                await _productService.UpdateProductAsync(updateProductDto);
                return RedirectToAction("Index");
            }
            return View(updateProductDto);
        }

        private async Task PopulateCategoriesAsync()
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories;
        }

        private async Task<IActionResult> RemoveImageUrlByIndexAsync(string productId, int? removeIndex)
        {
            await _productService.RemoveImageUrlByIndexAsync(productId, removeIndex.Value);
            var updateProduct = await _productService.GetByIdProductAsync(productId);
            await PopulateCategoriesAsync();
            return RedirectToAction("UpdateProduct", updateProduct);
        }    
    }
}
