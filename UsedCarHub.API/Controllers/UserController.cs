using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsedCarHub.BusinessLogic.Interfaces;

namespace UsedCarHub.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        
        [Authorize]
        [HttpGet("{userName}")]
        public async Task<IActionResult> GetByUsername(string userName)
        {
            var searchResult = await _userService.GetByUserName(userName);
            if (searchResult.IsSuccess)
            {
                return Ok(searchResult.Value);
            }

            return BadRequest(searchResult.ExecutionError.Description);
        }
    }
}