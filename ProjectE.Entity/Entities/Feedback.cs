using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectE.Entity.Entities
{
    public class Feedback
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string OfferId { get; set; }     // Geri bildirimin geldiği teklif
        public string UserId { get; set; }      // Yorumu yapan kullanıcı
        public string CompanyId { get; set; }   // Yorumu alan firma

        public string Comment { get; set; }
        public int Rating { get; set; }         // 1-5 arası puan

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
