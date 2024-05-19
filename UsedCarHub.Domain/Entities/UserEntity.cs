using Microsoft.AspNetCore.Identity;

namespace UsedCarHub.Domain.Entities
{
    public sealed class UserEntity : IdentityUser
    {
        public string Email { get; set; }
        public ICollection<UserRoleEntity> UserRoles { get; set; }
        public ICollection<AdvertisementEntity> Advertisements { get; set; }
        public ProfileEntity Profile { get; set; }
    }
}