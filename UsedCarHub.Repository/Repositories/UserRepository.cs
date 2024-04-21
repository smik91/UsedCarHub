using Microsoft.EntityFrameworkCore;
using UsedCarHub.Common.Results;
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

        public async Task<Result<UserEntity>> AddAsync(UserEntity user)
        {
            if (await _dbContext.Users.AnyAsync(x => x.Email == user.Email))
            {
                return Result<UserEntity>.Failure("A user with this email already exists");
            }
            if (user.Email == null)
            {
                return Result<UserEntity>.Failure("Email is required");
            }
            if (await _dbContext.Users.AnyAsync(x => x.UserName == user.UserName))
            {
                return Result<UserEntity>.Failure("A user with this username already exists");
            }
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return Result<UserEntity>.Success(user);
        }

        public async Task<Result<UserEntity>> GetByUserNameAsync(string userName)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                return Result<UserEntity>.Failure("There is no user with such username");
            }

            return Result<UserEntity>.Success(user);
        }

        public async Task<Result<UserEntity>> DeleteAsync(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return Result<UserEntity>.Failure("The user with this Id does not exist");
            }
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return Result<UserEntity>.Success(user);
        }

        public async Task<IEnumerable<UserEntity>> GetAllAsync()
        {
            return await _dbContext.Users.AsNoTracking().ToListAsync();
        }

        public async Task<Result<UserEntity>> GetAsync(int id)
        {
            var user = await _dbContext.Users.AsNoTracking().Include(x => x.CarsForSale).FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return Result<UserEntity>.Failure("The user with this Id does not exist");
            }
            return Result<UserEntity>.Success(user);
        }

        public async Task<IEnumerable<UserEntity>> GetWithCarsAsync()
        {
            return await _dbContext.Users.Include(x => x.CarsForSale).AsNoTracking().ToListAsync();
        }
    }
}
