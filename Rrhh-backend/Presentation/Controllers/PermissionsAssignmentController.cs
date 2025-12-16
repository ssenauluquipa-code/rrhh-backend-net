using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Requests;

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
        [HttpGet("{roleId}")]
        public async Task<IActionResult> LoadAssignment(int roleId)
        {
            try
            {
                var response = await _assignmentService.LoadAssignmentAsync(roleId);
                return Ok(new { success = true, data = response });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al cargar permisos" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> SaveAssignment([FromBody] PermissionAssignmentRequest request)
        {
            if (request.RoleId <= 0 || request.Modules == null)
                return BadRequest(new { success = false, message = "Datos inválidos" });

            try
            {
                await _assignmentService.SaveAssignmentAsync(request);
                return Ok(new { success = true, message = "Permisos guardados correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al guardar permisos" });
            }
        }
    }
}
