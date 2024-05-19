using System.ComponentModel.DataAnnotations;

namespace UsedCarHub.BusinessLogic.DTOs
{
    public class UpdateUserDto
    {
        [Required] 
        public string UserName { get; set; }
        [Required] 
        public string Email { get; set; }
        [Required] 
        public string Password { get; set; }
        [Required] 
        public string PhoneNumber { get; set; }
    }
}