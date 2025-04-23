using MongoDB.Driver;
using ProjectE.Business.Abstract;
using ProjectE.DataAccess.Context;
using ProjectE.DTO.SubscriptionDtos;
using ProjectE.Entity.Entities;

namespace ProjectE.Business.Concrete
{
    public class SubscriptionManager : ISubscriptionService
    {
        private readonly IMongoCollection<Subscription> _subscriptions;
        private readonly IMongoCollection<Company> _companies;

        public SubscriptionManager(MongoDbContext context)
        {
            _subscriptions = context.Subscriptions;
            _companies = context.Companies;
        }

        public async Task<string> StartSubscriptionAsync(CreateSubscriptionDto dto, string companyId)
        {
            var subscription = new Subscription
            {
                CompanyId = companyId,
                StartDate = DateTime.UtcNow,
                ExpireDate = DateTime.UtcNow.AddDays(dto.DurationInDays),
                IsActive = true
            };

            await _subscriptions.InsertOneAsync(subscription);

            // Firma reklam statüsünü aktif et
            var update = Builders<Company>.Update.Set(x => x.IsAdvertiser, true);
            await _companies.UpdateOneAsync(x => x.Id == companyId, update);

            return "Abonelik başarıyla başlatıldı.";
        }

        public async Task<ResultSubscriptionDto> GetMySubscriptionAsync(string companyId)
        {
            var subscription = await _subscriptions
                .Find(x => x.CompanyId == companyId && x.IsActive)
                .SortByDescending(x => x.ExpireDate)
                .FirstOrDefaultAsync();

            if (subscription == null) return null;

            return new ResultSubscriptionDto
            {
                Id = subscription.Id,
                CompanyId = subscription.CompanyId,
                StartDate = subscription.StartDate,
                ExpireDate = subscription.ExpireDate,
                IsActive = subscription.IsActive
            };
        }

        public async Task CheckAndExpireSubscriptionsAsync()
        {
            var now = DateTime.UtcNow;

            // Süresi dolmuş aktif abonelikleri bul
            var expiredSubscriptions = await _subscriptions
                .Find(x => x.IsActive && x.ExpireDate < now)
                .ToListAsync();

            foreach (var sub in expiredSubscriptions)
            {
                // 1. Aboneliği pasifleştir
                var updateSub = Builders<Subscription>.Update.Set(x => x.IsActive, false);
                await _subscriptions.UpdateOneAsync(x => x.Id == sub.Id, updateSub);

                // 2. Firma reklamlı değil yapılır
                var updateFirm = Builders<Company>.Update.Set(x => x.IsAdvertiser, false);
                await _companies.UpdateOneAsync(x => x.Id == sub.CompanyId, updateFirm);
            }
        }

    }
}
