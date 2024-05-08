using UsedCarHub.BusinessLogic.DTOs;
using UsedCarHub.Common.Results;

namespace UsedCarHub.BusinessLogic.Interfaces
{
    public interface IAdvertisementService
    {
        Task<Result<AdvertisementDto>> AddAsync(AddAdvertisementDto addAdvertisementDto);
        Task<Result<UpdateAdvertisementDto>> UpdateAsync(int advertisementId, UpdateAdvertisementDto updateAdvertisementDto);
        Task<Result<InfoAdvertisementDto>> GetInfoAsync(int advertisementId);
        Task<Result<string>> DeleteAsync(int advertisementId);
    }
}