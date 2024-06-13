using CoreLibrary.Authorization;
using CoreLibrary.Authorization.SecretKeySettings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationWAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private TokenService tokenService = new TokenService(BaseAuthSettings.SecretKey);

        [HttpPost("GetToken")]
        public IActionResult GetToken([FromBody] string licenseKey)
        {
            var token = tokenService.GenerateToken(licenseKey);
            return Ok(new { Token = token });
        }
    }
}
