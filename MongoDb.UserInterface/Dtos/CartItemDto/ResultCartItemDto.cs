namespace MongoDb.UserInterface.Dtos.CartItemDto
{
    public class ResultCartItemDto
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
    }
}
