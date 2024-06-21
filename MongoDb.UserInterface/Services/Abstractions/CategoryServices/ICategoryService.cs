using MongoDb.UserInterface.Dtos.CategoryDto;

namespace MongoDb.UserInterface.Services.Abstractions.CategoryServices
{
    public interface ICategoryService
    {
        Task<List<ResultCategoryDto>> GetAllAsync();
        Task CreateCategoryAsync(CreateCategoryDto createCategoryDto);
        Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto);
        Task DeleteCategoryAsync(string id);
        Task<GetByIdCategoryDto> GetByIdCategoryAsync(string id);
        Task<byte[]> CreateCategoryListPdfAsync(List<ResultCategoryDto> categories);
    }
}
