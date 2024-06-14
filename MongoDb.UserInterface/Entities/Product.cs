using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDb.UserInterface.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]       
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public List<string> ImageUrls { get; set; }
    }
}
