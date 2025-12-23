using Rrhh_backend.Core.Entities;
using Rrhh_backend.Presentation.DTOs.Responses.Permissions;

namespace Rrhh_backend.Core.Interfaces.Repositories
{
    public interface IPermissionTypeRepository
    {
        Task<List<PermissionType>> GetAllAsync();
        Task<PermissionType?> GetByIdAsync(int id);
        Task<PermissionType> CreateAsync(PermissionType permissionType);
        Task<PermissionType?> UpdateAsync(int id, PermissionType permissionType);
        Task<bool> DeleteAsync(int id);

        // === MÉTODO NUEVO ===
        Task<PermissionAssignmentResponse> GetByRoleIdAsync(int roleId);
    }
}
