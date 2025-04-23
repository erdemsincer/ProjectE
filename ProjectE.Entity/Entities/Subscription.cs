using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectE.Entity.Entities
{
    public class Subscription
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string CompanyId { get; set; }
        public string PlanName { get; set; } // Basic, Pro, Premium

        public DateTime StartDate { get; set; }
        public DateTime ExpireDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
