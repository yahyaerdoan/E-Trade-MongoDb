namespace MongoDb.UserInterface.Dtos.ProductDto
{
    public record CreateProductDto(string Description, decimal Price, int StockQuantity, string CategoryId)
    {
        public string Name { get; set; }        
        public List<IFormFile> Photos { get; set; }
        public List<string> ImageUrls { get; init; }
        public List<string> SavedFileNames { get; set; } = new List<string>();
    }
}
