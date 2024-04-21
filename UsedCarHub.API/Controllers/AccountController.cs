using Microsoft.AspNetCore.Mvc;
using UsedCarHub.BusinessLogic.Interfaces;

namespace UsedCarHub.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string userName, string email, string password)
        {
            var result = await _userService.RegisterAsync(userName, email, password);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.ExecutionError.Description);
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login(string userName,string password)
        {
            var resultLogin = await _userService.LoginAsync(userName, password);
            if (resultLogin.IsSuccess)
            {
                string token = resultLogin.Value;
                return Ok(token);
            }

            return BadRequest(resultLogin.ExecutionError.Description);
        }
    }
}