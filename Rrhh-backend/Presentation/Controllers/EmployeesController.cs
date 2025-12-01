using Microsoft.AspNetCore.Mvc;
using Rrhh_backend.Core.Exceptions;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Requests.Employee;
using Rrhh_backend.Presentation.DTOs.Responses.Employees;

namespace Rrhh_backend.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<EmployeeResponse>>> GetAll()
        {
            var employees = await _employeeService.GetAllAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeResponse>> GetById(int id)
        {
            try
            {
                var employee = await _employeeService.GetByIdAsync(id);
                return Ok(employee);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeResponse>> Create([FromBody] CreateEmployeeRequest request)
        {
            try
            {
                var employee = await _employeeService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeResponse>> Update(int id, [FromBody] UpdateEmployeeRequest request)
        {
            try
            {
                var employee = await _employeeService.UpdateAsync(id, request);
                return Ok(employee);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
