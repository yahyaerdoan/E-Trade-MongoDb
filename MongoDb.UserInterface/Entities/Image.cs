using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDb.UserInterface.Entities
{
    public class Image
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }
}
