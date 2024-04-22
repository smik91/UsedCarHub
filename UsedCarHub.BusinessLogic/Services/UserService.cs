using UsedCarHub.BusinessLogic.DTOs;
using UsedCarHub.BusinessLogic.Interfaces;
using UsedCarHub.Common.Errors;
using UsedCarHub.Common.Results;
using UsedCarHub.Repository.Interfaces;

namespace UsedCarHub.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task<Result<UserDto>> GetByUserName(string userName)
        {
            var findByUsernameResult = await _userRepository.GetByUserNameAsync(userName);
            if (findByUsernameResult.IsSuccess)
            {
                // ЗАЮЗАТЬ АВТОМАППЕР(РАЗОБРАТЬСЯ ПОЗЖЕ)
                UserDto userDto = new UserDto();
                userDto.Id = findByUsernameResult.Value.Id;
                userDto.UserName = findByUsernameResult.Value.UserName;
                userDto.Email = findByUsernameResult.Value.Email;
                userDto.FirstName = findByUsernameResult.Value.FirstName;
                userDto.LastName = findByUsernameResult.Value.LastName;
                // --------------------------
                return Result<UserDto>.Success(userDto);
            }

            return Result<UserDto>.Failure(AccountError.NotFountByUserName);
        }
    }
}