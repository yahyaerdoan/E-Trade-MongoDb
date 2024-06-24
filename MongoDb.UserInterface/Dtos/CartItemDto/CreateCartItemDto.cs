namespace MongoDb.UserInterface.Dtos.CartItemDto
{
    public class CreateCartItemDto
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
    }
}
