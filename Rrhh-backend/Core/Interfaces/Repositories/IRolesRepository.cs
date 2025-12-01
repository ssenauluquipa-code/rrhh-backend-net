using Rrhh_backend.Core.Entities;

namespace Rrhh_backend.Core.Interfaces.Repositories
{
    public interface IRolesRepository
    {
        Task<List<Role>> GetRolesAllAsync();
        Task<Role> GetRolesByIdAsync(Guid id);
        Task<Role> GetByNameAsync(string name);
        Task<Role> CreatedRoles(Role roles);
        Task<Role> UpdatedRoles(Role roles);
        Task<bool> Deleted(Guid id);
        Task<bool> IsActivateRoles(Guid id);
    }
}
