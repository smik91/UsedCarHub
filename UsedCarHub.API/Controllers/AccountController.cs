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

        /// <summary>
        /// Account registration
        /// </summary>
        /// <param name="registerUserDto"></param>
        /// <returns>A newly created user with Id, username and JWT Token</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///      POST /Account/register
        ///      {
        ///        "UserName": "exampleUsername",
        ///        "Email": "exampleeamil10@gmail.com",
        ///        "Password": "PasswordExample1234",
        ///        "FirstName": "Example",
        ///        "LastName": "Example",
        ///        "PhoneNumber": "+123456789"
        ///      }
        /// </remarks>
        /// <response code="200">Returns a newly created user with Id, username and JWT Token</response>
        /// <response code="400">Error</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            var resultRegisterUser = await _accountService.RegisterAsync(registerUserDto);
            if (resultRegisterUser.IsSuccess)
            {
                return Ok(resultRegisterUser.Value);
            }
            return BadRequest(resultRegisterUser.ExecutionErrors.Select(x=>x.Description));
        }

        /// <summary>
        /// Log in to account
        /// </summary>
        /// <param name="loginUserDto"></param>
        /// <returns></returns>
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
            {
                return Ok(resultDeleteUser.Value);
            }
            
            return NotFound(resultDeleteUser.ExecutionErrors.Select(x=>x.Description));
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> Update(string userId, UpdateUserDto updateUserDto)
        {
            var resultUpdateUser = await _accountService.UpdateAsync(userId, updateUserDto);
            if (resultUpdateUser.IsSuccess)
            {
                return Ok(resultUpdateUser.Value);
            }
            
            return NotFound(resultUpdateUser.ExecutionErrors.Select(x => x.Description));
        }

        [HttpPost("sellerRole")]
        [Authorize(Policy = "RequirePurchaserRole")]
        public async Task<IActionResult> GiveSellerRole(string userId)
        {
            var resultGiveRole = await _accountService.GiveSellerRole(userId);
            if (resultGiveRole.IsSuccess)
            {
                return Ok(resultGiveRole.Value);
            }
            
            return BadRequest(resultGiveRole.ExecutionErrors);
        }
    }
}