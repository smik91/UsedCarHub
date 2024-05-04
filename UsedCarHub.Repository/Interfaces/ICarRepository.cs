using UsedCarHub.Common.Results;
using UsedCarHub.Domain.Entities;

namespace UsedCarHub.Repository.Interfaces
{
    public interface ICarRepository
    {
        public Task<IEnumerable<CarEntity>> GetAllAsync();
        public Task<Result<CarEntity>> GetAsync(int id);
        public Task<IEnumerable<CarEntity>> GetWithOwnerAsync();
        public Task<Result<CarEntity>> DeleteAsync(int id);
        public Task<Result<CarEntity>> AddAsync(CarEntity car);
    }
}