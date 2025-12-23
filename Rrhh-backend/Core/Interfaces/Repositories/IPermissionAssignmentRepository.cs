using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Entities.ModuleEspecial;
using Rrhh_backend.Presentation.DTOs.Requests.Permission;
using Rrhh_backend.Presentation.DTOs.Responses.Permissions;

namespace Rrhh_backend.Core.Interfaces.Repositories
{
    public interface IPermissionAssignmentRepository
    {
        Task<PermissionAssignmentResponse> GetByRoleIdAsync(int roleId);
        Task AssignPermissionsAsync(PermissionAssignmentRequest request);
        Task RemovePermissionsByRoleAsync(int roleId);
        Task<List<Module>> GetAllModulesAsync();
        Task<List<PermissionType>> GetAllPermissionTypesAsync();
    }
}
