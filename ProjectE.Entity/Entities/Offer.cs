using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProjectE.Entity.Entities
{
    public class Offer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserId { get; set; }     // Teklif isteyen müşteri
        public string CompanyId { get; set; }  // Teklif veren firma

        public string Description { get; set; }

        public decimal? Price { get; set; }

        public bool IsApprovedByAdmin { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
