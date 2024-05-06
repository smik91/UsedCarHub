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

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterUserDto registerUserDto)
        {
            if (await _unitOfWork.UserManager.Users.AnyAsync(x => x.Email == registerUserDto.Email))
                return Result<UserDto>.Failure(AccountError.SameEmail);

            if (await _unitOfWork.UserManager.Users.AnyAsync(x => x.UserName == registerUserDto.UserName))
                return Result<UserDto>.Failure(AccountError.SameUserName);

            var user = _mapper.Map<UserEntity>(registerUserDto);

            var result = await _unitOfWork.UserManager.CreateAsync(user, registerUserDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => new Error(error.Code, error.Description));
                return Result<UserDto>.Failure(errors);
            }

            var roleResult = await _unitOfWork.UserManager.AddToRoleAsync(user, "Purchaser");

            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors.Select(error => new Error(error.Code, error.Description));
                return Result<UserDto>.Failure(errors);
            }

            var createdUser =
                await _unitOfWork.UserManager.Users.FirstOrDefaultAsync(x => x.UserName == registerUserDto.UserName);
            

            var userDto = new UserDto
            {
                UserName = registerUserDto.UserName,
                Token = await _tokenService.CreateTokenAsync(user),
                Id = createdUser.Id
            };

            return Result<UserDto>.Success(userDto);
        }

        public async Task<Result<UserDto>> LoginAsync(LoginUserDto loginUserDto)
        {
            var user = await _unitOfWork.UserManager.Users.FirstOrDefaultAsync(x =>
                x.UserName == loginUserDto.UserName);
            if (user == null)
                return Result<UserDto>.Failure(AccountError.NotFoundByUserName);

            var result = await _unitOfWork.SignInManager.CheckPasswordSignInAsync(user, loginUserDto.Password, false);
            if (!result.Succeeded)
                return Result<UserDto>.Failure(AccountError.InvalidPasswordOrUserName);
            return Result<UserDto>.Success(new UserDto
            {
                UserName = loginUserDto.UserName,
                Token = await _tokenService.CreateTokenAsync(user),
                Id = user.Id
            });
        }
        
        public async Task<Result<string>> DeleteAsync(string userId)
        {
            var user = await _unitOfWork.UserManager.FindByIdAsync(userId);
            if (user == null)
                return Result<string>.Failure(AccountError.NotFoundById);

            var result = await _unitOfWork.UserManager.DeleteAsync(user);
            if (!result.Succeeded)
                return Result<string>.Failure(result.Errors.Select(x=> new Error(x.Code,x.Description)));
            return Result<string>.Success($"user \"{user.UserName}\" was deleted");
        }

        public async Task<Result<UpdateUserDto>> UpdateAsync(string userId, UpdateUserDto updateUserDto)
        {
            var user = await _unitOfWork.UserManager.FindByIdAsync(userId);
            if (user == null)
                return Result<UpdateUserDto>.Failure(AccountError.NotFoundById);
            _mapper.Map(updateUserDto, user);
            var resultUpdate = await _unitOfWork.UserManager.UpdateAsync(user);
            if (resultUpdate.Succeeded)
                return Result<UpdateUserDto>.Success(updateUserDto);
            return Result<UpdateUserDto>.Failure(resultUpdate.Errors.Select(x => new Error
                (x.Code, x.Code)));
        }

        public async Task<Result<UserInfoDto>> GetInfoAsync(string userId)
        {
            var user = await _unitOfWork.UserManager.FindByIdAsync(userId);
            if (user == null)
                return Result<UserInfoDto>.Failure(AccountError.NotFoundById);
            var userInfoDto = _mapper.Map<UserInfoDto>(user);
            return Result<UserInfoDto>.Success(userInfoDto);
        }
    }
}