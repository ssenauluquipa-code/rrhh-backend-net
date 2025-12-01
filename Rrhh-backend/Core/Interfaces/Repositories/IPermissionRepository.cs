using Rrhh_backend.Core.Entities;

namespace Rrhh_backend.Core.Interfaces.Repositories
{
    public interface IPermissionRepository
    {
        Task<List<Permission>> GetAllAsync();
        Task<Permission?> GetByIdAsync(Guid id);
        Task<Permission> CreateAsync(Permission permission);
        Task<Permission> UpdateAsync(Permission permission);
        Task<Permission> DeleteAsync(Guid id);
    }
}
