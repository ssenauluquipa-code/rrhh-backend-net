using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Responses;

namespace Rrhh_backend.Infrastructure.Services
{
    public class ModuleService : IModuleService
    {
        public Task<List<ModuleResponse>> GetModulesByRoleAsync(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
