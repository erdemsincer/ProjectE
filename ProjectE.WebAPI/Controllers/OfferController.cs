using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ProjectE.Business.Abstract;
using ProjectE.DataAccess.Context;
using ProjectE.DTO.OfferDtos;
using System.Security.Claims;

namespace ProjectE.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        // ✅ Teklif oluştur (Sadece giriş yapmış kullanıcı)
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateOffer([FromBody] CreateOfferDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _offerService.CreateOfferAsync(dto, userId);
            return Ok(new { message = result });
        }

        // ✅ Tüm teklifleri getir (şimdilik test için)
        [HttpGet("all")]
        public async Task<IActionResult> GetAllOffers()
        {
            var offers = await _offerService.GetAllOffersAsync();
            return Ok(offers);
        }

        [Authorize]
        [HttpGet("for-company")]
        public async Task<IActionResult> GetOffersForCompany()
        {
            var companyId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Firma bilgilerini çekmek için DbContext üzerinden firma koleksiyonunu sorgula (örnek çözüm):
            var context = HttpContext.RequestServices.GetService<MongoDbContext>();
            var company = await context.Companies.Find(x => x.Id == companyId).FirstOrDefaultAsync();

            if (company == null)
                return Unauthorized("Firma bulunamadı.");

            var offers = await _offerService.GetOffersForCompanyAsync(companyId, company.IsAdvertiser);
            return Ok(offers);
        }

        [Authorize]
        [HttpPost("assign")]
        public async Task<IActionResult> AssignCompanyToOffer([FromBody] AssignCompanyToOfferDto dto)
        {
            var companyId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _offerService.AssignCompanyToOfferAsync(dto.OfferId, companyId);
            return Ok(new { message = result });
        }

        [Authorize]
        [HttpGet("my-offers")]
        public async Task<IActionResult> GetMyOffers()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var offers = await _offerService.GetOffersByUserAsync(userId);
            return Ok(offers);
        }

        [Authorize]
        [HttpGet("assigned-to-me")]
        public async Task<IActionResult> GetAssignedOffers()
        {
            var companyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var offers = await _offerService.GetOffersByCompanyAsync(companyId);
            return Ok(offers);
        }



    }
}
