using Rrhh_backend.Core.Entities.ModuleEspecial;

namespace Rrhh_backend.Core.Interfaces.Repositories
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Companies>> GetAllCompaniesAsync();
        Task<Companies> GetCompanyByIdAsync(int id);
        Task<Companies> CreateCompanyAsync(Companies company);
        Task<Companies> UpdateCompanyAsync(Companies company);
        Task<bool> DeleteCompanyAsync(int id);
        Task<bool> ChangeStatus(int id);
    }
}
