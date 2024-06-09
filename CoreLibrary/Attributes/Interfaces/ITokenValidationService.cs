namespace CoreLibrary.Attributes.Interfaces
{
    public interface ITokenValidationService
    {
        bool ValidateToken(string token, out string licenseKey, out string[] permissions);
    }
}