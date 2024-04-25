using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Register(string userName, string email, string password)
        {
            var result = await _accountService.RegisterAsync(userName, email, password);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.ExecutionError.Description);
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login(string userName,string password)
        {
            var resultLogin = await _accountService.LoginAsync(userName, password);
            if (resultLogin.IsSuccess)
            {
                string token = resultLogin.Value;
                
                Response.Cookies.Append("usedCarHubId", token, new CookieOptions
                {
                    HttpOnly = true, 
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    MaxAge = TimeSpan.FromHours(12)
                });
        
                return Ok("Login successful");
            }

            return BadRequest(resultLogin.ExecutionError.Description);
        }
    }
}