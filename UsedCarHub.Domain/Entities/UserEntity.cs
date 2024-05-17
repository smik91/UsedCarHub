using Microsoft.AspNetCore.Identity;

namespace UsedCarHub.Domain.Entities
{
    public sealed class UserEntity : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<UserRoleEntity> UserRoles { get; set; }
        public ICollection<AdvertisementEntity> Advertisements { get; set; }
    }
}