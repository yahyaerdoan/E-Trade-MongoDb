using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDb.UserInterface.Entities
{
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
