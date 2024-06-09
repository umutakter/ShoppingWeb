using AuthorizationWAPI.Models;
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
        public IActionResult GetToken([FromBody] TokenRequest request)
        {
            var token = _tokenService.GenerateToken(request.LicenseKey);
            return Ok(new { Token = token });
        }
    }
}
