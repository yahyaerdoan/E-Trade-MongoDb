namespace MongoDb.UserInterface.Dtos.ProductDto
{
    public record UpdateProductDto(string ProductId, string Name, string Description, decimal Price, int Stock, string ImageUrl);
}
