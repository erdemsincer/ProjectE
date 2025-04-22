using MongoDB.Driver;
using Org.BouncyCastle.Crypto.Generators;
using ProjectE.Business.Abstract;
using ProjectE.DataAccess.Context;
using ProjectE.DTO.UserDtos;
using ProjectE.Entity.Entities;

namespace ProjectE.Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IMongoCollection<User> _users;

        public AuthManager(MongoDbContext context)
        {
            _users = context.Users;
        }

        public async Task<string> RegisterAsync(RegisterUserDto dto)
        {
            var existingUser = await _users.Find(x => x.Email == dto.Email).FirstOrDefaultAsync();
            if (existingUser != null)
                return "Bu e-posta ile kayıtlı bir kullanıcı zaten var.";

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IsActive = true
            };

            await _users.InsertOneAsync(user);
            return "Kayıt başarılı.";
        }

        public async Task<string> LoginAsync(LoginUserDto dto)
        {
            var user = await _users.Find(x => x.Email == dto.Email).FirstOrDefaultAsync();
            if (user == null)
                return "Kullanıcı bulunamadı.";

            var isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!isValid)
                return "Şifre hatalı.";

            return "Giriş başarılı.";
        }
    }
}
