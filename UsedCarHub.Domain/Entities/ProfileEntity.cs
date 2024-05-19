namespace UsedCarHub.Domain.Entities
{
    public sealed class ProfileEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarUrl { get; set; }
        public UserEntity User { get; set; }
        public string UserId { get; set; }
    }
}