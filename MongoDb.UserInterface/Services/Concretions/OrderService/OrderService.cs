using MongoDb.UserInterface.Entities;
using MongoDb.UserInterface.Services.Abstractions.OrderService;
using MongoDb.UserInterface.Settings.MongoDb.NewContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDb.UserInterface.Services.Concretions.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IMongoDbContext _mongoDbContext;

        public OrderService(IMongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        public async Task CreateOrderAsync(Order order, List<OrderItem> orderItems)
        {
            await _mongoDbContext.Orders.InsertOneAsync(order);

            foreach (var orderItem in orderItems)
            {
                orderItem.OrderId = order.Id;
                await _mongoDbContext.OrderItems.InsertOneAsync(orderItem);
            }
        }

        public async Task<Order> GetOrderByIdAsync(string id)
        {
            return await _mongoDbContext.Orders.Find(o=> o.Id == id).FirstOrDefaultAsync();
        }
    }
}
