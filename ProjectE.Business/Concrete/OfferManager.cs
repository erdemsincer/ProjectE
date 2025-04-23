using MongoDB.Driver;
using ProjectE.Business.Abstract;
using ProjectE.DataAccess.Context;
using ProjectE.DTO.OfferDtos;
using ProjectE.Entity.Entities;

namespace ProjectE.Business.Concrete
{
    public class OfferManager : IOfferService
    {
        private readonly IMongoCollection<Offer> _offers;

        public OfferManager(MongoDbContext context)
        {
            _offers = context.Offers;
        }

        public async Task<string> CreateOfferAsync(CreateOfferDto dto, string userId)
        {
            var offer = new Offer
            {
                UserId = userId,
                Title = dto.Title,
                Description = dto.Description,
                Budget = dto.Budget,
                IsApprovedByAdmin = false,
                CreatedAt = DateTime.UtcNow
            };

            await _offers.InsertOneAsync(offer);
            return "Teklif başarıyla oluşturuldu.";
        }

        public async Task<List<ResultOfferDto>> GetAllOffersAsync()
        {
            var offers = await _offers.Find(_ => true).ToListAsync();

            return offers.Select(o => new ResultOfferDto
            {
                Id = o.Id,
                UserId = o.UserId,
                Title = o.Title,
                Description = o.Description,
                Budget = o.Budget,
                IsApprovedByAdmin = o.IsApprovedByAdmin
            }).ToList();
        }

        public async Task<List<ResultOfferDto>> GetOffersForCompanyAsync(string companyId, bool isAdvertiser)
        {
            var offers = await _offers.Find(_ => true).ToListAsync();

            // Reklamlı firma ise tüm teklifleri öncelikli görebilir
            // (şimdilik sıralama yapılmıyor, ama mantık kuruldu)

            return offers.Select(o => new ResultOfferDto
            {
                Id = o.Id,
                UserId = o.UserId,
                CompanyId = o.CompanyId,
                Title = o.Title,
                Description = o.Description,
                Budget = o.Budget,
                IsApprovedByAdmin = o.IsApprovedByAdmin,
                CreatedAt = o.CreatedAt
            }).ToList();
        }

        public async Task<string> AssignCompanyToOfferAsync(string offerId, string companyId)
        {
            var offer = await _offers.Find(x => x.Id == offerId).FirstOrDefaultAsync();
            if (offer == null)
                return "Teklif bulunamadı.";

            if (!string.IsNullOrEmpty(offer.CompanyId))
                return "Bu teklif zaten başka bir firmaya atanmış.";

            offer.CompanyId = companyId;

            await _offers.ReplaceOneAsync(x => x.Id == offerId, offer); // <-- BU SATIR ZORUNLU!!

            return "Teklif başarıyla firmaya atandı.";
        }

        public async Task<List<ResultOfferDto>> GetOffersByUserAsync(string userId)
        {
            var offers = await _offers.Find(x => x.UserId == userId).ToListAsync();

            return offers.Select(o => new ResultOfferDto
            {
                Id = o.Id,
                UserId = o.UserId,
                CompanyId = o.CompanyId,
                Title = o.Title,
                Description = o.Description,
                Budget = o.Budget,
                IsApprovedByAdmin = o.IsApprovedByAdmin,
                CreatedAt = o.CreatedAt
            }).ToList();
        }



    }
}
