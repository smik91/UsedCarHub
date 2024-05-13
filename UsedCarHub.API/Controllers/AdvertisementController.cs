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
            var resultAdd = await _advertisementService.AddAsync(addAdvertisementDto);
            if (resultAdd.IsSuccess)
            {
                return Ok(resultAdd.Value);
            }
            
            return BadRequest(resultAdd.ExecutionErrors.Select(x=>x.Description));
        }
        
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

        [HttpPut("update")]
        [Authorize(Policy = "RequireSellerRole")]
        public async Task<IActionResult> UpdateAsync(int advertisementId, UpdateAdvertisementDto updateAdvertisementDto)
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