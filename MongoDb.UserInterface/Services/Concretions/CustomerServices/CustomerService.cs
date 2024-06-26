using AutoMapper;
using MongoDb.UserInterface.Dtos.CategoryDto;
using MongoDb.UserInterface.Dtos.CustomerDtos;
using MongoDb.UserInterface.Dtos.ProductDto;
using MongoDb.UserInterface.Entities;
using MongoDb.UserInterface.Services.Abstractions.CustomerServices;
using MongoDb.UserInterface.Settings.MongoDb.NewContext;
using MongoDB.Driver;

namespace MongoDb.UserInterface.Services.Concretions.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly IMongoDbContext _mongoDbContext;
        private readonly IMapper _mapper;

        public CustomerService(IMongoDbContext mongoDbContext, IMapper mapper)
        {
            _mongoDbContext = mongoDbContext;
            _mapper = mapper;
        }

        public async Task CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            var values = _mapper.Map<Customer>(createCustomerDto);
            await _mongoDbContext.Customers.InsertOneAsync(values);
        }

        public async Task DeleteCustomerAsync(string id)
        {
            var values = await _mongoDbContext.Customers.DeleteOneAsync(x=> x.Id == id);
        }

        public async Task<List<ResultCustomerDto>> GetAllAsync()
        {

            var values = await _mongoDbContext.Customers.Find(x=> true).ToListAsync();
            return _mapper.Map<List<ResultCustomerDto>>(values);
        }

        public async Task<GetByIdCustomerDto> GetByIdCustomerAsync(string id)
        {
            var values = await _mongoDbContext.Customers.Find<Customer>(x=> x.Id==id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdCustomerDto>(values);
        }

        public string GetCustomerByStaticId()
        {
            var customerId = "667363e416637e439fd89a3c";
            return  customerId;
        }

        public async Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto)
        {
            var values = _mapper.Map<Customer>(updateCustomerDto);
            await _mongoDbContext.Customers.FindOneAndReplaceAsync(x => x.Id == updateCustomerDto.Id, values);
        }
    }
}
