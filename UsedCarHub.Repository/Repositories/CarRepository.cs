using Microsoft.EntityFrameworkCore;
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

        public async Task<Result<CarEntity>> AddAsync(CarEntity car)
        {
            if (await _dbContext.Cars.AnyAsync(c => c.RegistrationNumber == car.RegistrationNumber))
            {
                return Result<CarEntity>.Failure("A car with such reg. number already exists");
            }
            if (car.RegistrationNumber == null)
            {
                return Result<CarEntity>.Failure("Registration number is required");
            }
            if (car.Model == null)
            {
                return Result<CarEntity>.Failure("Model is required");
            }
            await _dbContext.Cars.AddAsync(car);
            await _dbContext.SaveChangesAsync();
            return Result<CarEntity>.Success(car);
        }

        public async Task<Result<CarEntity>> DeleteAsync(int id)
        {
            var car = await _dbContext.Cars.FirstOrDefaultAsync(x => x.Id == id);
            if (car == null)
            {
                return Result<CarEntity>.Failure("The car with this Id does not exist");
            }
            _dbContext.Cars.Remove(car);
            await _dbContext.SaveChangesAsync();
            return Result<CarEntity>.Success(car);
        }

        public async Task<IEnumerable<CarEntity>> GetAllAsync()
        {
            return await _dbContext.Cars.AsNoTracking().ToListAsync();
        }

        public async Task<Result<CarEntity>> GetAsync(int id)
        {
            var car = await _dbContext.Cars.AsNoTracking().Include(x => x.Owner).FirstOrDefaultAsync(x => x.Id == id);
            if (car == null)
            {
                return Result<CarEntity>.Failure("The car with this Id does not exist");
            }
            return Result<CarEntity>.Success(car);
        }

        public async Task<IEnumerable<CarEntity>> GetWithOwnerAsync()
        {
            return await _dbContext.Cars.Include(x => x.Owner).AsNoTracking().ToListAsync();
        }
    }
}
