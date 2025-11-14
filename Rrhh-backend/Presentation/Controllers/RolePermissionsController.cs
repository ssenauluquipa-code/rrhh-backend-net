using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Requests;
using Rrhh_backend.Presentation.DTOs.Responses;

namespace Rrhh_backend.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionsController : ControllerBase
    {
        private readonly IRolePermissionService _rolePermissionService;

        public RolePermissionsController(IRolePermissionService rolePermissionService)
        {
            _rolePermissionService = rolePermissionService;
        }
        // ✅ Asignar permiso a rol
        [HttpPost("assign")]
        public async Task<IActionResult> AssignPermissionToRole([FromBody] AssignPermissionRequest request)
        {
            var success = await _rolePermissionService.AssignPermissionToRoleAsync(request);

            if (!success)
            {
                return BadRequest(new { message = "Error al asignar permiso al rol." });
            }

            return Ok(new { message = "Permiso asignado correctamente." });
        }

        // ✅ Revocar permiso de rol
        [HttpDelete("revoke/{roleId}/{permissionId}")]
        public async Task<IActionResult> RevokePermissionFromRole(int roleId, int permissionId)
        {
            var success = await _rolePermissionService.RevokePermissionFromRoleAsync(roleId, permissionId);

            if (!success)
            {
                return NotFound(new { message = "Asignación de permiso no encontrada." });
            }

            return Ok(new { message = "Permiso revocado correctamente." });
        }

        // ✅ Listar permisos de un rol específico
        [HttpGet("by-role/{roleName}")]
        [AllowAnonymous] // Permite ver permisos sin ser admin (útil para frontend)
        public async Task<ActionResult<List<string>>> GetPermissionsByRoleName(string roleName)
        {
            var permissions = await _rolePermissionService.GetPermissionsByRoleNameAsync(roleName);

            if (permissions == null)
            {
                return NotFound(new { message = "Rol no encontrado." });
            }

            return Ok(permissions);
        }

        // ✅ Listar todas las asignaciones de permisos de un rol
        [HttpGet("assignments/{roleId}")]
        public async Task<ActionResult<List<RolePermissionResponse>>> GetRoleAssignments(int roleId)
        {
            var assignments = await _rolePermissionService.GetRoleAssignmentsAsync(roleId);

            if (assignments == null)
            {
                return NotFound(new { message = "Rol no encontrado." });
            }

            return Ok(assignments);
        }
    }
}
