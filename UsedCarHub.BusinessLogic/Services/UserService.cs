using UsedCarHub.BusinessLogic.DTOs;
using UsedCarHub.BusinessLogic.Interfaces;
using UsedCarHub.Common.Interfaces;
using UsedCarHub.Common.Results;
using UsedCarHub.Domain.Entities;
using UsedCarHub.Repository.Interfaces;

namespace UsedCarHub.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;

        public UserService(IPasswordHasher passwordHasher,IUserRepository userRepository,IJwtProvider jwtProvider)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }
        public async Task<Result<UserDto>> RegisterAsync(string userName, string email, string password)
        {
            var hashedPassword = _passwordHasher.HashPassword(password);
            // ЗАЮЗАТЬ АВТОМАППЕР(РАЗОБРАТЬСЯ ПОЗЖЕ)
            var user = new UserEntity();
            user.UserName = userName;
            user.Email = email;
            user.PasswordHash = hashedPassword;
            //------------------
            var resultRepository = await _userRepository.AddAsync(user);
            if (resultRepository.IsSuccess)
            {
                // ЗАЮЗАТЬ АВТОМАППЕР(РАЗОБРАТЬСЯ ПОЗЖЕ)
                UserDto userDto = new UserDto();
                userDto.userName = resultRepository.Value.UserName;
                userDto.email = resultRepository.Value.Email;
                //------------------
                return Result<UserDto>.Success(userDto);
            }

            return Result<UserDto>.Failure(resultRepository.ErrorMessage);
        }

        public async Task<Result<string>> LoginAsync(string userName, string password)
        {
            var resultRepository = await _userRepository.GetByUserNameAsync(userName);
            if (resultRepository.IsSuccess)
            {
                bool verifyPassword = _passwordHasher.VerifyPassword(password, resultRepository.Value.PasswordHash);
                if (verifyPassword == false)
                {
                    return Result<string>.Failure("Failed to login");
                }
                string token = _jwtProvider.GenerateJwtToken(resultRepository.Value.UserName, resultRepository.Value.Email);
                return Result<string>.Success(token);
            }
            return Result<string>.Failure(resultRepository.ErrorMessage);
        }
    }
}