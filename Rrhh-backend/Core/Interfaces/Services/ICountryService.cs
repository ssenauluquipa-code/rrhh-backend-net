using Rrhh_backend.Core.Entities;
using Rrhh_backend.Presentation.DTOs.Requests.Countries;
using Rrhh_backend.Presentation.DTOs.Responses.Countries;

namespace Rrhh_backend.Core.Interfaces.Services
{
    public interface ICountryService
    {
        Task<IEnumerable<CountryResponse>> GetAllCountriesAsync();
        Task<CountryResponse> GetCountryByIdAsync(int countryId);
        Task<CountryResponse> CreateCountryByAsync(CountryRequest request);
        Task<CountryResponse> UpdateCountryByAsync(int id, CountryRequest request);
        Task<bool> DeletedAsync(int id);
        Task<bool> ActivateAsync(int id);
    }
}
