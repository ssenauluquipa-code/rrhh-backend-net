using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Requests.Departament;

namespace Rrhh_backend.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _service;
        public DepartmentsController(IDepartmentService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetAll();
            return Ok(result);
        }

        [HttpGet("list/{companyId}")]
        public async Task<IActionResult> GetAll(int companyId) => Ok(await _service.GetListAsync(companyId));

        [HttpPost]
        public async Task<IActionResult> Create(DepartamentRequest request) => Ok(await _service.CreateAsync(request));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DepartamentRequest request) => Ok(await _service.UpdateAsync(id, request));

        [HttpDelete("{id}/{companyId}")]
        public async Task<IActionResult> Delete(int id, int companyId) => Ok(await _service.DeleteAsync(id, companyId));
    }
}
