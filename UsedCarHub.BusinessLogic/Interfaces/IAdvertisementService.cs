using UsedCarHub.BusinessLogic.DTOs;
using UsedCarHub.Common.Results;

namespace UsedCarHub.BusinessLogic.Interfaces
{
    public interface IAdvertisementService
    {
        Task<Result<AdvertisementDto>> AddAsync(AddAdvertisementDto addAdvertisementDto, string currentUserId);
        Task<Result<InfoAdvertisementDto>> GetInfoAsync(int advertisementId);
        Task<Result<List<InfoAdvertisementDto>>> GetAllInfoAsync();
        Task<Result<string>> UpdateAsync(int advertisementId, UpdateAdvertisementDto updateAdvertisementDto);
        Task<Result<string>> DeleteAsync(int advertisementId);
    }
}