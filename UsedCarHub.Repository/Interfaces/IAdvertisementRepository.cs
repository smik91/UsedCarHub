using UsedCarHub.Common.Results;
using UsedCarHub.Domain.Entities;

namespace UsedCarHub.Repository.Interfaces
{
    public interface IAdvertisementRepository
    {
        public Task<IEnumerable<AdvertisementEntity>> GetAllAsync();
        public Task<Result<AdvertisementEntity>> GetAsync(int id);
        public Task<Result<AdvertisementEntity>> DeleteAsync(int id);
        public Task<Result<AdvertisementEntity>> AddAsync(AdvertisementEntity advertisement);
    }
}