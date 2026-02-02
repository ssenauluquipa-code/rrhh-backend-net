using Rrhh_backend.Core.Entities.ModuleEspecial;
using Rrhh_backend.Core.Interfaces.Repositories;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Requests.Company;
using Rrhh_backend.Presentation.DTOs.Responses.Company;

namespace Rrhh_backend.Infrastructure.Services
{
    public class CompanyService :ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        public async Task<IEnumerable<CompanyResponse>> GetAllAsync()
        {
            var companies = await _companyRepository.GetAllCompaniesAsync();
            return companies.Select(MapToResponse).ToList();
        }

        public async Task<CompanyResponse?> GetByIdAsync(int id)
        {
            var company = await _companyRepository.GetCompanyByIdAsync(id);
            return company == null ? null : MapToResponse(company);
        }
        public async Task<CompanyResponse> CreateAsync(CompanyRequest companyDto)
        {
            var company = new Companies
            {
                CompanyName = companyDto.CompanyName,
                TaxId = companyDto.TaxId,
                Address = companyDto.Address,
                CountryId = companyDto.CountryId,
                IsActive = true
            };
            var result = await _companyRepository.CreateCompanyAsync(company);
            return MapToResponse(result);
        }
        public async Task<CompanyResponse> UpdateAsync(int id, CompanyRequest companyDto)
        {
            var company = await _companyRepository.GetCompanyByIdAsync(id);
            if(company == null) return null;
            
            company.CompanyName = companyDto.CompanyName;
            company.TaxId = companyDto.TaxId;
            company.Address = companyDto.Address;
            company.CountryId = companyDto.CountryId;
            company.UpdatedAt = DateTime.UtcNow;

            var result = await _companyRepository.UpdateCompanyAsync(company);
            return MapToResponse(result);

        }
        public async Task<bool> DeleteAsync(int id)
        {
            var companies = await _companyRepository.DeleteCompanyAsync(id);
            return companies;
        }
        public async Task<bool> ChangeStatusAsync(int id)
        {
            var companies = await _companyRepository.ChangeStatus(id);
            return companies;
        }


        private static CompanyResponse MapToResponse(Companies c) => new CompanyResponse
        {
            CompanyId = c.CompanyId,
            CompanyName = c.CompanyName,
            TaxId = c.TaxId,
            Address = c.Address,
            CountryName = c.Country?.CountryName ?? "N/A",
            IsActive = c.IsActive
        };
    }
}
