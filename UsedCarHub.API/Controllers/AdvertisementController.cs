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
        
        [HttpPost("create")]
        [Authorize(Policy = "RequireSellerRole")]
        public async Task<IActionResult> CreateAsync(AddAdvertisementDto addAdvertisementDto)
        {
            var result = await _advertisementService.AddAsync(addAdvertisementDto);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.ExecutionErrors.Select(x=>x.Description));
        }
        
        [HttpDelete("delete")]
        [Authorize(Policy = "RequireSellerRole")]
        public async Task<IActionResult> DeleteAsync(int advertisementId)
        {
            var result = await _advertisementService.DeleteAsync(advertisementId);
            if (result.IsSuccess)
                return Ok(result.Value);
            return NotFound(result.ExecutionErrors);
        }

        [HttpPut("update")]
        [Authorize(Policy = "RequireSellerRole")]
        public async Task<IActionResult> UpdateAsync(int advertisementId, UpdateAdvertisementDto updateAdvertisementDto)
        {
            return Ok();
        }
    }
}