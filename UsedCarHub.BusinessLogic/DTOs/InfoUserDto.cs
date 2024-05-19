namespace UsedCarHub.BusinessLogic.DTOs
{
    public class InfoUserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ProfileDto Profile { get; set; }
    }
}