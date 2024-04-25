using UsedCarHub.Common.Results;
using UsedCarHub.Domain.Entities;

namespace UsedCarHub.Repository.Interfaces
{
    public interface IUserRepository
    {
        public Task<IEnumerable<UserEntity>> GetAllAsync();
        public Task<Result<UserEntity>> GetAsync(int id);
        public Task<IEnumerable<UserEntity>> GetWithCarsAsync();
        public Task<Result<UserEntity>> DeleteAsync(int id);
        public Task<Result<UserEntity>> AddAsync(UserEntity user);
        public Task<Result<UserEntity>> GetByUserNameAsync(string userName);
    }
}
