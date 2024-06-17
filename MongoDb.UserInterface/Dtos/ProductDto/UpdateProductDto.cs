namespace MongoDb.UserInterface.Dtos.ProductDto
{
    public record UpdateProductDto(string Id, string Name, string Description, decimal Price, int StockQuantity, string CategoryId, List<string> ImageIds);
}
