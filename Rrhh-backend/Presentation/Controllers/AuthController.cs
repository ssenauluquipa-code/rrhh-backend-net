using Microsoft.AspNetCore.Mvc;
using Rrhh_backend.Core.Exceptions;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Requests.Auth;
using Rrhh_backend.Presentation.DTOs.Responses.Auth;

namespace Rrhh_backend.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                var result = await _authService.LoginAsync(request);

                if (result == null)
                {
                    return Unauthorized(new { message = "Credenciales inválidas." });
                }
                return Ok(result);
            }
            catch(BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }

        [HttpPost("logout")]
        public async Task<IActionResult> logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var success = await _authService.LogoutAsync(token);
            if (success)
            {
                return Ok(new { message = "Session cerrada correctamente" });
            }
            else
            {
                return BadRequest(new { message = " No se pudo cerrar session" });
            }
        }        
    }
}