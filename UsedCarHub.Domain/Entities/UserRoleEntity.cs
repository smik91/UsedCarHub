using Microsoft.AspNetCore.Identity;

namespace UsedCarHub.Domain.Entities
{
    public class UserRoleEntity : IdentityUserRole<string>
    {
        public UserEntity User { get; set; }
        public RoleEntity Role { get; set; }
    }
}