using Rrhh_backend.Presentation.DTOs.Responses;

namespace Rrhh_backend.Core.Interfaces.Services
{
    public interface IModuleService 
    {
        Task<List<ModuleResponse>> GetModulesByRoleAsync(string roleName);
    }
}
