using Rrhh_backend.Presentation.DTOs.Responses;

namespace Rrhh_backend.Core.Interfaces.Services
{
    public interface IPermissionService
    {
        Task<PermissionResponse> GetUserPermissionsAsync(int roleId);
    }
}
