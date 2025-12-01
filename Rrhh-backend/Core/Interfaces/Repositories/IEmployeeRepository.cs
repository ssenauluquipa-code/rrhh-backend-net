using Rrhh_backend.Core.Entities;

namespace Rrhh_backend.Core.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetEmployeeAll();
        Task<Employee> GetEmployeeByIdAsync(Guid id);
        Task<Employee> CreateByAsync(Employee employee);
        Task<Employee> UpdateByAsync(Employee employee);

    }
}
