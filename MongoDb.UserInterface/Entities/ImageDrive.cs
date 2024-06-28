using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations.Schema;

namespace MongoDb.UserInterface.Entities
{
    public class ImageDrive
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

     
        public List<IFormFile>? Photos { get; set; }

        public List<string>? SavedUrls { get; set; } = new List<string>();

      
        public List<string>? SignedUrls { get; set; } = new List<string>();

        public List<string>? SavedFileNames { get; set; } = new List<string>();

        public string ProductId { get; set; }
    }
}
