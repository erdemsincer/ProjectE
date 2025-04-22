using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectE.Business.Abstract;
using ProjectE.DTO.UserDtos;
using System.Security.Claims;

namespace ProjectE.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await _userService.GetProfileAsync(userId);

            if (profile == null) return NotFound();
            return Ok(profile);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile(UpdateUserDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _userService.UpdateProfileAsync(dto, userId);
            return Ok(new { message = result });
        }
    }
}
