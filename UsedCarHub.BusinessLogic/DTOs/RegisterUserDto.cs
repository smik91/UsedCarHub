using System.ComponentModel.DataAnnotations;

namespace UsedCarHub.BusinessLogic.DTOs
{
    public class RegisterUserDto
    {
        [Required]
        public string UserName { get; set; } 
        [Required]
        public string Email { get; set; } 
        [Required]
        public string Password { get; set; }
        [Required] 
        public string FirstName { get; set; }
        [Required] 
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}