using AutoMapper;
using MongoDb.UserInterface.Dtos.CategoryDto;
using MongoDb.UserInterface.Entities;
using MongoDb.UserInterface.Services.Abstractions.CategoryServices;
using MongoDb.UserInterface.Settings.MongoDb.NewContext;
using MongoDB.Driver;

namespace MongoDb.UserInterface.Services.Concretions.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        #region old version for db connection
        #region Db Settings For Services. This is the Murat teacher's configuration. This is not using. Updated.
        //private readonly IMongoCollection<Category> _categoryCollection;
        #endregion
        #region Db Settings For Services. This is the Murat teacher's configuration. This is not using. Updated.
        //public CategoryService(IMapper mapper, IDatabaseSettings _databaseSettings)
        //{
        //    var client = new MongoClient(_databaseSettings.ConnectionString);
        //    var database = client.GetDatabase(_databaseSettings.DatabaseName);
        //    _categoryCollection = database.GetCollection<Category>(_databaseSettings.CategoryCollectionName);
        //    _mapper = mapper;
        //}
        #endregion
        #endregion

        private readonly IMongoDbContext _mongoDbContext;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IMongoDbContext mongoDbContext)
        {
            _mapper = mapper;
            _mongoDbContext = mongoDbContext;
        }

        public async Task CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var values = _mapper.Map<Category>(createCategoryDto);
            await _mongoDbContext.Categories.InsertOneAsync(values);
        }

        public async Task DeleteCategoryAsync(string id)
        {
            await _mongoDbContext.Categories.DeleteOneAsync(x=> x.Id == id);
        }

        public async Task<List<ResultCategoryDto>> GetAllAsync()
        {
            var values = await _mongoDbContext.Categories.Find(x=> true).SortBy(x=> x.Name).ToListAsync();
            return _mapper.Map<List<ResultCategoryDto>>(values);
        }

        public async Task<GetByIdCategoryDto> GetByIdCategoryAsync(string id)
        {
            var values  = await _mongoDbContext.Categories.Find<Category>(x=> x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdCategoryDto>(values);
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
        {
            var values = _mapper.Map<Category>(updateCategoryDto);
            await _mongoDbContext.Categories.FindOneAndReplaceAsync(x=> x.Id == updateCategoryDto.Id, values);
        }
    }
}
