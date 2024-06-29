using Microsoft.AspNetCore.Mvc;
using MongoDb.UserInterface.Dtos.ProductDto;
using MongoDb.UserInterface.Entities;
using MongoDb.UserInterface.GoogleCloudStorage.Services;
using MongoDb.UserInterface.Services.Abstractions.ProductServices;
using MongoDb.UserInterface.Settings.MongoDb.NewContext;

namespace MongoDb.UserInterface.Controllers
{
    public class ImageDriveController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICloudStorageService _cloudStorageService;

        public ImageDriveController(ICloudStorageService cloudStorageService, IProductService productService)
        {
            _cloudStorageService = cloudStorageService;
            _productService = productService;
        }

        public IActionResult Create(string productId)
        {
            var model = new ImageDrive { ProductId = productId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("ProductId,Photos")] ImageDrive imageDrive)
        {
            if (ModelState.IsValid)
            {
                if (imageDrive.Photos != null && imageDrive.Photos.Count > 0)
                {
                    foreach (var photo in imageDrive.Photos)
                    {
                        var savedFileName = GenerateFileNameToSave(photo.FileName);
                        var savedUrl = await _cloudStorageService.UploadFileAsync(photo, savedFileName);

                        imageDrive.SavedFileNames.Add(savedFileName);
                        imageDrive.SavedUrls.Add(savedUrl);
                    }

                    // Update product with image URLs
                    var product = await _productService.GetByIdProductAsync(imageDrive.ProductId);
                    if (product != null)
                    {
                        var updateProductDto = new UpdateProductDto(product.Id, product.Name, product.Description, product.Price, product.StockQuantity, product.CategoryId)
                        {
                            ImageUrls = product.ImageUrls.Concat(imageDrive.SavedUrls).ToList()
                        };

                        await _productService.UpdateProductAsync(updateProductDto);
                    }
                }

                return RedirectToAction("Index", "Product");
            }
            return View(imageDrive);
        }

        private string GenerateFileNameToSave(string incomingFileName)
        {
            var fileName = Path.GetFileNameWithoutExtension(incomingFileName);
            var extension = Path.GetExtension(incomingFileName);
            return $"{fileName}-{DateTime.Now.ToUniversalTime():yyyyMMddHHmmss}{extension}";
        }
    }
}
