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
            var result = await _accountService.RegisterAsync(registerUserDto);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.ExecutionErrors.Select(x=>x.Description));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            var resultLogin = await _accountService.LoginAsync(loginUserDto);
            if (resultLogin.IsSuccess)
            {
                string token = resultLogin.Value.Token;
                
                Response.Cookies.Append("usedCarHubId", token, new CookieOptions
                {
                    HttpOnly = true, 
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    MaxAge = TimeSpan.FromHours(12)
                });
        
                return Ok(resultLogin.Value);
            }

            return BadRequest(resultLogin.ExecutionErrors.Select(x=>x.Description));
        }
        
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string Id)
        {
            var resultDelete = await _accountService.DeleteAsync(Id);
            if (resultDelete.IsSuccess)
            {
                return Ok(resultDelete.Value);
            }

            return NotFound(resultDelete.ExecutionErrors.Select(x=>x.Description));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(string Id, UpdateUserDto updateUserDto)
        {
            var resultUpdate = await _accountService.UpdateAsync(Id, updateUserDto);
            if (resultUpdate.IsSuccess)
                return Ok(resultUpdate.Value);
            return NotFound(resultUpdate.ExecutionErrors.Select(x => x.Description));
        }
    }
}