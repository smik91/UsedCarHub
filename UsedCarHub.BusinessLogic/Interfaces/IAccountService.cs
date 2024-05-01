using UsedCarHub.BusinessLogic.DTOs;
using UsedCarHub.Common.Results;

namespace UsedCarHub.BusinessLogic.Interfaces
{
    public interface IAccountService
    {
        Task<Result<RegisterUserDto>> RegisterAsync(RegisterUserDto registerUserDto);
        Task<Result<string>> LoginAsync(LoginUserDto loginUserDto);
        Task<Result<UserDto>> UpdateAsync(string userId, UpdateUserDto updateUserDto);
        Task<Result<UserInfoDto>> GetInfoAsync(string userId);
    }
}