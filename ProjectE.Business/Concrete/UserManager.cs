using MongoDB.Driver;
using ProjectE.Business.Abstract;
using ProjectE.DataAccess.Context;
using ProjectE.DTO.UserDtos;
using ProjectE.Entity.Entities;

namespace ProjectE.Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IMongoCollection<User> _users;

        public UserManager(MongoDbContext context)
        {
            _users = context.Users;
        }

        public async Task<UpdateUserDto> GetProfileAsync(string userId)
        {
            var user = await _users.Find(x => x.Id == userId).FirstOrDefaultAsync();
            if (user == null) return null;

            return new UpdateUserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<string> UpdateProfileAsync(UpdateUserDto dto, string userId)
        {
            var user = await _users.Find(x => x.Id == userId).FirstOrDefaultAsync();
            if (user == null) return "Kullanıcı bulunamadı.";

            user.FullName = dto.FullName;
            user.PhoneNumber = dto.PhoneNumber;

            await _users.ReplaceOneAsync(x => x.Id == userId, user);
            return "Profil güncellendi.";
        }
    }
}
