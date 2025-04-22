using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectE.Entity.Entities
{
    public class Notification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserId { get; set; }
        public string Message { get; set; }

        public bool IsRead { get; set; } = false;

        public string Channel { get; set; } // Email / SMS / WhatsApp

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
