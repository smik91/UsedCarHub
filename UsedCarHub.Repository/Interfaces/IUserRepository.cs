using UsedCarHub.Domain.Entities;

namespace UsedCarHub.Repository.Interfaces
{
    public interface IUserRepository
    {
        public Task<IEnumerable<UserEntity>> GetAllAsync();
        public Task<UserEntity> GetAsync(int id);
        public Task<IEnumerable<UserEntity>> GetWithCarsAsync();
        public Task DeleteAsync(int id);
        public Task<UserEntity> CreateAsync(UserEntity user);
    }
}
