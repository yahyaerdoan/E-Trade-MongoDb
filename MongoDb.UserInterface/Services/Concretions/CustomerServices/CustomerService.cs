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

        public Task DeleteCustomerAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ResultCustomerDto>> GetAllAsync()
        {

            var values = await _mongoDbContext.Customers.Find(x=> true).ToListAsync();
            return _mapper.Map<List<ResultCustomerDto>>(values);
        }

        public Task<GetByIdProductDto> GetByIdProductAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto)
        {
            throw new NotImplementedException();
        }
    }
}
