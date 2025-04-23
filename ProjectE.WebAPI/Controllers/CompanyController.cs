using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectE.Business.Abstract;
using ProjectE.DTO.CompanyDtos;
using System.Security.Claims;

namespace ProjectE.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyAuthService _companyAuthService;
        private readonly ICompanyService _companyService;
        private readonly IFeedbackService _feedbackService;

        public CompanyController(ICompanyAuthService companyAuthService, ICompanyService companyService, IFeedbackService feedbackService)
        {
            _companyAuthService = companyAuthService;
            _companyService = companyService;
            _feedbackService = feedbackService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCompanyDto dto)
        {
            var result = await _companyAuthService.RegisterAsync(dto);
            return Ok(new { message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCompanyDto dto)
        {
            var token = await _companyAuthService.LoginAsync(dto);
            if (token == null)
                return Unauthorized("E-posta veya şifre hatalı");

            return Ok(new { token });
        }
        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var companyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await _companyService.GetCompanyProfileAsync(companyId);

            if (profile == null)
                return NotFound();

            return Ok(profile);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateCompanyDto dto)
        {
            var companyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _companyService.UpdateCompanyProfileAsync(dto, companyId);
            return Ok(new { message = result });
        }
        [AllowAnonymous]
        [HttpGet("sorted")]
        public async Task<IActionResult> GetSortedCompanies()
        {
            var companies = await _companyService.GetAllCompaniesSortedAsync();
            return Ok(companies);
        }
        [AllowAnonymous]
        [HttpGet("{companyId}/stats")]
        public async Task<IActionResult> GetCompanyStats(string companyId)
        {
            var stats = await _feedbackService.GetCompanyStatsAsync(companyId);
            return Ok(stats);
        }


    }
}
