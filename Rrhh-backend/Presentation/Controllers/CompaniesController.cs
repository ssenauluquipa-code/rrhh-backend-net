using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Requests.Company;
using Rrhh_backend.Presentation.DTOs.Responses.Company;

namespace Rrhh_backend.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyResponse>>> GetAllAsync()
        {
            return Ok(await _companyService.GetAllAsync());
        }
        [HttpGet("{id}", Name = "GetCompanyById")]
        public async Task<ActionResult<CompanyResponse>> GetById(int id)
        {
            var company = await _companyService.GetByIdAsync(id);
            if(company == null) return NotFound();

            return Ok(company);
        }
        [HttpPost]
        public async Task<ActionResult<CompanyResponse>> CreateAsync([FromBody] CompanyRequest request)
        {
            var result = await _companyService.CreateAsync(request);

            return CreatedAtRoute("GetCompanyById", new { id = result.CompanyId }, result);
        }

        [HttpPut("{id}")] 
        public async Task<ActionResult<CompanyResponse>> UpdateAsync(int id, [FromBody] CompanyRequest request)
        {
            var result = await _companyService.UpdateAsync(id, request);
            if(result == null) return NotFound();
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var result = await _companyService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
        [HttpPut("{id}/activate")]
        public async Task<ActionResult> ActivateAsync(int id)
        {
            var result = await _companyService.ChangeStatusAsync(id);
            if (!result) return NotFound();
            return result ? Ok(new { message = "Estado actualizado correctamente" }) : NotFound();
        }
    }
}
