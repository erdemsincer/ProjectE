using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectE.Entity.Entities
{
    public class Rating
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ReviewerUserId { get; set; }
        public string CompanyId { get; set; }

        public int Stars { get; set; } // 1-5 arası
        public string Comment { get; set; }

        public DateTime RatedAt { get; set; } = DateTime.UtcNow;
    }
}
