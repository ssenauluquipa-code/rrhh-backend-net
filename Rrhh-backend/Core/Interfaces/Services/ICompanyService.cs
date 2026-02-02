using Rrhh_backend.Presentation.DTOs.Requests.Company;
using Rrhh_backend.Presentation.DTOs.Responses.Company;

namespace Rrhh_backend.Core.Interfaces.Services
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyResponse>> GetAllAsync();
        Task<CompanyResponse> GetByIdAsync(int id);
        Task<CompanyResponse> CreateAsync(CompanyRequest companyDto);
        Task<CompanyResponse> UpdateAsync(int id,CompanyRequest companyDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ChangeStatusAsync(int id);
    }
}
