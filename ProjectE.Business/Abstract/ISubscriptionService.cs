using ProjectE.DTO.SubscriptionDtos;

namespace ProjectE.Business.Abstract
{
    public interface ISubscriptionService
    {
        Task<string> StartSubscriptionAsync(CreateSubscriptionDto dto, string companyId);
        Task<ResultSubscriptionDto> GetMySubscriptionAsync(string companyId);
    }
}
