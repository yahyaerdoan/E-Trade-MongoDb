namespace MongoDb.UserInterface.Dtos.ProductDto
{
    public record UpdateProductDto(string Id, string Name, string Description, decimal Price, int StockQuantity, string CategoryId)
    {
        public List<string> ImageUrls { get; set; } = new List<string>();
        public List<IFormFile> Photos { get; set; } = new List<IFormFile>();
        public List<string>? SavedFileNames { get; set; } = new List<string>();
    }
}
