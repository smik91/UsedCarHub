using UsedCarHub.Common.Errors;
using Microsoft.EntityFrameworkCore;
using UsedCarHub.Common.Errors;
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
            if (user.Email == null)
            {
                return Result<UserEntity>.Failure(AccountError.EmailIsNull);
            }
            if (await _dbContext.Users.AnyAsync(x => x.Email == user.Email))
            {
                return Result<UserEntity>.Failure(AccountError.SameEmail);
            }
            if (user.UserName == null)
            {
                return Result<UserEntity>.Failure(AccountError.UserNameIsNull);
            }
            if (await _dbContext.Users.AnyAsync(x => x.UserName == user.UserName))
            {
                return Result<UserEntity>.Failure(AccountError.SameUserName);
            }
            if (user.PasswordHash == null)
            {
                return Result<UserEntity>.Failure(AccountError.PasswordHashIsNull);
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
                return Result<UserEntity>.Failure(AccountError.NotFountByUserName);
            }

            return Result<UserEntity>.Success(user);
        }

        public async Task<Result<UserEntity>> DeleteAsync(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return Result<UserEntity>.Failure(AccountError.NotFoundById);
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
                return Result<UserEntity>.Failure(AccountError.NotFoundById);
            }
            return Result<UserEntity>.Success(user);
        }

        public async Task<IEnumerable<UserEntity>> GetWithCarsAsync()
        {
            return await _dbContext.Users.Include(x => x.CarsForSale).AsNoTracking().ToListAsync();
        }
    }
}
