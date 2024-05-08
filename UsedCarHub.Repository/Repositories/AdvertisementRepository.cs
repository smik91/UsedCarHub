using Microsoft.EntityFrameworkCore;
using UsedCarHub.Common.Errors;
using UsedCarHub.Common.Results;
using UsedCarHub.Domain;
using UsedCarHub.Domain.Entities;
using UsedCarHub.Repository.Interfaces;

namespace UsedCarHub.Repository.Repositories
{
    public class AdvertisementRepository : IAdvertisementRepository
    {
        private readonly AppDbContext _dbContext;

        public AdvertisementRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<AdvertisementEntity>> AddAsync(AdvertisementEntity advertisement)
        {
            await _dbContext.Advertisements.AddAsync(advertisement);
            await _dbContext.SaveChangesAsync();
            return Result<AdvertisementEntity>.Success(advertisement);
        }

        public async Task<Result<AdvertisementEntity>> DeleteAsync(int advertisementId)
        {
            var advertisement = await _dbContext.Advertisements.FirstOrDefaultAsync(x => x.Id == advertisementId);
            if (advertisement == null)
                return Result<AdvertisementEntity>.Failure(AdvertisementError.NotFoundById);
            _dbContext.Advertisements.Remove(advertisement);
            await _dbContext.SaveChangesAsync();
            return Result<AdvertisementEntity>.Success(advertisement);
        }

        public async Task<IEnumerable<AdvertisementEntity>> GetAllAsync()
        {
            return await _dbContext.Advertisements.Include(x => x.Car)
                .Include(x => x.SellerId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Result<AdvertisementEntity>> GetAsync(int advertisementId)
        {
            var advertisement = await _dbContext.Advertisements.AsNoTracking()
                .Include(x => x.SellerId)
                .Include(x => x.Car)
                .FirstOrDefaultAsync(x => x.Id == advertisementId);
            if (advertisement == null)
                return Result<AdvertisementEntity>.Failure(AdvertisementError.NotFoundById);
            return Result<AdvertisementEntity>.Success(advertisement);
        }
    }
}