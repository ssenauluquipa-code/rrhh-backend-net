using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Requests.Tenants;
using Rrhh_backend.Presentation.DTOs.Responses.Tenants;

namespace Rrhh_backend.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantsController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }
        [HttpPost("create")]
        public async Task<ActionResult<TenantResponse>> CreateTenant([FromBody] CreateTenantRequest request)
        {
            try
            {
                var result = await _tenantService.CreateTenantAsync(request);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
