using MongoDb.UserInterface.Entities;

namespace MongoDb.UserInterface.Services.Abstractions.OrderService
{
    public interface IOrderService
    {
        void Create(Order order);
    }
}
