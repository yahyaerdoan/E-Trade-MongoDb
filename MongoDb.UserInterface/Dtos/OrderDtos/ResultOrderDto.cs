using MongoDb.UserInterface.Dtos.CartItemDto;

namespace MongoDb.UserInterface.Dtos.OrderDtos
{
    public class ResultOrderDto
    {
        public CreateOrderDto CreateOrderDto { get; set; }
        public ResultCartDto ResultCartDto { get; set; }
    }
}
