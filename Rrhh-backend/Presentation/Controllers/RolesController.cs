using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Requests.Roles;
using Rrhh_backend.Presentation.DTOs.Responses.Roles;

namespace Rrhh_backend.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        [HttpGet]
        public async Task<ActionResult<List<RolesResponse>>> GetRolesAll()
        {
            var roles = await _rolesService.GetRolesAllAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RolesResponse>> GetRolesById(Guid id)
        {
            var roles = await _rolesService.GetRolesByIdAsync(id);
            if (roles == null) return NotFound();
            return Ok(roles);
        }

        [HttpPost]
        public async Task<ActionResult<RolesResponse>> CreateRoles([FromBody] CreateRolesRequest request)
        {
            var roles = await _rolesService.CreatedRolesAsync(request);
            return CreatedAtAction(nameof(GetRolesById), new { id = roles.Id }, roles);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RolesResponse>> UpdateRoles(Guid id, [FromBody] UpdateRolesRequest request)
        {
            var roles = await _rolesService.UpdateRolesAsync(id, request);
            if (roles == null) return NotFound();
            return Ok(roles);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteRoles(Guid id)
        {
            var result = await _rolesService.DeletedAsync(id);
            if (!result) return NotFound(new { message = "se cambio d eestado de forma correcta" });
            return NoContent();
        }

        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateRoles(Guid id)
        {
            var result = await _rolesService.ActivatedAsync(id);
            if (!result) return NotFound(new { message = "no se cmabio ocurrio un error" });
            return Ok(result);
        }
    }
}
