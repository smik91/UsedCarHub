using UsedCarHub.BusinessLogic.DTOs;
using UsedCarHub.Common.Results;

namespace UsedCarHub.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserDto>> GetByUserName(string userName);
    }
}