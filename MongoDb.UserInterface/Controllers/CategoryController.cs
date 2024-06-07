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

        public IActionResult Index()
        {
            return View();
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
    }
}
