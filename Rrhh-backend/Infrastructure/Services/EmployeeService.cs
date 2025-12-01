using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Exceptions;
using Rrhh_backend.Core.Interfaces.Repositories;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Requests.Employee;
using Rrhh_backend.Presentation.DTOs.Responses.Employees;

namespace Rrhh_backend.Infrastructure.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request)
        {
            var employee = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Position = request.Position,
                Department = request.Department,
                Email = request.Email,
                Phone = request.Phone
            };

            await _employeeRepository.CreateByAsync(employee);
            return new EmployeeResponse
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                FullName = $"{employee.FirstName} {employee.LastName}",
                Position = employee.Position,
                Department = employee.Department,
                Email = employee.Email,
                Phone = employee.Phone,
                IsActive = employee.IsActive
            };
        }

        public async Task<List<EmployeeResponse>> GetAllAsync()
        {
            var employees = await _employeeRepository.GetEmployeeAll();
            return employees.Select(e => new EmployeeResponse
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                FullName = $"{e.FirstName} {e.LastName}",
                Position = e.Position,
                Department = e.Department,
                Email = e.Email,
                Phone = e.Phone,
                IsActive = e.IsActive
            }).ToList();
        }

        public async Task<EmployeeResponse> GetByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            return new EmployeeResponse
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                FullName = $"{employee.FirstName} {employee.LastName}",
                Position = employee.Position,
                Department = employee.Department,
                Email = employee.Email,
                Phone = employee.Phone,
                IsActive = employee.IsActive
            };
        }

        public async Task<EmployeeResponse> UpdateAsync(int id, UpdateEmployeeRequest request)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null)
                throw new BusinessException("Empleado no encontrado.");

            employee.FirstName = request.FirstName;
            employee.LastName = request.LastName;
            employee.Position = request.Position;
            employee.Department = request.Department;
            employee.Email = request.Email;
            employee.Phone = request.Phone;
            employee.IsActive = request.IsActive;

            await _employeeRepository.UpdateByAsync(employee);

            return new EmployeeResponse
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                FullName = $"{employee.FirstName} {employee.LastName}",
                Position = employee.Position,
                Department = employee.Department,
                Email = employee.Email,
                Phone = employee.Phone,
                IsActive = employee.IsActive
            };
        }
    }
}
