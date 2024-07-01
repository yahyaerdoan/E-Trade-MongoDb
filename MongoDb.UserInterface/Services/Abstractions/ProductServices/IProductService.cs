using MongoDb.UserInterface.Dtos.ProductDto;

namespace MongoDb.UserInterface.Services.Abstractions.ProductServices
{
    public interface IProductService
    {
        Task<List<ResultProductDto>> GetAllAsync();
        Task CreateProductAsync(CreateProductDto createProductDto);
        Task UpdateProductAsync(UpdateProductDto updateProductDto);
        Task DeleteProductAsync(string id);
        Task<GetByIdProductDto> GetByIdProductAsync(string id);
        Task<List<ResultProductWithCategoryDto>> GetAllProductWithCategoryAsync();
        Task RemoveImageUrlByIndexAsync(string productId, int index);
        Task SubtractFromProductStockAsync(string productId, int quantity);
    }
}
