using Rrhh_backend.Core.Entities;

namespace Rrhh_backend.Core.Interfaces.Repositories
{
    public interface IRolePermissionRepository
    {
        Task<List<RolePermission>> GetAllByRoleIdAsync(int roleId);
        Task<List<RolePermission>> GetAllByPermissionIdAsync(int permissionId);
        Task<RolePermission> AssignPermissionToRoleAsync(int roleId, int permissionId);
        Task<bool> RevokePermissionFromRoleAsync(int roleId, int permissionId);
        Task<bool> CheckPermissionForRoleAsync(int roleId, int permissionId);
    }
}
