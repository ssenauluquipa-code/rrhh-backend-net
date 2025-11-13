using Rrhh_backend.Core.Entities;

namespace Rrhh_backend.Core.Interfaces.Repositories
{
    public interface IRolesRepository
    {
        Task<List<Role>> GetRolesAll();
        Task<Role> GetRolesById(int id);
        Task<Role> CreatedRoles(Role roles);
        Task<Role> UpdatedRoles(int id, Role roles);
        Task<bool> Deleted(int id);
        Task<bool> IsActivateRoles(int id);
    }
}
