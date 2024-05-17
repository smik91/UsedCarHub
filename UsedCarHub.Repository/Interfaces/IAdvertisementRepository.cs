using UsedCarHub.Common.Results;
using UsedCarHub.Domain.Entities;

namespace UsedCarHub.Repository.Interfaces
{
    public interface IAdvertisementRepository
    {
        public Task<IEnumerable<AdvertisementEntity>> GetAllAsync();
        public Task<Result<AdvertisementEntity>> GetAsync(int advertisementId);
        public Task<Result<AdvertisementEntity>> DeleteAsync(int advertisementId);
        public Task<Result<AdvertisementEntity>> AddAsync(AdvertisementEntity advertisement);
    }
}