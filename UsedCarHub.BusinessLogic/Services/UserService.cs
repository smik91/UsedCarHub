using UsedCarHub.BusinessLogic.DTOs;
using UsedCarHub.BusinessLogic.Interfaces;
using UsedCarHub.Common.Errors;
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
            var AddAsyncResult = await _userRepository.AddAsync(user);
            if (AddAsyncResult.IsSuccess)
            {
                // ЗАЮЗАТЬ АВТОМАППЕР(РАЗОБРАТЬСЯ ПОЗЖЕ)
                UserDto userDto = new UserDto();
                userDto.UserName = AddAsyncResult.Value.UserName;
                userDto.Email = AddAsyncResult.Value.Email;
                //------------------
                return Result<UserDto>.Success(userDto);
            }

            return Result<UserDto>.Failure(AddAsyncResult.ExecutionError);
        }

        public async Task<Result<string>> LoginAsync(string userName, string password)
        {
            var getByUserNameResult = await _userRepository.GetByUserNameAsync(userName);
            if (getByUserNameResult.IsSuccess)
            {
                var isPasswordVerified  = _passwordHasher.VerifyPassword(password, getByUserNameResult.Value.PasswordHash);
                if (isPasswordVerified  == false)
                {
                    return Result<string>.Failure(AccountError.InvalidPassword);
                }
                string token = _jwtProvider.GenerateJwtToken(getByUserNameResult.Value.UserName, getByUserNameResult.Value.Email);
                return Result<string>.Success(token);
            }
            return Result<string>.Failure(getByUserNameResult.ExecutionError);
        }
    }
}