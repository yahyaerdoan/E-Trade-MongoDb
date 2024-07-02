using MongoDb.UserInterface.Entities;

namespace MongoDb.UserInterface.Services.Abstractions.OrderService
{
    public interface IOrderService
    {       
        Task CreateOrderAsync(Order order, List<OrderItem> orderItems);
        Task<Order> GetOrderByIdAsync(string id);
    }
}
