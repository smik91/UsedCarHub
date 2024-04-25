namespace UsedCarHub.Common.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateJwtToken(string userName, string email);
    }
}