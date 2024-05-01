using UsedCarHub.Domain.Entities;

namespace UsedCarHub.BusinessLogic.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(UserEntity user);
    }
}