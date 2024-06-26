using Microsoft.EntityFrameworkCore;
using UsedCarHub.Common.Errors;
using UsedCarHub.Common.Results;
using UsedCarHub.Domain;
using UsedCarHub.Domain.Entities;
using UsedCarHub.Repository.Interfaces;

namespace UsedCarHub.Repository.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly AppDbContext _dbContext;

        public CarRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<Result<CarEntity>> GetAsync(int carId)
        {
            var car = await _dbContext.Cars.AsNoTracking().FirstOrDefaultAsync(x => x.Id == carId);
            if (car == null)
            {
                return Result<CarEntity>.Failure(CarError.NotFoundById);
            }

            return Result<CarEntity>.Success(car);
        }

        public async Task<Result<CarEntity>> DeleteAsync(int carId)
        {
            var car = await _dbContext.Cars.FirstOrDefaultAsync(x => x.Id == carId);
            if (car == null)
            {
                return Result<CarEntity>.Failure(CarError.NotFoundById);
            }

            _dbContext.Cars.Remove(car);
            await _dbContext.SaveChangesAsync();
            return Result<CarEntity>.Success(car);
        }

        public async Task<Result<CarEntity>> AddAsync(CarEntity car)
        {
            if (await _dbContext.Cars.AnyAsync(x => x.VIN == car.VIN))
            {
                return Result<CarEntity>.Failure(CarError.SameVIN);
            }

            await _dbContext.Cars.AddAsync(car);
            await _dbContext.SaveChangesAsync();
            return Result<CarEntity>.Success(car);
        }

        public async Task<Result<CarEntity>> UpdateAsync(int carId, CarEntity updateCar)
        {
            var car = await _dbContext.Cars.FirstOrDefaultAsync(x => x.Id == carId);
            if (car == null)
            {
                return Result<CarEntity>.Failure(CarError.NotFoundById);
            }

            if (await _dbContext.Cars.AnyAsync(x => x.VIN == updateCar.VIN))
            {
                return Result<CarEntity>.Failure(CarError.SameVIN);
            }

            car.RegistrationNumber = updateCar.RegistrationNumber;
            car.VIN = updateCar.VIN;
            car.Mark = updateCar.Mark;
            car.Model = updateCar.Model;
            car.YearOfProduction = updateCar.YearOfProduction;
            car.TransmissionType = updateCar.TransmissionType;
            car.EngineCapacity = updateCar.EngineCapacity;
            car.Mileage = updateCar.Mileage;
            await _dbContext.SaveChangesAsync();
            return Result<CarEntity>.Success(car);
        }
    }
}