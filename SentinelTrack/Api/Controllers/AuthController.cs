using Microsoft.AspNetCore.Mvc;
using SentinelTrack.Application.DTOs.Request;
using SentinelTrack.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace SentinelTrack.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    [SwaggerTag("Controlador responsável por cuidar da autorização.")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Gera um token de autenticação
        /// </summary>
        /// <param name="request">Usuário e Senha</param>
        /// <returns>Token gerado</returns>
        [HttpPost("token")]
        public IActionResult Token([FromBody] AuthRequest request)
        {
            var result = _authService.Authenticate(request);
            if (result == null) return Unauthorized(new { message = "Invalid username or password." });
            return Ok(result);
        }
    }
}