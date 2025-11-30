using Rrhh_backend.Presentation.DTOs.Requests.Tenants;
using Rrhh_backend.Presentation.DTOs.Responses.Tenants;

namespace Rrhh_backend.Core.Interfaces.Services
{
    public interface ITenantService
    {
        Task<TenantResponse> CreateTenantAsync(CreateTenantRequest request);
    }
}
