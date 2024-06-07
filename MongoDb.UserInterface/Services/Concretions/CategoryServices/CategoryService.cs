using AutoMapper;
using MongoDb.UserInterface.Dtos.CategoryDto;
using MongoDb.UserInterface.Entities;
using MongoDb.UserInterface.Services.Abstractions.CategoryServices;
using MongoDb.UserInterface.Settings;
using MongoDB.Driver;

namespace MongoDb.UserInterface.Services.Concretions.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollecton;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _categoryCollecton = database.GetCollection<Category>(_databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var values = _mapper.Map<Category>(createCategoryDto);
            await _categoryCollecton.InsertOneAsync(values);
        }

        public Task DeleteCategoryAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ResultCategoryDto>> GetAllAsync()
        {
            var values = await _categoryCollecton.Find(x=> true).ToListAsync();
            return _mapper.Map<List<ResultCategoryDto>>(values);
        }

        public Task<GetByIdCategoryDto> GetByIdCategoryAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
        {
            throw new NotImplementedException();
        }
    }
}
