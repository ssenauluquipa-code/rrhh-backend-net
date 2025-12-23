using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Infrastructure.Services;
using Rrhh_backend.Presentation.DTOs.Requests.Permission;
using Rrhh_backend.Presentation.DTOs.Responses.Permissions;

namespace Rrhh_backend.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsAssignmentController : ControllerBase
    {
        private readonly IPermissionAssignmentService _assignmentService;

        public PermissionsAssignmentController(IPermissionAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [HttpGet("role/{roleId}")]
        public async Task<ActionResult<PermissionAssignmentResponse>> GetAssignmentByRole(int roleId)
        {
            try
            {
                var assignment = await _assignmentService.GetAssignmentByRoleAsync(roleId);
                return Ok(new { success = true, data = assignment });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignPermissions([FromBody] PermissionAssignmentRequest request)
        {
            try
            {
                await _assignmentService.AssignPermissionsAsync(request);
                return Ok(new { success = true, message = "Permisos asignados exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("permission-types")]
        public async Task<ActionResult<List<PermissionType>>> GetAllPermissionTypes()
        {
            try
            {
                var permissionTypes = await _assignmentService.GetAllPermissionTypesAsync();
                return Ok(new { success = true, data = permissionTypes });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

    }
}
