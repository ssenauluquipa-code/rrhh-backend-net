using Rrhh_backend.Presentation.DTOs.Requests;
using Rrhh_backend.Presentation.DTOs.Responses;

namespace Rrhh_backend.Core.Interfaces.Services
{
    public interface IRolePermissionService
    {
        Task<bool> AssignPermissionToRoleAsync(AssignPermissionRequest request);
        Task<bool> RevokePermissionFromRoleAsync(int roleId, int permissionId);
        Task<List<string>> GetPermissionsByRoleNameAsync(string roleName);
        Task<List<RolePermissionResponse>> GetRoleAssignmentsAsync(int roleId);
    }
}
