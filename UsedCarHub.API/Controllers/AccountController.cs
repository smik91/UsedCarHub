using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsedCarHub.BusinessLogic.DTOs;
using UsedCarHub.BusinessLogic.Interfaces;

namespace UsedCarHub.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            var resultRegisterUser = await _accountService.RegisterAsync(registerUserDto);
            if (resultRegisterUser.IsSuccess)
                return Ok(resultRegisterUser.Value);
            return BadRequest(resultRegisterUser.ExecutionErrors.Select(x=>x.Description));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            var resultLoginUser = await _accountService.LoginAsync(loginUserDto);
            if (resultLoginUser.IsSuccess)
            {
                string token = resultLoginUser.Value.Token;
                
                Response.Cookies.Append("usedCarHubId", token, new CookieOptions
                {
                    HttpOnly = true, 
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    MaxAge = TimeSpan.FromHours(12)
                });
        
                return Ok(resultLoginUser.Value);
            }

            return BadRequest(resultLoginUser.ExecutionErrors.Select(x=>x.Description));
        }
        
        [HttpDelete("delete")]
        [Authorize]
        public async Task<IActionResult> Delete(string userId)
        {
            var resultDeleteUser = await _accountService.DeleteAsync(userId);
            if (resultDeleteUser.IsSuccess)
                return Ok(resultDeleteUser.Value);
            return NotFound(resultDeleteUser.ExecutionErrors.Select(x=>x.Description));
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> Update(string userId, UpdateUserDto updateUserDto)
        {
            var resultUpdateUser = await _accountService.UpdateAsync(userId, updateUserDto);
            if (resultUpdateUser.IsSuccess)
                return Ok(resultUpdateUser.Value);
            return NotFound(resultUpdateUser.ExecutionErrors.Select(x => x.Description));
        }

        [HttpPost("sellerRole")]
        [Authorize]
        public async Task<IActionResult> GiveSellerRole(string userId)
        {
            return Ok();
        }
    }
}