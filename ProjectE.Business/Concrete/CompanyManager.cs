using MongoDB.Driver;
using ProjectE.Business.Abstract;
using ProjectE.DataAccess.Context;
using ProjectE.DTO.CompanyDtos;
using ProjectE.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectE.Business.Concrete
{
    public class CompanyManager : ICompanyService
    {
        private readonly IMongoCollection<Company> _companies;

        public CompanyManager(MongoDbContext context)
        {
            _companies = context.Companies;
        }

        public async Task<UpdateCompanyDto> GetCompanyProfileAsync(string companyId)
        {
            var company = await _companies.Find(x => x.Id == companyId).FirstOrDefaultAsync();
            if (company == null) return null;

            return new UpdateCompanyDto
            {
                Id = company.Id,
                CompanyName = company.CompanyName,
                PhoneNumber = company.PhoneNumber,
                Description = company.Description
            };
        }

        public async Task<string> UpdateCompanyProfileAsync(UpdateCompanyDto dto, string companyId)
        {
            var company = await _companies.Find(x => x.Id == companyId).FirstOrDefaultAsync();
            if (company == null) return "Firma bulunamadı.";

            company.CompanyName = dto.CompanyName;
            company.PhoneNumber = dto.PhoneNumber;
            company.Description = dto.Description;

            await _companies.ReplaceOneAsync(x => x.Id == companyId, company);

            return "Firma profili güncellendi.";
        }
    }
}
