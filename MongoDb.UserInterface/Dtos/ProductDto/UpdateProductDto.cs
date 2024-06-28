using System.ComponentModel.DataAnnotations.Schema;

namespace MongoDb.UserInterface.Dtos.ProductDto
{
    public class UpdateProductDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string CategoryId { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();

  
        public List<IFormFile> Photos { get; set; } = new List<IFormFile>();
    }
}
