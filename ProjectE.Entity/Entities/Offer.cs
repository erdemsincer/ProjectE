using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProjectE.Entity.Entities
{
    public class Offer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserId { get; set; }
        public string CompanyId { get; set; }  // Firma bu teklife teklif verdiğinde dolacak
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? Budget { get; set; }
        public bool IsApprovedByAdmin { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
