using Rrhh_backend.Core.Entities.ModuleEspecial;

namespace Rrhh_backend.Core.Interfaces.Repositories
{
    public interface ICompanyRepository
    {
        Task<Company> CreateAsync(Company company);
        Task<Company> GetByNameAsync(string name);
    }
}
