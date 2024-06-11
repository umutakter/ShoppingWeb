using CoreLibrary.Authorization.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationWAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthorizationController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("GetToken")]
        public IActionResult GetToken([FromBody] string licenseKey)
        {
            var token = _tokenService.GenerateToken(licenseKey);
            return Ok(new { Token = token });
        }
    }
}
