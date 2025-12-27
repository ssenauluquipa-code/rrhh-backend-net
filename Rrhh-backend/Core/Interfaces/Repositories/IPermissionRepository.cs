using Rrhh_backend.Core.Entities;

namespace Rrhh_backend.Core.Interfaces.Repositories
{
    public interface IPermissionRepository
    {

        Task<List<Permission>> GetActiveByRoleIdAsync(int roleId);
    }
}
