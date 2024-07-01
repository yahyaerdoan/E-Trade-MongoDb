using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDb.UserInterface.Dtos.ProductDto;
using MongoDb.UserInterface.Entities;
using MongoDb.UserInterface.GoogleCloudStorage.Services;
using MongoDb.UserInterface.Services.Abstractions.ProductServices;
using MongoDb.UserInterface.Settings.MongoDb.OldContext;
using MongoDB.Driver;
using System.Security.Cryptography.Xml;

namespace MongoDb.UserInterface.Services.Concretions.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;
        private readonly ICloudStorageService _cloudStorageService;

        public ProductService(IMapper mapper, IDatabaseSettings databaseSettings, ICloudStorageService cloudStorageService)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _productCollection = database.GetCollection<Product>(databaseSettings.ProductCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
            _cloudStorageService = cloudStorageService;
        }

        public async Task CreateProductAsync(CreateProductDto createProductDto)
        {
            var savedFileNames = new List<string>();
            var imageUrls = new List<string>();

            foreach (var photo in createProductDto.Photos)
            {
                var savedFileName = GenerateFileNameToSave(createProductDto.Name);
                var imageUrl = await _cloudStorageService.UploadFileAsync(photo, savedFileName);

                savedFileNames.Add(savedFileName);
                imageUrls.Add(imageUrl);
            }

            createProductDto = createProductDto with
            {
                ImageUrls = imageUrls,
                SavedFileNames = savedFileNames
            };

            var values = _mapper.Map<Product>(createProductDto);
            await _productCollection.InsertOneAsync(values);
        }

        public async Task DeleteProductAsync(string id)
        {
            await _productCollection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<List<ResultProductDto>> GetAllAsync()
        {
            var values = await _productCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultProductDto>>(values);
        }

        public async Task<List<ResultProductWithCategoryDto>> GetAllProductWithCategoryAsync()
        {
            #region Working with current record type dto .I removed all images urls got key cannot be null error, tried differents solve

            //var products = await _productCollection.Find(_ => true).ToListAsync();
            //var categories = await _categoryCollection.Find(_ => true).ToListAsync();

            //var categoryDictionary = categories.ToDictionary(c => c.Id, c => (c.Name, c.Description));

            //var result = products.Select(product =>
            //{
            //    if (product.CategoryId != null && categoryDictionary.TryGetValue(product.CategoryId, out var category))
            //    {
            //        return new ResultProductWithCategoryDto(
            //            product.Id,
            //            product.Name,
            //            product.Description,
            //            product.Price,
            //            product.StockQuantity,
            //            product.ImageUrls,
            //            category.Name,
            //            category.Description
            //        );
            //    }
            //    else
            //    {
            //        // Handle the case where the category is not found or CategoryId is null
            //        return new ResultProductWithCategoryDto(
            //            product.Id,
            //            product.Name,
            //            product.Description,
            //            product.Price,
            //            product.StockQuantity,
            //            product.ImageUrls,
            //            "Unknown",
            //            "Unknown"
            //        );
            //    }
            //}).ToList();


            //return result;
            #endregion


            #region This is not using with current dto, this not using with record type. 
            //I removed all images urls got key cannot be null error, tried differents solve
            //var products = await _productCollection.Find(_ => true).ToListAsync();
            //var categories = await _categoryCollection.Find(_ => true).ToListAsync();

            //var categoryDictionary = categories.ToDictionary(c => c.Id, c => (c.Name, c.Description));

            //var result = products.Select(product =>
            //{
            //    if (product.CategoryId != null && categoryDictionary.TryGetValue(product.CategoryId, out var category))
            //    {
            //        return new ResultProductWithCategoryDto
            //        {
            //            Id = product.Id,
            //            Name = product.Name,
            //            Description = product.Description,
            //            CategoryName = category.Name,
            //            CategoryDescription = category.Description
            //        };
            //    }
            //    else
            //    {
            //        // Handle the case where the category is not found or CategoryId is null
            //        return new ResultProductWithCategoryDto
            //        {
            //            Id = product.Id,
            //            Name = product.Name,
            //            Description = product.Description,
            //            CategoryName = "Unknown",
            //            CategoryDescription = "Unknown"
            //        };
            //    }
            //}).ToList();

            //return result;

            #endregion


            var products = await _productCollection.Find(_ => true).ToListAsync();
            var categories = await _categoryCollection.Find(_ => true).ToListAsync();

            var categoryDictionary = categories.ToDictionary(c => c.Id, c => (c.Name, c.Description));

            var result = products.Select(product =>
            {
                // Initialize default values
                string name = "Unknown";
                string description = "Unknown";

                // Check if CategoryId is not null and exists in the dictionary
                if (product.CategoryId != null && categoryDictionary.TryGetValue(product.CategoryId, out var details))
                {
                    name = details.Name;
                    description = details.Description;
                }

                return _mapper.Map<ResultProductWithCategoryDto>((product, name, description));
            }).ToList();

            return result;

            #region This is working first. I removed all images urls got key cannot be null error, tried differents solve
            //var products = await _productCollection.Find(x => true).ToListAsync();
            //var categories = await _categoryCollection.Find(x => true).ToListAsync();
            //var categoryDictionary = categories.ToDictionary(c => c.Id, c => (c.Name, c.Description));
            //var result = products.Select(product =>
            //{
            //    var (Name, Description) = categoryDictionary
            //                             .TryGetValue(product.CategoryId, out var details)
            //                             ? details : (Name: string.Empty, Description: string.Empty);
            //    var name = Name;
            //    var description = Description;
            //    return _mapper.Map<ResultProductWithCategoryDto>((product, name, description));
            //}).ToList();
            //return result;
            #endregion
        }

        public async Task<GetByIdProductDto> GetByIdProductAsync(string id)
        {
            var values = await _productCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdProductDto>(values);
        }

        public async Task RemoveImageUrlByIndexAsync(string productId, int index)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Id, productId);
            var product = await _productCollection.Find(filter).FirstOrDefaultAsync();

            if (product != null && index >= 0 && index < product.ImageUrls.Count && index < product.SavedFileNames.Count)
            {

                var fileNameToDelete = product.SavedFileNames[index];
                await _cloudStorageService.DeleteFileAsync(fileNameToDelete);

                product.ImageUrls.RemoveAt(index);
                product.SavedFileNames.RemoveAt(index);

                var update = Builders<Product>.Update
                    .Set(x => x.SavedFileNames, product.SavedFileNames)
                    .Set(x => x.ImageUrls, product.ImageUrls);

                await _productCollection.UpdateOneAsync(filter, update);
            }
        }

        public async Task SubtractFromProductStockAsync(string productId, int quantity)
        {
            var productFilter = Builders<Product>.Filter.Eq(x => x.Id, productId);
            var product = await _productCollection.Find(productFilter).FirstOrDefaultAsync();

            int newStockQuantity = product.StockQuantity - quantity;

            var updateProductStock = Builders<Product>.Update.Set(p => p.StockQuantity, newStockQuantity);
            await _productCollection.UpdateOneAsync(productFilter, updateProductStock);
        }

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            // Upload new photos and add URLs to the ImageUrls list
            if (updateProductDto.Photos != null && updateProductDto.Photos.Count > 0)
            {
                var savedFileNames = new List<string>();
                var imageUrls = new List<string>();

                foreach (var photo in updateProductDto.Photos)
                {
                    var savedFileName = GenerateFileNameToSave(updateProductDto.Name);
                    var imageUrl = await _cloudStorageService.UploadFileAsync(photo, savedFileName);

                    savedFileNames.Add(savedFileName);
                    imageUrls.Add(imageUrl);
                }

                updateProductDto = updateProductDto with
                {
                    ImageUrls = imageUrls,
                    SavedFileNames = savedFileNames
                };
            }

            var values = _mapper.Map<Product>(updateProductDto);
            await _productCollection.FindOneAndReplaceAsync(x => x.Id == updateProductDto.Id, values);
        }

        private string GenerateFileNameToSave(string incomingFileName)
        {
            var fileName = Path.GetFileNameWithoutExtension(incomingFileName);
            var extension = Path.GetExtension(incomingFileName);
            return $"{fileName}-{DateTime.Now.ToUniversalTime():yyyyMMddHHmmss}{extension}";
        }
    }
}
