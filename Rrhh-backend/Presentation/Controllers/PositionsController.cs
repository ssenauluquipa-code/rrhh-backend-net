using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Requests.Position;

namespace Rrhh_backend.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly IPositionService _service;
        public PositionsController(IPositionService service)
        {
            _service = service;
        }

        [HttpGet] // LISTAR TODO (SIN PARÁMETROS)
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("list/{companyId}")] // LISTAR POR EMPRESA
        public async Task<IActionResult> GetByCompany(int companyId) => Ok(await _service.GetByCompanyAsync(companyId));

        [HttpPost] // CREAR
        public async Task<IActionResult> Create(PositionRequest request) => Ok(await _service.CreateAsync(request));

        [HttpPut("{id}")] // EDITAR (ACTUALIZAR)
        public async Task<IActionResult> Update(int id, PositionRequest request) => Ok(await _service.UpdateAsync(id, request));

        [HttpDelete("{id}/{companyId}")] // DESACTIVAR (BORRADO LÓGICO)
        public async Task<IActionResult> Deactivate(int id, int companyId) =>
            Ok(await _service.ChangeStatusAsync(id, companyId, false));

        [HttpPut("activate/{id}/{companyId}")] // ACTIVAR
        public async Task<IActionResult> Activate(int id, int companyId) =>
            Ok(await _service.ChangeStatusAsync(id, companyId, true));
    }
}
