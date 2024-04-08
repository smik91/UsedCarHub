using CarAPI.Database;
using CarAPI.Models;

namespace CarAPI.Services
{
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetAsync();
        public Task<User?> GetByIdAsync(int id);
        public Task<bool> Delete(int id);
        public Task Update(User newUser, int id);
        public Task Create(User newUser);
    }
}
