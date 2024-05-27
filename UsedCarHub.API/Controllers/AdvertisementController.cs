using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsedCarHub.BusinessLogic.DTOs;
using UsedCarHub.BusinessLogic.Interfaces;
using UsedCarHub.Common.Errors;

namespace UsedCarHub.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementService _advertisementService;

        public AdvertisementController(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }
        
        /// <summary>
        /// Create a new advertisement
        /// </summary>
        /// <param name="addAdvertisementDto">Details of the advertisement to create</param>
        /// <returns>The created advertisement</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Advertisement/create
        ///     {
        ///        "Price": 10000,
        ///        "Description": "A well-maintained car",
        ///        "Car":
        ///          {
        ///             "registrationNumber": "string",
        ///             "vin": "string",
        ///             "mark": 1,
        ///             "model": "string",
        ///             "yearOfProduction": 0,
        ///             "transmissionType": 1,
        ///             "engineCapacity": 0,
        ///             "mileage": 0
        ///          }
        ///     }
        /// </remarks>
        /// <response code="200">Returns the created advertisement</response>
        /// <response code="400">If the input is invalid</response>
        [HttpPost("create")]
        [Authorize(Policy = "RequireSellerRole")]
        public async Task<IActionResult> CreateAsync(AddAdvertisementDto addAdvertisementDto)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var resultAdd = await _advertisementService.AddAsync(addAdvertisementDto, currentUserId);
            if (resultAdd.IsSuccess)
            {
                return Ok(resultAdd.Value);
            }
            
            return BadRequest(resultAdd.ExecutionErrors.Select(x=>x.Description));
        }
        
        /// <summary>
        /// Get advertisement information by ID
        /// </summary>
        /// <param name="advertisementId">ID of the advertisement</param>
        /// <returns>Advertisement information</returns>
        /// <response code="200">Returns the advertisement information</response>
        /// <response code="400">If the advertisement is not found</response>
        [HttpGet("get")]
        public async Task<IActionResult> GetInfo(int advertisementId)
        {
            var resultGetInfo = await _advertisementService.GetInfoAsync(advertisementId);
            if (resultGetInfo.IsSuccess)
            {
                return Ok(resultGetInfo.Value);
            }
            
            return BadRequest(resultGetInfo.ExecutionErrors);
        }
        
        /// <summary>
        /// Get information for all advertisements
        /// </summary>
        /// <returns>List of all advertisement information</returns>
        /// <response code="200">Returns a list of advertisement information</response>
        /// <response code="400">If there is an error retrieving the advertisements information</response>
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllInfo()
        {
            var resultGetAllInfo = await _advertisementService.GetAllInfoAsync();
            if (resultGetAllInfo.IsSuccess)
            {
                return Ok(resultGetAllInfo.Value);
            }

            return BadRequest(resultGetAllInfo.ExecutionErrors);
        }

        /// <summary>
        /// Update an advertisement
        /// </summary>
        /// <param name="advertisementId">ID of the advertisement to update</param>
        /// <param name="updateAdvertisementDto">Updated details of the advertisement</param>
        /// <returns>The updated advertisement</returns>
        /// <response code="200">Returns the updated advertisement</response>
        /// <response code="401">If user is not authorized</response>
        /// <response code="404">If the advertisement is not found</response>
        [HttpPut("update")]
        [Authorize(Policy = "RequireSellerRole")]
        public async Task<IActionResult> UpdateAsync(int advertisementId, UpdateAdvertisementDto updateAdvertisementDto)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            var getCurrentAdvertisementResult = await _advertisementService.GetInfoAsync(advertisementId);
            if (!getCurrentAdvertisementResult.IsSuccess)
            {
                return BadRequest(getCurrentAdvertisementResult.ExecutionErrors.Select(x => x.Description));
            }

            if (currentUserId != getCurrentAdvertisementResult.Value.SellerId)
            {
                return BadRequest(AccountError.NoAccess.Description);
            }
            
            var resultUpdate = await _advertisementService.UpdateAsync(advertisementId, updateAdvertisementDto);
            if (resultUpdate.IsSuccess)
            {
                return Ok(resultUpdate.Value);
            }
            
            return NotFound(resultUpdate.ExecutionErrors);
        }
        
        /// <summary>
        /// Delete an advertisement
        /// </summary>
        /// <param name="advertisementId">ID of the advertisement to delete</param>
        /// <returns>Confirmation of deletion</returns>
        /// <response code="200">Returns confirmation of successful deletion</response>
        /// <response code="401">If user is not authorized</response>
        /// <response code="404">If the advertisement is not found</response>
        [HttpDelete("delete")]
        [Authorize(Policy = "RequireSellerRole")]
        public async Task<IActionResult> DeleteAsync(int advertisementId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            var getCurrentAdvertisementResult = await _advertisementService.GetInfoAsync(advertisementId);
            if (!getCurrentAdvertisementResult.IsSuccess)
            {
                return BadRequest(getCurrentAdvertisementResult.ExecutionErrors.Select(x => x.Description));
            }

            if (currentUserId != getCurrentAdvertisementResult.Value.SellerId)
            {
                return BadRequest(AccountError.NoAccess.Description);
            }
            
            var resultDelete = await _advertisementService.DeleteAsync(advertisementId);
            if (resultDelete.IsSuccess)
            {
                return Ok(resultDelete.Value);
            }
            
            return NotFound(resultDelete.ExecutionErrors);
        }
    }
}