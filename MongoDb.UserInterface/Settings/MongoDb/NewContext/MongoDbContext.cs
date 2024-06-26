using MongoDb.UserInterface.Entities;
using MongoDb.UserInterface.Settings.MongoDb.NewContext;
using MongoDB.Driver;

namespace MongoDb.UserInterface.Settings.MongoDb.Context
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;
        public MongoDbContext(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
        }
        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");

        public IMongoCollection<Customer> Customers => _database.GetCollection<Customer>("Customers");

        public IMongoCollection<Image> Images => _database.GetCollection<Image>("Images");

        public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");

        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");

        public IMongoCollection<Cart> Carts => _database.GetCollection<Cart>("Carts");

        public IMongoCollection<CartItem> CartItems => _database.GetCollection<CartItem>("CartItems");
        public IMongoCollection<OrderItem> OrderItems => _database.GetCollection<OrderItem>("OrderItems");

    }
}
