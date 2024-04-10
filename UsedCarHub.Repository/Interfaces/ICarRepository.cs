using UsedCarHub.Domain.Entities;

namespace UsedCarHub.Repository.Interfaces
{
    public interface ICarRepository
    {
        public Task<IEnumerable<CarEntity>> GetAllAsync();
        public Task<CarEntity> GetAsync(int id);
        public Task<IEnumerable<CarEntity>> GetWithOwnerAsync();
        public Task DeleteAsync(int id);
        public Task<CarEntity> CreateAsync(CarEntity car);
    }
}
