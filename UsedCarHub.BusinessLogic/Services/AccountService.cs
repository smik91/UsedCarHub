using UsedCarHub.BusinessLogic.DTOs;
using UsedCarHub.BusinessLogic.Interfaces;
using UsedCarHub.Common.Errors;
using UsedCarHub.Common.Interfaces;
using UsedCarHub.Common.Results;
using UsedCarHub.Domain.Entities;
using UsedCarHub.Repository.Interfaces;

namespace UsedCarHub.BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;

        public AccountService(IPasswordHasher passwordHasher,IUserRepository userRepository,IJwtProvider jwtProvider)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }
        
        public async Task<Result<RegisterUserDto>> RegisterAsync(string userName, string email, string password)
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
                RegisterUserDto registerUserDto = new RegisterUserDto();
                registerUserDto.UserName = AddAsyncResult.Value.UserName;
                registerUserDto.Email = AddAsyncResult.Value.Email;
                //------------------
                return Result<RegisterUserDto>.Success(registerUserDto);
            }

            return Result<RegisterUserDto>.Failure(AddAsyncResult.ExecutionError);
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