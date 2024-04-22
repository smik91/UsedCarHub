using AutoMapper;
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
        private readonly IMapper _mapper; 

        public AccountService(IPasswordHasher passwordHasher,IUserRepository userRepository,IJwtProvider jwtProvider,IMapper mapper)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _mapper = mapper;
        }
        
        public async Task<Result<RegisterUserDto>> RegisterAsync(string userName, string email, string password)
        {
            var hashedPassword = _passwordHasher.HashPassword(password);
            var registerUserDto = new RegisterUserDto(userName, email, hashedPassword);
            var user = _mapper.Map<UserEntity>(registerUserDto);
            var addAsyncResult = await _userRepository.AddAsync(user);
            if (addAsyncResult.IsSuccess)
            {
                registerUserDto = _mapper.Map<RegisterUserDto>(addAsyncResult.Value);
                return Result<RegisterUserDto>.Success(registerUserDto);
            }

            return Result<RegisterUserDto>.Failure(addAsyncResult.ExecutionError);
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