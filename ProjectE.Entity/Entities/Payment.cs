using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectE.Entity.Entities
{
    public class Payment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserId { get; set; }
        public string CompanyId { get; set; }

        public decimal Amount { get; set; }
        public string PaymentType { get; set; } // Abonelik / Komisyon
        public bool IsApproved { get; set; } = false;

        public DateTime PaidAt { get; set; } = DateTime.UtcNow;
    }
}
