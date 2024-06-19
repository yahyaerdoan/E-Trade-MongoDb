using MongoDB.Driver;
using MongoDb.UserInterface.Entities;

namespace MongoDb.UserInterface.Settings.MongoDb.Context
{
    public interface IMongoDbContext
    {
        IMongoCollection<Category> Categories { get; }
        IMongoCollection<Customer> Customers { get; }
        IMongoCollection<Image> Images { get; }
        IMongoCollection<Order> Orders { get; }
        IMongoCollection<OrderItem> OrderItems { get; }
        IMongoCollection<Product> Products { get; }
    }
}
