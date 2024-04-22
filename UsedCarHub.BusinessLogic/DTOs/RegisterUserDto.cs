namespace UsedCarHub.BusinessLogic.DTOs
{
    public class RegisterUserDto(string userName, string email, string passwordHash)
    {
        public string UserName { get; set; } = userName;
        public string Email { get; set; } = email;
        public string PasswordHash { get; set; } = passwordHash;
    }
}