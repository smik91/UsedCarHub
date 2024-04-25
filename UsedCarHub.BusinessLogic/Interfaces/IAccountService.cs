using UsedCarHub.BusinessLogic.DTOs;
using UsedCarHub.Common.Results;

namespace UsedCarHub.BusinessLogic.Interfaces
{
    public interface IAccountService
    {
        Task<Result<RegisterUserDto>> RegisterAsync(string userName, string email, string password);
        Task<Result<string>> LoginAsync(string userName, string password);
    }
}