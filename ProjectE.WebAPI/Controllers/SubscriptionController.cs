using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectE.Business.Abstract;
using ProjectE.DTO.SubscriptionDtos;
using System.Security.Claims;

namespace ProjectE.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartSubscription([FromBody] CreateSubscriptionDto dto)
        {
            var companyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _subscriptionService.StartSubscriptionAsync(dto, companyId);
            return Ok(new { message = result });
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMySubscription()
        {
            var companyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var subscription = await _subscriptionService.GetMySubscriptionAsync(companyId);

            if (subscription == null)
                return NotFound("Aktif aboneliğiniz bulunamadı.");

            return Ok(subscription);
        }

        [Authorize]
        [HttpPost("expire-check")]
        public async Task<IActionResult> CheckAndExpire()
        {
            await _subscriptionService.CheckAndExpireSubscriptionsAsync();
            return Ok(new { message = "Kontrol tamamlandı. Süresi biten abonelikler kapatıldı." });
        }

    }
}
