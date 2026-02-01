using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Exceptions;
using Rrhh_backend.Core.Interfaces.Repositories;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Requests.Countries;
using Rrhh_backend.Presentation.DTOs.Responses.Countries;

namespace Rrhh_backend.Infrastructure.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository) {
            _countryRepository = countryRepository;
        }
        
        public async Task<IEnumerable<CountryResponse>> GetAllCountriesAsync()
        {
            var countries = await _countryRepository.GetCountries();
            return countries.Select(c => new CountryResponse
            {
                CountryId = c.CountryId,
                CountryName = c.CountryName,
                IsoCode = c.IsoCode,
                Currency = c.Currency,
                IdentityLabel = c.IdentityLabel,
                IsActive = c.IsActive
            });

        }

        public async Task<CountryResponse> GetCountryByIdAsync(int countryId)
        {
            var country = await _countryRepository.GetByIdAsync(countryId);
            if (country == null) return null;
            return new CountryResponse
            {
                CountryId = country.CountryId,
                CountryName = country.CountryName,
                IsoCode = country.IsoCode,
                Currency = country.Currency,
                IdentityLabel = country.IdentityLabel
            };
        }

        public async Task<CountryResponse> UpdateCountryByAsync(int id, CountryRequest request)
        {
            var country = await _countryRepository.GetByIdAsync(id);
            if (country == null) return null;
                
            
            country.CountryName = request.CountryName;
            country.IsoCode = request.IsoCode;
            country.Currency = request.Currency;
            country.IdentityLabel = request.IdentityLabel;
            await _countryRepository.UpdateByAsync(country);

            return new CountryResponse
            {
                CountryId = country.CountryId,
                CountryName = country.CountryName,
                IsoCode = country.IsoCode,
                Currency = country.Currency,
                IdentityLabel = country.IdentityLabel,
                IsActive = country.IsActive
            };
        }

        public async Task<bool> ActivateAsync(int id)
        {
            return await _countryRepository.IsActivateCountry(id);
        }

        public async Task<CountryResponse> CreateCountryByAsync(CountryRequest request)
        {
            var existingCountry = await _countryRepository.GetByNameCountries(request.CountryName);
            if (existingCountry != null) throw new BusinessException("El nombre ya existe");

            var countries = new Countries
            {
                CountryName = request.CountryName,
                IsoCode = request.IsoCode,
                Currency = request.Currency,
                IdentityLabel = request.IdentityLabel,
            };
            await _countryRepository.CreateByAsync(countries);
            return new CountryResponse
            {
                CountryId = countries.CountryId,
                CountryName = countries.CountryName,
                IsoCode = countries.IsoCode,
                Currency = countries.Currency,
                IdentityLabel = countries.IdentityLabel,
                IsActive = countries.IsActive
            };
        }

        public async Task<bool> DeletedAsync(int id)
        {
            return await _countryRepository.DeleteByAsync(id);
        }
    }
}
