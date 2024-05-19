using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsedCarHub.BusinessLogic.DTOs;
using UsedCarHub.BusinessLogic.Interfaces;

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
        ///        "SellerId": "string",
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
            var resultAdd = await _advertisementService.AddAsync(addAdvertisementDto);
            if (resultAdd.IsSuccess)
            {
                return Ok(resultAdd.Value);
            }
            
            return BadRequest(resultAdd.ExecutionErrors.Select(x=>x.Description));
        }
        
        /// <summary>
        /// Delete an advertisement
        /// </summary>
        /// <param name="advertisementId">ID of the advertisement to delete</param>
        /// <returns>Confirmation of deletion</returns>
        /// <response code="200">Returns confirmation of successful deletion</response>
        /// <response code="404">If the advertisement is not found</response>
        [HttpDelete("delete")]
        [Authorize(Policy = "RequireSellerRole")]
        public async Task<IActionResult> DeleteAsync(int advertisementId)
        {
            var resultDelete = await _advertisementService.DeleteAsync(advertisementId);
            if (resultDelete.IsSuccess)
            {
                return Ok(resultDelete.Value);
            }
            
            return NotFound(resultDelete.ExecutionErrors);
        }

        /// <summary>
        /// Update an advertisement
        /// </summary>
        /// <param name="advertisementId">ID of the advertisement to update</param>
        /// <param name="updateAdvertisementDto">Updated details of the advertisement</param>
        /// <returns>The updated advertisement</returns>
        /// <response code="200">Returns the updated advertisement</response>
        /// <response code="404">If the advertisement is not found</response>
        [HttpPut("update")]
        [Authorize(Policy = "RequireSellerRole")]
        public async Task<IActionResult> UpdateAsync(int advertisementId,[FromBody] UpdateAdvertisementDto updateAdvertisementDto)
        {
            var resultUpdate = await _advertisementService.UpdateAsync(advertisementId, updateAdvertisementDto);
            if (resultUpdate.IsSuccess)
            {
                return Ok(resultUpdate.Value);
            }
            
            return NotFound(resultUpdate.ExecutionErrors);
        }
    }
}