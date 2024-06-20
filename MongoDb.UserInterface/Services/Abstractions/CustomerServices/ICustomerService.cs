using MongoDb.UserInterface.Dtos.CustomerDtos;
using MongoDb.UserInterface.Dtos.ProductDto;

namespace MongoDb.UserInterface.Services.Abstractions.CustomerServices
{
    public interface ICustomerService
    {
        Task<List<ResultCustomerDto>> GetAllAsync();
        Task CreateCustomerAsync(CreateCustomerDto createCustomerDto);
        Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto);
        Task DeleteCustomerAsync(string id);
        Task<GetByIdCustomerDto> GetByIdCustomerAsync(string id);
    }
}
