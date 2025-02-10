using dotnetcorebackend.Models;

namespace dotnetcorebackend.Infrastructure.Repositories.UserRepository
{
    public interface IUserRepository
    {
        public Task<User?> GetByEmailAsync(string email);
        public Task<User> RegisterUserAsync(User user);
    }
}
