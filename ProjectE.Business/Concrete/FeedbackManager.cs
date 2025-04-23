using MongoDB.Driver;
using ProjectE.Business.Abstract;
using ProjectE.DataAccess.Context;
using ProjectE.DTO.FeedbackDtos;
using ProjectE.Entity.Entities;

namespace ProjectE.Business.Concrete
{
    public class FeedbackManager : IFeedbackService
    {
        private readonly IMongoCollection<Feedback> _feedbacks;
        private readonly IMongoCollection<Offer> _offers;

        public FeedbackManager(MongoDbContext context)
        {
            _feedbacks = context.Feedbacks;
            _offers = context.Offers;
        }

        public async Task<string> CreateFeedbackAsync(CreateFeedbackDto dto, string userId)
        {
            var offer = await _offers.Find(x => x.Id == dto.OfferId).FirstOrDefaultAsync();
            if (offer == null || offer.UserId != userId || string.IsNullOrEmpty(offer.CompanyId))
                return "Yorum yapma yetkiniz yok.";

            // ✅ Bu kullanıcı bu teklif için zaten yorum yapmış mı?
            var existing = await _feedbacks.Find(x => x.OfferId == dto.OfferId && x.UserId == userId).FirstOrDefaultAsync();
            if (existing != null)
                return "Bu teklife daha önce yorum yaptınız.";

            var feedback = new Feedback
            {
                OfferId = dto.OfferId,
                UserId = userId,
                CompanyId = offer.CompanyId,
                Comment = dto.Comment,
                Rating = dto.Rating,
                CreatedAt = DateTime.UtcNow
            };

            await _feedbacks.InsertOneAsync(feedback);
            return "Yorum başarıyla eklendi.";
        }


        public async Task<List<ResultFeedbackDto>> GetFeedbacksByCompanyIdAsync(string companyId)
        {
            var feedbacks = await _feedbacks.Find(x => x.CompanyId == companyId).ToListAsync();

            return feedbacks.Select(f => new ResultFeedbackDto
            {
                Id = f.Id,
                OfferId = f.OfferId,
                UserId = f.UserId,
                CompanyId = f.CompanyId,
                Comment = f.Comment,
                Rating = f.Rating,
                CreatedAt = f.CreatedAt
            }).ToList();
        }

        public async Task<double> GetCompanyAverageRatingAsync(string companyId)
        {
            var feedbacks = await _feedbacks.Find(x => x.CompanyId == companyId).ToListAsync();

            if (!feedbacks.Any())
                return 0;

            return Math.Round(feedbacks.Average(x => x.Rating), 1); // 1 ondalık basamakla
        }

    }
}
