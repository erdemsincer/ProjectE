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

        public AdminController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpPut("approve-offer")]
        public async Task<IActionResult> ApproveOffer([FromBody] ApproveOfferDto dto)
        {
            var result = await _offerService.ApproveOfferAsync(dto);
            return Ok(new { message = result });
        }
    }
}
