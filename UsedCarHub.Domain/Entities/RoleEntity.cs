using Microsoft.AspNetCore.Identity;

namespace UsedCarHub.Domain.Entities
{
    public class RoleEntity : IdentityRole
    {
        public ICollection<UserRoleEntity> UserRoles { get; set; }
    }
}