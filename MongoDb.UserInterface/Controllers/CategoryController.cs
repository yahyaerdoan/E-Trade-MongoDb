using Microsoft.AspNetCore.Mvc;
using MongoDb.UserInterface.Dtos.CategoryDto;
using MongoDb.UserInterface.Services.Abstractions.CategoryServices;

namespace MongoDb.UserInterface.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _categoryService.GetAllAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            await _categoryService.CreateCategoryAsync(createCategoryDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCategory(string id)
        {
            var values = await _categoryService.GetByIdCategoryAsync(id);
            return View(values);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            await _categoryService.UpdateCategoryAsync(updateCategoryDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DownloadPdf()
        {
            var values = await _categoryService.GetAllAsync();
            var pdf = await _categoryService.CreateCategoryListPdfAsync(values);
            return File(pdf, "application/pdf", "CategoryList.pdf");
        }
    }
}
