using ProjectE.DTO.FeedbackDtos;

namespace ProjectE.Business.Abstract
{
    public interface IFeedbackService
    {
        Task<string> CreateFeedbackAsync(CreateFeedbackDto dto, string userId);
        Task<List<ResultFeedbackDto>> GetFeedbacksByCompanyIdAsync(string companyId);
        Task<double> GetCompanyAverageRatingAsync(string companyId);
        Task<string> DeleteFeedbackByIdAsync(string feedbackId);

    }
}
