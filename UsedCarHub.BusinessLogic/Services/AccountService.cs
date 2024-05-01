using AutoMapper;
using Microsoft.AspNetCore.Identity;
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

        public async Task<Result<RegisterUserDto>> RegisterAsync(RegisterUserDto registerUserDto)
        {
            if (await _unitOfWork.UserManager.Users.AnyAsync(x => x.UserName == registerUserDto.UserName))
                return Result<RegisterUserDto>.Failure(AccountError.SameUserName);
            if (await _unitOfWork.UserManager.Users.AnyAsync(x => x.Email == registerUserDto.Email))
                return Result<RegisterUserDto>.Failure(AccountError.SameEmail);
            if (await _unitOfWork.UserManager.Users.AnyAsync(x => x.))
            var user = _mapper.Map<UserEntity>(registerUserDto);
            var result = await _unitOfWork.UserManager.CreateAsync(user, registerUserDto.Password);
            
        }

        public Task<Result<string>> LoginAsync(LoginUserDto loginUserDto)
        {
            throw new NotImplementedException();
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