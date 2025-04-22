using MongoDB.Driver;
using ProjectE.Business.Abstract;
using ProjectE.Business.Helpers;
using ProjectE.DataAccess.Context;
using ProjectE.DTO.CompanyDtos;
using ProjectE.Entity.Entities;

namespace ProjectE.Business.Concrete
{
    public class CompanyAuthManager : ICompanyAuthService
    {
        private readonly IMongoCollection<Company> _companies;
        private readonly TokenGenerator _tokenGenerator;

        public CompanyAuthManager(MongoDbContext context, TokenGenerator tokenGenerator)
        {
            _companies = context.Companies;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<string> RegisterAsync(RegisterCompanyDto dto)
        {
            var existing = await _companies.Find(x => x.Email == dto.Email).FirstOrDefaultAsync();
            if (existing != null)
                return "Bu e-posta ile kayıtlı bir firma zaten var.";

            var company = new Company
            {
                CompanyName = dto.CompanyName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Description = dto.Description,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow
            };

            await _companies.InsertOneAsync(company);
            return "Kayıt başarılı.";
        }

        public async Task<string> LoginAsync(LoginCompanyDto dto)
        {
            var company = await _companies.Find(x => x.Email == dto.Email).FirstOrDefaultAsync();
            if (company == null || !BCrypt.Net.BCrypt.Verify(dto.Password, company.PasswordHash))
                return null;

            // Firma için token üret (CompanyId yerine UserId gibi claim olabilir)
            var token = _tokenGenerator.GenerateToken(new User
            {
                Id = company.Id,
                FullName = company.CompanyName,
                Email = company.Email
            });

            return token;
        }
    }
}
