using Rrhh_backend.Core.Entities;

namespace Rrhh_backend.Core.Interfaces.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAll();
        Task<IEnumerable<Department>> GetAllByCompanyAsync(int companyId);
        Task<Department> GetByIdAsync(int id, int companyId);
        Task CreateAsync(Department department);
        Task UpdateAsync(Department department);
        Task SaveChangeAsync();
    }
}
