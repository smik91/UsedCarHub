using UsedCarHub.Common.Results;
using UsedCarHub.Domain.Entities;

namespace UsedCarHub.Repository.Interfaces
{
    public interface ICarRepository
    {
        public Task<Result<CarEntity>> GetAsync(int carId);
        public Task<Result<CarEntity>> DeleteAsync(int carId);
        public Task<Result<CarEntity>> AddAsync(CarEntity car);
        public Task<Result<CarEntity>> UpdateAsync(int carId, CarEntity updateCar);
    }
}