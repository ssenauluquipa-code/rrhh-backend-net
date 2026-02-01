using Rrhh_backend.Core.Entities;

namespace Rrhh_backend.Core.Interfaces.Repositories
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Countries>> GetCountries();
        Task<Countries> GetByNameCountries(string countriesName);
        Task<Countries> GetByIdAsync(int id);
        Task<Countries> CreateByAsync(Countries countries);
        Task<Countries> UpdateByAsync(Countries countries);
        Task<bool> DeleteByAsync(int id);
        Task<bool> IsActivateCountry(int id);
    }
}
