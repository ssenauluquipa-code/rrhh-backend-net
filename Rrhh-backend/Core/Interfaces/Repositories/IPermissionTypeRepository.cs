using Rrhh_backend.Core.Entities;

namespace Rrhh_backend.Core.Interfaces.Repositories
{
    public interface IPermissionTypeRepository
    {
        Task<List<PermissionType>> GetAllAsync();
    }
}
