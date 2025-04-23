using ProjectE.DTO.FeedbackDtos;

namespace ProjectE.Business.Abstract
{
    public interface IFeedbackService
    {
        Task<string> CreateFeedbackAsync(CreateFeedbackDto dto, string userId);
        Task<List<ResultFeedbackDto>> GetFeedbacksByCompanyIdAsync(string companyId);
        Task<double> GetCompanyAverageRatingAsync(string companyId);
        Task<string> DeleteFeedbackByIdAsync(string feedbackId);
        Task<string> UpdateFeedbackAsync(UpdateFeedbackDto dto, string userId);
        Task<string> ReplyToFeedbackAsync(ReplyToFeedbackDto dto, string companyId);
        Task<string> DeleteFeedbackByUserAsync(string feedbackId, string userId);
        Task<List<ResultFeedbackDto>> GetFeedbacksByUserAsync(string userId);





    }
}
