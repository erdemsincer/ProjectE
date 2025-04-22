using ProjectE.DTO.UserDtos;

namespace ProjectE.Business.Abstract
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterUserDto dto);
        Task<string> LoginAsync(LoginUserDto dto);
    }
}
