namespace MongoDb.UserInterface.Dtos.ProductDto
{
    public record GetByIdProductDto(string ProductId, string Name, string Description, decimal Price, int Stock, string ImageUrl);
}
