using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectE.Business.Abstract;
using ProjectE.DTO.FeedbackDtos;
using System.Security.Claims;

namespace ProjectE.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        // ✅ Yorum oluşturma (sadece kullanıcı kendisi yorum yapabilir)
        [HttpPost("create")]
        public async Task<IActionResult> CreateFeedback([FromBody] CreateFeedbackDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _feedbackService.CreateFeedbackAsync(dto, userId);
            return Ok(new { message = result });
        }

        // ✅ Firma yorumlarını getirme (herkes görebilir)
        [AllowAnonymous]
        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetFeedbacksByCompany(string companyId)
        {
            var feedbacks = await _feedbackService.GetFeedbacksByCompanyIdAsync(companyId);
            return Ok(feedbacks);
        }
    }
}
