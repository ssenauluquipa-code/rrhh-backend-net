using Rrhh_backend.Presentation.DTOs.Requests.Employee;
using Rrhh_backend.Presentation.DTOs.Responses.Employees;

namespace Rrhh_backend.Core.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request);
        Task<EmployeeResponse> GetByIdAsync(Guid id);
        Task<EmployeeResponse> UpdateAsync(Guid id, UpdateEmployeeRequest request);
        Task<List<EmployeeResponse>> GetAllAsync();
    }
}
