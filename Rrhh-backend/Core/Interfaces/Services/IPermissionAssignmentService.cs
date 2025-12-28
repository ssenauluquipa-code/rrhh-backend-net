using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Entities.ModuleEspecial;
using Rrhh_backend.Presentation.DTOs.Requests.Permission;
using Rrhh_backend.Presentation.DTOs.Responses.Permissions;

namespace Rrhh_backend.Core.Interfaces.Services
{
    public interface IPermissionAssignmentService
    {
        Task<PermissionAssignmentResponse> GetAssignmentByRoleAsync(int roleId);
        Task AssignPermissionsAsync(PermissionAssignmentRequest request);
        Task<List<ModulePermissionAssignment>> GetAllModulesAsync();
        Task<List<PermissionType>> GetAllPermissionTypesAsync();
    }
}
