using Rrhh_backend.Core.Entities;

namespace Rrhh_backend.Core.Interfaces.Repositories
{
    public interface IPermissionRepository
    {
        Task<List<Permission>> GetAllAsync();
        Task<Permission?> GetByIdAsync();
        Task<Permission> CreateAsync(Permission permission);
        Task<Permission> UpdateAsync(int id,Permission permission);
        Task<Permission> DeleteAsync(int id);
    }
}
