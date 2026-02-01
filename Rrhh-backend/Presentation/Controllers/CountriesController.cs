using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Requests.Countries;
using Rrhh_backend.Presentation.DTOs.Responses.Countries;

namespace Rrhh_backend.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _service;
        public CountriesController(ICountryService service) {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryResponse>>> GetAllAsync()
        {
            var countries = await _service.GetAllCountriesAsync();
            return Ok(countries);
        }

        [HttpGet("{id}", Name = "GetCountryById")]
        public async Task<ActionResult<CountryResponse>> GetById(int id)
        {
            var countries = await _service.GetCountryByIdAsync(id);
            if (countries == null) return NotFound();
            return Ok(countries);
        }

        [HttpPost]
        public async Task<ActionResult<CountryResponse>> CreateAsync([FromBody]CountryRequest request)
        {
            var countries = await _service.CreateCountryByAsync(request);
           return Ok(countries);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<CountryResponse>> UpdateAsync(int id, [FromBody]CountryRequest request)
        {
            var countries = await _service.UpdateCountryByAsync(id, request);
            if (countries == null) return NotFound();
            return Ok(countries);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var countries = await _service.DeletedAsync(id);
            if (!countries) return NotFound();
            return NoContent();
        }

        [HttpPut("{id}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            var result = await _service.ActivateAsync(id);
            if (!result) return NotFound();
            return Ok(new { message = "Pais reactivado exitosamente" });
        }
    }
}
