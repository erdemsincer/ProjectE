using ProjectE.DTO.OfferDtos;

namespace ProjectE.Business.Abstract
{
    public interface IOfferService
    {
        Task<string> CreateOfferAsync(CreateOfferDto dto, string userId);
        Task<List<ResultOfferDto>> GetAllOffersAsync(); // Tüm teklifleri getir (şimdilik admin/firma için)
    }
}
