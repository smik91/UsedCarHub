using CarAPI.Models;
using CarAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace UsedCarHub.Controllers
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

        [HttpGet]
        [Route("get")]
        public async Task<IEnumerable<User>> Get()
        {
            var allUsers = await _userService.GetAsync();
            return allUsers;
        }

        [HttpPost]
        [Route("post")]
        public async Task Post([FromBody] User user)
        {
            await _userService.Create(user);
        }
    }
}
