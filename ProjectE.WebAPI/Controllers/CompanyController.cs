using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectE.Business.Abstract;
using ProjectE.DTO.CompanyDtos;

namespace ProjectE.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyAuthService _companyAuthService;

        public CompanyController(ICompanyAuthService companyAuthService)
        {
            _companyAuthService = companyAuthService;
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
    }
}
