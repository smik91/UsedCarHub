using UsedCarHub.BusinessLogic.DTOs;
using UsedCarHub.Common.Results;

namespace UsedCarHub.BusinessLogic.Interfaces
{
    public interface IAccountService
    {
        Task<Result<string>> RegisterAsync(RegisterUserDto registerUserDto);
        Task<Result<UserDto>> LoginAsync(LoginUserDto loginUserDto);
        Task<Result<UpdateUserDto>> UpdateAsync(string userId, UpdateUserDto updateUserDto);
        Task<Result<InfoUserDto>> GetInfoAsync(string userId);
        Task<Result<string>> DeleteAsync(string userId, string password);
        Task<Result<string>> GiveSellerRole(string userId);
    }
}