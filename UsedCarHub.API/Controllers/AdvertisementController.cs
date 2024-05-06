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
        [Authorize]
        public async Task<IActionResult> Create(AddAdvertisementDto addAdvertisementDto)
        {
            var result = await _advertisementService.AddAsync(addAdvertisementDto);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.ExecutionErrors.Select(x=>x.Description));
        }
    }
}