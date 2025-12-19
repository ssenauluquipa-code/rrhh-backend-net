using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rrhh_backend.Core.Entities;
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
        private readonly IPermissionService _permissionService;
        public AuthController(IAuthService authService, IPermissionService permissionService)
        {
            _authService = authService;
            _permissionService = permissionService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                var result = await _authService.LoginAsync(request);

                //if (result == null)
                //{
                //    return Unauthorized(new { message = "Credenciales inválidas." });
                //}
                return Ok(result);
            }
            catch(BusinessException ex)
            {
                Console.WriteLine($"Error en login: {ex.Message}");
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

        [HttpGet("permissions")]
        [Authorize]
        public async Task<IActionResult> GetUserPermissions()
        {
            try
            {
                // 🔑 OBTIENE ROLEID DEL TOKEN
                var roleIdClaim = User.FindFirst("RoleId");
                if (roleIdClaim == null || !int.TryParse(roleIdClaim.Value, out int roleId))
                    return BadRequest("Token no contiene RoleId válido");

                // 🔑 OBTIENE PERMISOS
                var permissions = await _permissionService.GetUserPermissionsAsync(roleId);
                return Ok(new { success = true, data = permissions });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR EN AuthController: {ex.Message}");
                // 🔑 DEVUELVE EL ERROR REAL EN LA RESPUESTA
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message,
                    stackTrace = ex.StackTrace // Solo para diagnóstico
                });
            }
        }

        [HttpPost("refresh")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.AccessToken))
                    return BadRequest("Token no proporcionado");

                if (_authService.IsTokenExpired(request.AccessToken))
                {
                    var newToken = _authService.RefreshToken(request.AccessToken);
                    return Ok(new { success = true, token = newToken });
                }
                else
                {
                    // Token aún válido, devolver el mismo
                    return Ok(new { success = true, token = request.AccessToken });
                }
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al renovar token" });
            }
        }
    }
}