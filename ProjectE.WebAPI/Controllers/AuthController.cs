using Microsoft.AspNetCore.Mvc;
using ProjectE.Business.Abstract;
using ProjectE.DTO.UserDtos;

namespace ProjectE.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(new { message = result });
        }

        [HttpPost("login")]
       
        public async Task<IActionResult> Login(LoginUserDto dto)
        {
            var token = await _authService.LoginAsync(dto);
            if (token == null) return Unauthorized("Hatalı giriş.");

            return Ok(new { token });
        }
    }
}
