using MongoDB.Driver;
using ProjectE.Business.Abstract;
using ProjectE.DataAccess.Context;
using ProjectE.DTO.CompanyDtos;
using ProjectE.DTO.FeedbackDtos;
using ProjectE.Entity.Entities;

namespace ProjectE.Business.Concrete
{
    public class FeedbackManager : IFeedbackService
    {
        private readonly IMongoCollection<Feedback> _feedbacks;
        private readonly IMongoCollection<Offer> _offers;
        private readonly IMongoCollection<FeedbackReaction> _reactions;

        public FeedbackManager(MongoDbContext context, IMongoCollection<FeedbackReaction> reactions)
        {
            _feedbacks = context.Feedbacks;
            _offers = context.Offers;
            _reactions = reactions;
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
            var feedbacks = await _feedbacks
                .Find(x => x.CompanyId == companyId)
                .SortByDescending(x => x.LikeCount) // ✅ Sıralama buradan başlar
                .ThenByDescending(x => x.Rating)
                .ThenByDescending(x => x.CreatedAt)
                .ToListAsync();

            return feedbacks.Select(f => new ResultFeedbackDto
            {
                Id = f.Id,
                OfferId = f.OfferId,
                UserId = f.UserId,
                CompanyId = f.CompanyId,
                Comment = f.Comment,
                Rating = f.Rating,
                CreatedAt = f.CreatedAt,
                FeedbackReply = f.FeedbackReply,
                LikeCount = f.LikeCount,
                DislikeCount = f.DislikeCount
            }).ToList();
        }

        public async Task<double> GetCompanyAverageRatingAsync(string companyId)
        {
            var feedbacks = await _feedbacks.Find(x => x.CompanyId == companyId).ToListAsync();

            if (!feedbacks.Any())
                return 0;

            return Math.Round(feedbacks.Average(x => x.Rating), 1); // 1 ondalık basamakla
        }

        public async Task<string> DeleteFeedbackByIdAsync(string feedbackId)
        {
            var result = await _feedbacks.DeleteOneAsync(x => x.Id == feedbackId);

            return result.DeletedCount > 0
                ? "Yorum silindi."
                : "Yorum bulunamadı.";
        }

        public async Task<string> UpdateFeedbackAsync(UpdateFeedbackDto dto, string userId)
        {
            var feedback = await _feedbacks.Find(x => x.Id == dto.FeedbackId).FirstOrDefaultAsync();
            if (feedback == null)
                return "Yorum bulunamadı.";

            if (feedback.UserId != userId)
                return "Bu yorumu güncelleme yetkiniz yok.";

            var update = Builders<Feedback>.Update
                .Set(x => x.Comment, dto.Comment)
                .Set(x => x.Rating, dto.Rating);

            await _feedbacks.UpdateOneAsync(x => x.Id == dto.FeedbackId, update);

            return "Yorum güncellendi.";
        }
        public async Task<string> ReplyToFeedbackAsync(ReplyToFeedbackDto dto, string companyId)
        {
            var feedback = await _feedbacks.Find(x => x.Id == dto.FeedbackId).FirstOrDefaultAsync();

            if (feedback == null)
                return "Yorum bulunamadı.";

            if (feedback.CompanyId != companyId)
                return "Bu yoruma cevap yazamazsınız.";

            var update = Builders<Feedback>.Update
                .Set(x => x.FeedbackReply, dto.Reply);

            await _feedbacks.UpdateOneAsync(x => x.Id == dto.FeedbackId, update);

            return "Firma cevabı eklendi.";
        }
        public async Task<string> DeleteFeedbackByUserAsync(string feedbackId, string userId)
        {
            var feedback = await _feedbacks.Find(x => x.Id == feedbackId).FirstOrDefaultAsync();

            if (feedback == null)
                return "Yorum bulunamadı.";

            if (feedback.UserId != userId)
                return "Bu yorumu silme yetkiniz yok.";

            var result = await _feedbacks.DeleteOneAsync(x => x.Id == feedbackId);
            return result.DeletedCount > 0 ? "Yorum silindi." : "Silinemedi.";
        }
        public async Task<List<ResultFeedbackDto>> GetFeedbacksByUserAsync(string userId)
        {
            var feedbacks = await _feedbacks.Find(x => x.UserId == userId).ToListAsync();

            return feedbacks.Select(f => new ResultFeedbackDto
            {
                Id = f.Id,
                OfferId = f.OfferId,
                UserId = f.UserId,
                CompanyId = f.CompanyId,
                Comment = f.Comment,
                Rating = f.Rating,
                CreatedAt = f.CreatedAt,
                FeedbackReply = f.FeedbackReply
            }).ToList();
        }
        public async Task<List<ResultFeedbackDto>> GetAllFeedbacksAsync()
        {
            var feedbacks = await _feedbacks.Find(_ => true).ToListAsync();

            return feedbacks.Select(f => new ResultFeedbackDto
            {
                Id = f.Id,
                OfferId = f.OfferId,
                UserId = f.UserId,
                CompanyId = f.CompanyId,
                Comment = f.Comment,
                Rating = f.Rating,
                CreatedAt = f.CreatedAt,
                FeedbackReply = f.FeedbackReply
            }).ToList();
        }
        public async Task<CompanyStatsDto> GetCompanyStatsAsync(string companyId)
        {
            var feedbacks = await _feedbacks.Find(x => x.CompanyId == companyId).ToListAsync();

            var average = feedbacks.Any()
                ? Math.Round(feedbacks.Average(x => x.Rating), 1)
                : 0;

            return new CompanyStatsDto
            {
                CompanyId = companyId,
                AverageRating = average,
                FeedbackCount = feedbacks.Count
            };
        }

        public async Task<string> AddReactionToFeedbackAsync(LikeFeedbackDto dto)
        {
            var feedback = await _feedbacks.Find(x => x.Id == dto.FeedbackId).FirstOrDefaultAsync();

            if (feedback == null)
                return "Yorum bulunamadı.";

            var update = dto.IsLike
                ? Builders<Feedback>.Update.Inc(x => x.LikeCount, 1)
                : Builders<Feedback>.Update.Inc(x => x.DislikeCount, 1);

            await _feedbacks.UpdateOneAsync(x => x.Id == dto.FeedbackId, update);
            return dto.IsLike ? "Beğenildi olarak işaretlendi." : "Yararsız olarak işaretlendi.";
        }
        public async Task<CompanyFeedbackPanelDto> GetPanelDataForCompanyAsync(string companyId)
        {
            var feedbacks = await _feedbacks
                .Find(x => x.CompanyId == companyId)
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync();

            var avg = feedbacks.Any() ? Math.Round(feedbacks.Average(x => x.Rating), 1) : 0;

            return new CompanyFeedbackPanelDto
            {
                AverageRating = avg,
                FeedbackCount = feedbacks.Count,
                Feedbacks = feedbacks.Select(f => new ResultFeedbackDto
                {
                    Id = f.Id,
                    OfferId = f.OfferId,
                    UserId = f.UserId,
                    CompanyId = f.CompanyId,
                    Comment = f.Comment,
                    Rating = f.Rating,
                    CreatedAt = f.CreatedAt,
                    FeedbackReply = f.FeedbackReply,
                    LikeCount = f.LikeCount,
                    DislikeCount = f.DislikeCount
                }).ToList()
            };
        }
        public async Task<string> AddReactionToFeedbackAsync(LikeFeedbackDto dto, string userId)
        {
            var feedback = await _feedbacks.Find(x => x.Id == dto.FeedbackId).FirstOrDefaultAsync();

            if (feedback == null)
                return "Yorum bulunamadı.";

            // Aynı kullanıcı aynı yoruma daha önce oy vermiş mi?
            var existing = await _reactions
                .Find(x => x.FeedbackId == dto.FeedbackId && x.UserId == userId)
                .FirstOrDefaultAsync();

            if (existing != null)
                return "Bu yoruma daha önce tepki verdiniz.";

            // Tepkiyi kaydet
            var reaction = new FeedbackReaction
            {
                FeedbackId = dto.FeedbackId,
                UserId = userId,
                IsLike = dto.IsLike
            };
            await _reactions.InsertOneAsync(reaction);

            // Sayacı güncelle
            var update = dto.IsLike
                ? Builders<Feedback>.Update.Inc(x => x.LikeCount, 1)
                : Builders<Feedback>.Update.Inc(x => x.DislikeCount, 1);

            await _feedbacks.UpdateOneAsync(x => x.Id == dto.FeedbackId, update);

            return dto.IsLike ? "Beğenildi olarak işaretlendi." : "Yararsız olarak işaretlendi.";
        }











    }
}
