namespace MongoDb.UserInterface.Dtos.ProductDto
{
    public record CreateProductDto(string Name, string Description, decimal Price, int StockQuantity, string CategoryId, string ImageUrl);
}
