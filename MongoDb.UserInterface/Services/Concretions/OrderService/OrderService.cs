using MongoDb.UserInterface.Entities;
using MongoDb.UserInterface.Services.Abstractions.OrderService;
using MongoDb.UserInterface.Settings.MongoDb.NewContext;

namespace MongoDb.UserInterface.Services.Concretions.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IMongoDbContext _mongoDbContext;

        public OrderService(IMongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        public void Create(Order order)
        {
            _mongoDbContext.Orders.InsertOneAsync(order);
        }
    }
}
