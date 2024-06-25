using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDb.UserInterface.Entities
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<CartItem> CartItems { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
