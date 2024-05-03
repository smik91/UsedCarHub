using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UsedCarHub.BusinessLogic.DTOs;
using UsedCarHub.BusinessLogic.Interfaces;
using UsedCarHub.Common.Errors;
using UsedCarHub.Common.Results;
using UsedCarHub.Domain.Entities;
using UsedCarHub.Repository.Interfaces;

namespace UsedCarHub.BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AccountService(IUnitOfWork unitOfWork,IMapper mapper,ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterUserDto registerUserDto)
        {
            if (await _unitOfWork.UserManager.Users.AnyAsync(x => x.UserName == registerUserDto.UserName))
                return Result<UserDto>.Failure(AccountError.SameUserName);
            
            if (await _unitOfWork.UserManager.Users.AnyAsync(x => x.Email == registerUserDto.Email))
                return Result<UserDto>.Failure(AccountError.SameEmail);
            
            if (await _unitOfWork.UserManager.Users.AnyAsync(x => x.PhoneNumber == registerUserDto.PhoneNumber))
                return Result<UserDto>.Failure(AccountError.SamePhone);
            
            var user = _mapper.Map<UserEntity>(registerUserDto);
            var result = await _unitOfWork.UserManager.CreateAsync(user, registerUserDto.Password);
            
            if (!result.Succeeded)
                return Result<UserDto>.Failure(AccountError.Addition);
            var roleResult = await _unitOfWork.UserManager.AddToRoleAsync(user, "Purchaser");
            if (!roleResult.Succeeded)
                return Result<UserDto>.Failure(AccountError.AdditionToRole);
            return Result<UserDto>.Success(new UserDto
            {
                UserName = registerUserDto.UserName,
                Token = await _tokenService.CreateTokenAsync(user)
            });
        }

        public async Task<Result<UserDto>> LoginAsync(LoginUserDto loginUserDto)
        {
            var user = await _unitOfWork.UserManager.Users.FirstOrDefaultAsync(x =>
                x.UserName == loginUserDto.UserName);
            if (user == null)
                return Result<UserDto>.Failure(AccountError.NotFountByUserName);
            var result = await _unitOfWork.SignInManager.CheckPasswordSignInAsync(user, loginUserDto.Password, false);
            if (!result.Succeeded)
                return Result<UserDto>.Failure(AccountError.InvalidPasswordOrUserName);
            return Result<UserDto>.Success(new UserDto
            {
                UserName = loginUserDto.UserName,
                Token = await _tokenService.CreateTokenAsync(user)
            });
        }

        public Task<Result<UserDto>> UpdateAsync(string userId, UpdateUserDto updateUserDto)
        {
            throw new NotImplementedException();
        }

        public Task<Result<UserInfoDto>> GetInfoAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}