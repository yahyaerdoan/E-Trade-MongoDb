using AutoMapper;
using MongoDb.UserInterface.Dtos.ProductDto;
using MongoDb.UserInterface.Entities;
using MongoDb.UserInterface.GoogleCloudStorage.Services;
using MongoDb.UserInterface.Services.Abstractions.ProductServices;
using MongoDb.UserInterface.Settings.MongoDb.OldContext;
using MongoDB.Driver;

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
            var products = await _productCollection.Find(x => true).ToListAsync();
            var categories = await _categoryCollection.Find(x => true).ToListAsync();

            var categoryDictionary = categories.ToDictionary(c => c.Id, c => (c.Name, c.Description));
            var result = products.Select(product =>
            {
                var (Name, Description) = categoryDictionary
                                         .TryGetValue(product.CategoryId, out var details)
                                         ? details : (Name: string.Empty, Description: string.Empty);
                var name = Name;
                var description = Description;
                return _mapper.Map<ResultProductWithCategoryDto>((product, name, description));
            }).ToList();
            return result;
        }

        public async Task<GetByIdProductDto> GetByIdProductAsync(string id)
        {
            var values = await _productCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdProductDto>(values);
        }

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            // Upload new photos and add URLs to the ImageUrls list
            if (updateProductDto.Photos != null && updateProductDto.Photos.Count > 0)
            {
                foreach (var photo in updateProductDto.Photos)
                {
                    var savedFileName = GenerateFileNameToSave(photo.FileName);
                    var savedUrl = await _cloudStorageService.UploadFileAsync(photo, savedFileName);
                    updateProductDto.ImageUrls.Add(savedUrl);
                }
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
