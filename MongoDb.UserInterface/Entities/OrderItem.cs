using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDb.UserInterface.Entities
{
    public class OrderItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
    }
}