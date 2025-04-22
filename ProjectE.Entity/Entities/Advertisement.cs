using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectE.Entity.Entities
{
    public class Advertisement
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string CompanyId { get; set; }

        public string AdTitle { get; set; }
        public string TargetCategory { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
