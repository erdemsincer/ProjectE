using ProjectE.DTO.CompanyDtos;

namespace ProjectE.Business.Abstract
{
    public interface ICompanyAuthService
    {
        Task<string> RegisterAsync(RegisterCompanyDto dto);
        Task<string> LoginAsync(LoginCompanyDto dto);
    }
}
