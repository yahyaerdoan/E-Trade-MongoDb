using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDb.UserInterface.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }
        public string Name { get; set; }    
        public string Description { get; set; }    
        public decimal Price { get; set; }    
        public int Stock { get; set; }    
        public string ImageUrl { get; set; }    
    }
}
