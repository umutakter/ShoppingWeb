namespace AuthorizationWAPI.Models
{
    public class TokenRequest
    {
        public string LicenseKey { get; set; }
        public string[] Permissions { get; set; }
    }
}
