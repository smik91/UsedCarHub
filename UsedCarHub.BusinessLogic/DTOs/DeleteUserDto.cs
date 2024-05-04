using System.ComponentModel.DataAnnotations;

namespace UsedCarHub.BusinessLogic.DTOs
{
    public class DeleteUserDto
    {
        [Required] 
        public string UserName { get; set; }
    }
}