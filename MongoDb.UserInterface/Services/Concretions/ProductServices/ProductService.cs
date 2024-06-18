using AutoMapper;
using MongoDb.UserInterface.Dtos.CategoryDto;
using MongoDb.UserInterface.Dtos.ProductDto;
using MongoDb.UserInterface.Entities;
using MongoDb.UserInterface.Services.Abstractions.ProductServices;
using MongoDb.UserInterface.Settings;
using MongoDB.Driver;

namespace MongoDb.UserInterface.Services.Concretions.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public ProductService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _productCollection = database.GetCollection<Product>(_databaseSettings.ProductCollectionName);
            _categoryCollection = database.GetCollection<Category>(_databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task CreateProductAsync(CreateProductDto createProductDto)
        {

            var values = _mapper.Map<Product>(createProductDto);
            await _productCollection.InsertOneAsync(values);    
        }

        public async Task DeleteProductAsync(string id)
        {
            await _productCollection.DeleteOneAsync(x=> x.Id == id);
        }

        public async Task<List<ResultProductDto>> GetAllAsync()
        {
            var values = await _productCollection.Find(x=> true).ToListAsync();     
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
                                        ? details : (Name :string.Empty, Description: string.Empty);
                var name = Name;
                var description = Description;
                return _mapper.Map<ResultProductWithCategoryDto>((product, name, description));
            }).ToList();
            return result;

        }

        public async Task<GetByIdProductDto> GetByIdProductAsync(string id)
        {
            var values = await _productCollection.Find<Product>(x => x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdProductDto>(values);
        }

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
           var values  = _mapper.Map<Product>(updateProductDto);
            await _productCollection.FindOneAndReplaceAsync(x => x.Id == updateProductDto.Id, values);
        }
    }
}
