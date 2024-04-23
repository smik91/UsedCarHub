using AutoMapper;
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
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        
        public async Task<Result<UserDto>> GetByUserName(string userName)
        {
            var findByUsernameResult = await _userRepository.GetByUserNameAsync(userName);
            if (findByUsernameResult.IsSuccess)
            {
                var userDto = _mapper.Map<UserDto>(findByUsernameResult.Value);
                return Result<UserDto>.Success(userDto);
            }

            return Result<UserDto>.Failure(AccountError.NotFountByUserName);
        }
    }
}