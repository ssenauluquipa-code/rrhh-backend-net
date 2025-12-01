using Rrhh_backend.Presentation.DTOs.Requests.Roles;
using Rrhh_backend.Presentation.DTOs.Responses.Roles;

namespace Rrhh_backend.Core.Interfaces.Services
{
    public interface IRolesService
    {
        Task<List<RolesResponse>> GetRolesAllAsync();
        Task<RolesResponse> GetRolesByIdAsync(int id);
        Task<RolesResponse> CreatedRolesAsync(CreateRolesRequest request);
        Task<RolesResponse> UpdateRolesAsync(int id,UpdateRolesRequest request);
        Task<bool> DeletedAsync(int id);
        Task<bool> ActivatedAsync(int id);
    }
}
