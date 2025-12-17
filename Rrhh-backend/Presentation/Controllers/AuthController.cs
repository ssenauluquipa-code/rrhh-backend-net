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
                // Asumiendo que obtienes el roleId del usuario actual
                var roleId = GetRoleIdFromClaims(); // Implementa esta lógica
                var permissions = await _permissionService.GetUserPermissionsAsync(roleId);
                return Ok(new { success = true, data = permissions });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al cargar permisos" });
            }
        }
        private int GetRoleIdFromClaims()
        {
            var roleIdClaim = User.FindFirst("RoleId");
            if(roleIdClaim == null || !int.TryParse(roleIdClaim.Value, out int roleId))
            {
                throw new NotImplementedException("Implementar obtención de roleId");
            }
            return roleId;
        }
    }
}