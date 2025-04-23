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
            // ✅ Sadece admin onaylı teklifleri getir
            var offers = await _offers.Find(x => x.IsApprovedByAdmin).ToListAsync();

            // Reklamlı firmalara göre sıralama
            var sortedOffers = isAdvertiser
                ? offers.OrderByDescending(x => x.CreatedAt).ToList()
                : offers.OrderBy(x => x.CreatedAt).ToList();

            return sortedOffers.Select(o => new ResultOfferDto
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

        public async Task<List<ResultOfferDto>> GetOffersByCompanyAsync(string companyId)
        {
            var offers = await _offers.Find(x => x.CompanyId == companyId).ToListAsync();

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

        public async Task<string> ApproveOfferAsync(ApproveOfferDto dto)
        {
            var offer = await _offers.Find(x => x.Id == dto.OfferId).FirstOrDefaultAsync();
            if (offer == null)
                return "Teklif bulunamadı.";

            var update = Builders<Offer>.Update.Set(x => x.IsApprovedByAdmin, dto.IsApproved);
            await _offers.UpdateOneAsync(x => x.Id == dto.OfferId, update);

            return dto.IsApproved ? "Teklif onaylandı." : "Teklif reddedildi.";
        }





    }
}
