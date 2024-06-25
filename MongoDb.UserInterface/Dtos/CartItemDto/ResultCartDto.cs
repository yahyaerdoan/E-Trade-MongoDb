using MongoDb.UserInterface.Entities;

namespace MongoDb.UserInterface.Dtos.CartItemDto
{
    public class ResultCartDto
    {
        public string Id { get; set; }
        public List<ResultCartItemDto> ResultCartItemDtos { get; set; }


    }

    public class ResultCartItemDto
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string StringTypePrice => Price.ToString("C2");
        public int Quantity { get; set; }
    }
}
