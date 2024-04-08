using CarAPI.Database;
using CarAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task Create(User newUser)
        {
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (user != null)
            {
               _context.Users.Remove(user);
                _context.SaveChanges();
                return true;
            }
            else 
            { 
                return false; 
            }
        }

        public async Task<IEnumerable<User>> GetAsync()
        {
            var allUsers = await _context.Users.ToListAsync();
            return allUsers;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public async Task Update(User newUser, int id)
        {
            throw new NotImplementedException();
        }
    }
}
