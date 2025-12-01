using Rrhh_backend.Presentation.DTOs.Requests.Roles;
using Rrhh_backend.Presentation.DTOs.Responses.Roles;

namespace Rrhh_backend.Core.Interfaces.Services
{
    public interface IRolesService
    {
        Task<List<RolesResponse>> GetRolesAllAsync();
        Task<RolesResponse> GetRolesByIdAsync(Guid id);
        Task<RolesResponse> CreatedRolesAsync(CreateRolesRequest request);
        Task<RolesResponse> UpdateRolesAsync(Guid id,UpdateRolesRequest request);
        Task<bool> DeletedAsync(Guid id);
        Task<bool> ActivatedAsync(Guid id);
    }
}
