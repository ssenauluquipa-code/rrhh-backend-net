using Rrhh_backend.Core.Entities.ModuleEspecial;

namespace Rrhh_backend.Core.Interfaces.Repositories
{
    public interface IFunctionRepository
    {
        Task<IEnumerable<Function>> GetFunctionsByModuleAsync(int moduleId);
        Task<Function?> GetFunctionByIdAsync(int id);
    }
}
