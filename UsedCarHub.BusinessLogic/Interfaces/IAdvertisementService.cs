using UsedCarHub.BusinessLogic.DTOs;
using UsedCarHub.Common.Results;

namespace UsedCarHub.BusinessLogic.Interfaces
{
    public interface IAdvertisementService
    {
        Task<Result<AdvertisementDto>> AddAsync(AddAdvertisementDto registerUserDto);
        Task<Result<UpdateAdvertisementDto>> UpdateAsync(string advertisementId, UpdateAdvertisementDto updateUserDto);
        Task<Result<AdvertisementInfoDto>> GetInfoAsync(string advertisementId);
        Task<Result<string>> DeleteAsync(string advertisementId);
    }
}