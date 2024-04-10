using Microsoft.EntityFrameworkCore;
using UsedCarHub.Common.Exceptions;
using UsedCarHub.Domain;
using UsedCarHub.Domain.Entities;
using UsedCarHub.Repository.Interfaces;

namespace UsedCarHub.Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserEntity> CreateAsync(UserEntity user)
        {
            if (await _dbContext.Users.AnyAsync(x => x.Email == user.Email))
            {
                throw new BadRequestException("A user with this email already exists");
            }
            if (user.FirstName == null )
            {
                throw new BadRequestException("First name is required");
            }
            if (user.LastName == null)
            {
                throw new BadRequestException("Last name is required");
            }
            if (user.Email == null)
            {
                throw new BadRequestException("Email is required");
            }
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                throw new NotFoundException("The user with this Id does not exist");
            }
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserEntity>> GetAllAsync()
        {
            return await _dbContext.Users.AsNoTracking().ToListAsync();
        }

        public async Task<UserEntity> GetAsync(int id)
        {
            var user = await _dbContext.Users.AsNoTracking().Include(x => x.CarsForSale).FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                throw new NotFoundException("The user with this Id does not exist");
            }
            return user;
        }

        public async Task<IEnumerable<UserEntity>> GetWithCarsAsync()
        {
            return await _dbContext.Users.Include(x => x.CarsForSale).AsNoTracking().ToListAsync();
        }
    }
}
