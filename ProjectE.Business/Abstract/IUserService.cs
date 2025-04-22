using ProjectE.DTO.UserDtos;

namespace ProjectE.Business.Abstract
{
    public interface IUserService
    {
        Task<UpdateUserDto> GetProfileAsync(string userId);
        Task<string> UpdateProfileAsync(UpdateUserDto dto, string userId);
    }
}
