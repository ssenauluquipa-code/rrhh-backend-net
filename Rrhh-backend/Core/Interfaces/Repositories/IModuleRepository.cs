using Rrhh_backend.Core.Entities.ModuleEspecial;

namespace Rrhh_backend.Core.Interfaces.Repositories
{
    public interface IModuleRepository
    {
        Task<List<Module>> GetAllAsync();
    }
}
