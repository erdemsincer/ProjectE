﻿using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        [HttpGet("company/{companyId}/average-rating")]
        public async Task<IActionResult> GetAverageRating(string companyId)
        {
            var rating = await _feedbackService.GetCompanyAverageRatingAsync(companyId);
            return Ok(new { averageRating = rating });
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateFeedback([FromBody] UpdateFeedbackDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _feedbackService.UpdateFeedbackAsync(dto, userId);
            return Ok(new { message = result });
        }
        [Authorize]
        [HttpPost("reply")]
        public async Task<IActionResult> ReplyToFeedback([FromBody] ReplyToFeedbackDto dto)
        {
            var companyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _feedbackService.ReplyToFeedbackAsync(dto, companyId);
            return Ok(new { message = result });
        }
        [Authorize]
        [HttpDelete("delete/{feedbackId}")]
        public async Task<IActionResult> DeleteFeedback(string feedbackId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _feedbackService.DeleteFeedbackByUserAsync(feedbackId, userId);
            return Ok(new { message = result });
        }
        [Authorize]
        [HttpGet("my-feedbacks")]
        public async Task<IActionResult> GetMyFeedbacks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var feedbacks = await _feedbackService.GetFeedbacksByUserAsync(userId);
            return Ok(feedbacks);
        }
        [Authorize]
        [HttpPost("react")]
        public async Task<IActionResult> ReactToFeedback([FromBody] LikeFeedbackDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _feedbackService.AddReactionToFeedbackAsync(dto, userId);

            return Ok(new { message = result });
        }

        [Authorize]
        [HttpGet("for-company")]
        public async Task<IActionResult> GetCompanyFeedbackPanel()
        {
            var companyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var data = await _feedbackService.GetPanelDataForCompanyAsync(companyId);
            return Ok(data);
        }






    }
}
