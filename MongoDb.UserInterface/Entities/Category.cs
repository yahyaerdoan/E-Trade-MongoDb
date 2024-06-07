using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDb.UserInterface.Entities
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
