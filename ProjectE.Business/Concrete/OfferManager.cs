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
    }
}
