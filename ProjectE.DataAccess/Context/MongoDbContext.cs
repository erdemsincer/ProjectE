using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProjectE.DataAccess.Settings;
using ProjectE.Entity.Entities;

namespace ProjectE.DataAccess.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        // Tüm koleksiyonlar burada:
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Company> Companies => _database.GetCollection<Company>("Companies");
        public IMongoCollection<Offer> Offers => _database.GetCollection<Offer>("Offers");
        public IMongoCollection<Payment> Payments => _database.GetCollection<Payment>("Payments");
        public IMongoCollection<Notification> Notifications => _database.GetCollection<Notification>("Notifications");
        public IMongoCollection<Subscription> Subscriptions => _database.GetCollection<Subscription>("Subscriptions");
        public IMongoCollection<Advertisement> Advertisements => _database.GetCollection<Advertisement>("Advertisements");
        public IMongoCollection<Rating> Ratings => _database.GetCollection<Rating>("Ratings");
        public IMongoCollection<Feedback> Feedbacks =>_database.GetCollection<Feedback>("Feedbacks");
        public IMongoCollection<FeedbackReaction> FeedbackReactions =>_database.GetCollection<FeedbackReaction>("FeedbackReactions");

    }
}
