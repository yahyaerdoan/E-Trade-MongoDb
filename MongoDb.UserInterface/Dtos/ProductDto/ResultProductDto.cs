namespace MongoDb.UserInterface.Dtos.ProductDto
{
    public record ResultProductDto(string Id, string Name, string Description, decimal Price, int StockQuantity, string CategoryId, List<string> ImageUrls);
  
}
