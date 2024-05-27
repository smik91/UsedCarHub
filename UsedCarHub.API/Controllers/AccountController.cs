using System.Security.Claims;
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
        /// <remarks>
        /// Sample request:
        /// 
        ///      POST /Account/register
        ///      {
        ///        "userName": "exampleUsername",
        ///        "email": "exampleemail10@gmail.com",
        ///        "password": "PasswordExample1234",
        ///        "firstName": "Example",
        ///        "lastName": "Example",
        ///        "phoneNumber": "+123456789"
        ///      }
        /// </remarks>
        /// <response code="200">Returns confirmation of successful registration</response>
        /// <response code="400">If user with this username/email/phone number already exists.</response>
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
        /// <remarks>
        /// Sample request:
        /// 
        ///      POST /Account/login
        ///      {
        ///        "userName": "exampleUsername",
        ///        "password": "PasswordExample1234"
        ///      }
        /// </remarks>
        /// <response code="200">Returns ID, username, JWT Token and profile.</response>
        /// <response code="400">If the username or password is incorrect.</response>
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
        
        /// <summary>
        /// Get account info
        /// </summary>
        /// <param name="userId">The ID of user to get info</param>
        /// <response code="200">Returns info about user.</response>
        /// <response code="401">If user with this ID doesn't exists</response>
        [HttpGet("get")]
        public async Task<IActionResult> GetInfo(string userId)
        {
            var getInfoResult = await _accountService.GetInfoAsync(userId);
            if (getInfoResult.IsSuccess)
            {
                return Ok(getInfoResult.Value);
            }

            return BadRequest(getInfoResult.ExecutionErrors.Select(x => x.Description));
        }
        
        /// <summary>
        /// Delete account
        /// </summary>
        /// <param name="password">The password of the user to delete</param>
        /// <response code="200">Returns confirmation of successful deletion.</response>
        /// <response code="401">If user is not authorized</response>
        /// <response code="404">If password is not correct.</response>
        [HttpDelete("delete")]
        [Authorize]
        public async Task<IActionResult> Delete(string password)
        {
            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Unauthorized();
            }
            
            var resultDeleteUser = await _accountService.DeleteAsync(currentUserId, password);
            if (resultDeleteUser.IsSuccess)
            {
                return Ok(resultDeleteUser.Value);
            }
            
            return NotFound(resultDeleteUser.ExecutionErrors.Select(x=>x.Description));
        }

        /// <summary>
        /// Update account details
        /// </summary>
        /// <param name="updateUserDto">The new user details</param>
        /// <returns>Updated user details</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///      PUT /Account/update
        ///      {
        ///        "userName": "exampleUsername",
        ///        "email": "exampleeamil10@gmail.com",
        ///        "password": "PasswordExample1234",
        ///        "phoneNumber": "+123456789"
        ///      }
        /// </remarks>
        /// <response code="200">Returns the updated user details.</response>
        /// <response code="401">If user is not authorized</response>
        /// <response code="404">If data is not correct.</response>
        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> Update(UpdateUserDto updateUserDto)
        {
            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Unauthorized();
            }
           
            var resultUpdateUser = await _accountService.UpdateAsync(currentUserId, updateUserDto);
            if (resultUpdateUser.IsSuccess)
            {
                return Ok(resultUpdateUser.Value);
            }
            
            return NotFound(resultUpdateUser.ExecutionErrors.Select(x => x.Description));
        }

        /// <summary>
        /// Grant seller role to user
        /// </summary>
        /// <returns>Confirmation of role assignment</returns>
        /// <response code="200">Returns confirmation of successful role assignment.</response>
        /// <response code="401">If user is not authorized</response>
        /// <response code="400">If there is an error in granting the role or user is not authorized.</response>
        [HttpPost("sellerRole")]
        [Authorize(Policy = "RequirePurchaserRole")]
        public async Task<IActionResult> GiveSellerRole()
        {
            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var resultGiveRole = await _accountService.GiveSellerRole(currentUserId);
            if (resultGiveRole.IsSuccess)
            {
                return Ok(resultGiveRole.Value);
            }
            
            return BadRequest(resultGiveRole.ExecutionErrors);
        }
        
        /// <summary>
        /// Get all user's advertisements
        /// </summary>
        /// <returns>All user's advertisements</returns>
        /// <response code="200">Returns all user's advertisements.</response>
        /// <response code="401">If user is not authorized</response>
        /// <response code="400">If user is not authorized.</response>
        [HttpGet("advertisements")]
        [Authorize(Policy = "RequireSellerRole")]
        public async Task<IActionResult> GetAdvertisements()
        {
            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            var resultAdvertisements = await _accountService.GetAdvertisements(currentUserId);
            if (resultAdvertisements.IsSuccess)
            {
                return Ok(resultAdvertisements.Value);
            }

            return BadRequest();
        }
    }
}