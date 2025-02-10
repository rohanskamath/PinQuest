using dotnetcorebackend.Infrastructure.Context;
using dotnetcorebackend.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnetcorebackend.Application.Repositories.UserRepository
{
    public class UserRepositoryImplementation : IUserRepository
    {
        private readonly ApplicationDBContext _applicationDBContext;

        public UserRepositoryImplementation(ApplicationDBContext applicationDB)
        {
            _applicationDBContext = applicationDB;
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            var result = await _applicationDBContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return result;
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            await _applicationDBContext.Users.AddAsync(user);
            await _applicationDBContext.SaveChangesAsync();
            return user;
        }
    }
}
