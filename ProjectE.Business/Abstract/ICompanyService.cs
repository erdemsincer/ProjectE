using ProjectE.DTO.CompanyDtos;

namespace ProjectE.Business.Abstract
{
    public interface ICompanyService
    {
        Task<UpdateCompanyDto> GetCompanyProfileAsync(string companyId);
        Task<string> UpdateCompanyProfileAsync(UpdateCompanyDto dto, string companyId);
        Task<List<ResultCompanyDto>> GetAllCompaniesSortedAsync();
        




    }
}
