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

        public async Task<Result<string>> RegisterAsync(RegisterUserDto registerUserDto)
        {
            if (await _unitOfWork.UserManager.Users.AnyAsync(x => x.Email == registerUserDto.Email))
            {
                return Result<string>.Failure(AccountError.SameEmail);
            }
            
            if (await _unitOfWork.UserManager.Users.AnyAsync(x => x.UserName == registerUserDto.UserName))
            {
                return Result<string>.Failure(AccountError.SameUserName);
            }
            
            if (await _unitOfWork.UserManager.Users.AnyAsync(x => x.PhoneNumber == registerUserDto.PhoneNumber))
            {
                return Result<string>.Failure(AccountError.SamePhone);
            }

            var user = _mapper.Map<UserEntity>(registerUserDto);
            var result = await _unitOfWork.UserManager.CreateAsync(user, registerUserDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => new Error(error.Code, error.Description));
                return Result<string>.Failure(errors);
            }

            var roleResult = await _unitOfWork.UserManager.AddToRoleAsync(user, "Purchaser");
            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors.Select(error => new Error(error.Code, error.Description));
                return Result<string>.Failure(errors);
            }

            var createdUser =
                await _unitOfWork.UserManager.Users.FirstOrDefaultAsync(x => x.UserName == registerUserDto.UserName);
            if (createdUser == null)
            {
                return Result<string>.Failure(AccountError.NotFoundById);
            }

            createdUser.Profile = new ProfileEntity
            {
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                UserId = createdUser.Id
            };
            if (!await _unitOfWork.Commit())
            {
                return Result<string>.Failure(DbError.FailSaveChanges);
            }
            
            return Result<string>.Success($"{createdUser.UserName} has been registered");
        }

        public async Task<Result<UserDto>> LoginAsync(LoginUserDto loginUserDto)
        {
            var user = await _unitOfWork.UserManager.Users
                .AsNoTracking()
                .Include(x=>x.Profile)
                .FirstOrDefaultAsync(x => x.UserName == loginUserDto.UserName);
            if (user == null)
            {
                return Result<UserDto>.Failure(AccountError.NotFoundByUserName);
            }

            var result = await _unitOfWork.SignInManager.CheckPasswordSignInAsync(user, loginUserDto.Password, false);
            if (!result.Succeeded)
            {
                return Result<UserDto>.Failure(AccountError.InvalidPasswordOrUserName);
            }

            return Result<UserDto>.Success(new UserDto
            {
                UserName = loginUserDto.UserName,
                Token = await _tokenService.CreateTokenAsync(user),
                Id = user.Id,
                Profile = _mapper.Map<ProfileDto>(user.Profile)
            });
        }
        
        public async Task<Result<string>> DeleteAsync(string userId, string password)
        {
            var user = await _unitOfWork.UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<string>.Failure(AccountError.NotFoundById);
            }

            var checkPasswordResult = await _unitOfWork.SignInManager.CheckPasswordSignInAsync(user, password, false);
            if (!checkPasswordResult.Succeeded)
            {
                return Result<string>.Failure(AccountError.InvalidPasswordOrUserName);
            }

            var result = await _unitOfWork.UserManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return Result<string>.Failure(result.Errors.Select(x=> new Error(x.Code,x.Description)));
            }

            return Result<string>.Success($"user \"{user.UserName}\" was deleted");
        }

        public async Task<Result<UpdateUserDto>> UpdateAsync(string userId, UpdateUserDto updateUserDto)
        {
            var user = await _unitOfWork.UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<UpdateUserDto>.Failure(AccountError.NotFoundById);
            }

            _mapper.Map(updateUserDto, user);
            var resultUpdate = await _unitOfWork.UserManager.UpdateAsync(user);
            if (resultUpdate.Succeeded)
            {
                return Result<UpdateUserDto>.Success(updateUserDto);
            }

            return Result<UpdateUserDto>.Failure(resultUpdate.Errors.Select(x => new Error
                (x.Code, x.Code)));
        }

        public async Task<Result<InfoUserDto>> GetInfoAsync(string userId)
        {
            var user = await _unitOfWork.UserManager.Users
                .AsNoTracking()
                .Include(x => x.Profile)
                .FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return Result<InfoUserDto>.Failure(AccountError.NotFoundById);
            }

            var userInfoDto = _mapper.Map<InfoUserDto>(user);
            userInfoDto.Profile = _mapper.Map<ProfileDto>(user.Profile);
            return Result<InfoUserDto>.Success(userInfoDto);
        }
        
        public async Task<Result<string>> GiveSellerRole(string userId)
        {
            var user = await _unitOfWork.UserManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return Result<string>.Failure(AccountError.NotFoundById);
            }

            var resultAddRole = await _unitOfWork.UserManager.AddToRoleAsync(user, "Seller");
            if (!resultAddRole.Succeeded)
            {
                return Result<string>.Failure(resultAddRole.Errors.Select(x => new Error(x.Code,x.Description)));
            }

            if (await _unitOfWork.Commit())
            {
                return Result<string>.Success($"A role has been added to the user {user.UserName}");
            }

            return Result<string>.Failure(DbError.FailSaveChanges);
        }
    }
}