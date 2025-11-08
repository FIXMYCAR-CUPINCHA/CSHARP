using Microsoft.AspNetCore.Mvc;
using SentinelTrack.Application.DTOs.Request;
using SentinelTrack.Application.Interfaces;

namespace SentinelTrack.Api.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("token")]
        public IActionResult Token([FromBody] AuthRequest request)
        {
            var result = _authService.Authenticate(request);
            if (result == null) return Unauthorized(new { message = "Invalid username or password." });
            return Ok(result);
        }
    }
}