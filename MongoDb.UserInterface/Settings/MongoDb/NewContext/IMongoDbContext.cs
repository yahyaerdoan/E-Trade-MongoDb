using MongoDb.UserInterface.Entities;
using MongoDB.Driver;

namespace MongoDb.UserInterface.Settings.MongoDb.NewContext
{
    public interface IMongoDbContext
    {
        IMongoCollection<Category> Categories { get; }
        IMongoCollection<Customer> Customers { get; }
        IMongoCollection<ImageDrive> ImageDrives { get; }
        IMongoCollection<Order> Orders { get; }
        IMongoCollection<Product> Products { get; }
        IMongoCollection<Cart> Carts { get; }
        IMongoCollection<CartItem> CartItems { get; }
        IMongoCollection<OrderItem> OrderItems { get; }
    }
}
