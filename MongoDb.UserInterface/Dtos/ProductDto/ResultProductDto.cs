namespace MongoDb.UserInterface.Dtos.ProductDto
{
    public record ResultProductDto(string ProductId, string Name, string Description, decimal Price, int Stock, string ImageUrl);
}
