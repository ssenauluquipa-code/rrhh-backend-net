using Rrhh_backend.Core.Entities;
using Rrhh_backend.Presentation.DTOs.Requests.Departament;
using Rrhh_backend.Presentation.DTOs.Responses.Departament;

namespace Rrhh_backend.Core.Interfaces.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartamentResponse>> GetAll();
        Task<IEnumerable<DepartamentResponse>> GetListAsync(int companyId);
        Task<bool> CreateAsync(DepartamentRequest request);
        Task<bool> UpdateAsync(int id,DepartamentRequest request);
        Task<bool> DeleteAsync(int id, int companyId);
    }
}
