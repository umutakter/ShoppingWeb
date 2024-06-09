namespace CoreLibrary.Authorization.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string licenseKey);
    }
}
