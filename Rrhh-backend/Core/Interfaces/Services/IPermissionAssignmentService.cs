using Rrhh_backend.Presentation.DTOs.Requests;
using Rrhh_backend.Presentation.DTOs.Responses;

namespace Rrhh_backend.Core.Interfaces.Services
{
    public interface IPermissionAssignmentService
    {
        Task<PermissionAssignmentResponse> LoadAssignmentAsync(int roleId);
        Task SaveAssignmentAsync(PermissionAssignmentRequest request);
    }
}
