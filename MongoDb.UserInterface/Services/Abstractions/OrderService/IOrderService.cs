using MongoDb.UserInterface.Entities;

namespace MongoDb.UserInterface.Services.Abstractions.OrderService
{
    public interface IOrderService
    {       
        Task CreateOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(string id);
    }
}
