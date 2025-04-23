using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectE.Entity.Entities
{
    public class FeedbackReaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string FeedbackId { get; set; }
        public string UserId { get; set; }
        public bool IsLike { get; set; }
    }
}
