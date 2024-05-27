using System.ComponentModel.DataAnnotations;

namespace UsedCarHub.BusinessLogic.DTOs
{
    public class UpdateAdvertisementDto
    {
        [Required]
        public int Price { get; set; }
        [Required]
        public string Description { get; set; }
    }
}