using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectE.Business.Abstract;
using ProjectE.DTO.OfferDtos;

namespace ProjectE.WebAPI.Controllers
{
    [Authorize] // İleride sadece admin rolü eklenebilir
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IOfferService _offerService;
        private readonly IFeedbackService _feedbackService;

        public AdminController(IOfferService offerService, IFeedbackService feedbackService)
        {
            _offerService = offerService;
            _feedbackService = feedbackService;
        }

        [HttpPut("approve-offer")]
        public async Task<IActionResult> ApproveOffer([FromBody] ApproveOfferDto dto)
        {
            var result = await _offerService.ApproveOfferAsync(dto);
            return Ok(new { message = result });
        }

        [Authorize] // ileride admin rol kontrolü eklenebilir
        [HttpDelete("delete-feedback/{feedbackId}")]
        public async Task<IActionResult> DeleteFeedback(string feedbackId)
        {
            var result = await _feedbackService.DeleteFeedbackByIdAsync(feedbackId);
            return Ok(new { message = result });
        }

    }
}
