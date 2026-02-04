using Rrhh_backend.Presentation.DTOs.Requests.Position;
using Rrhh_backend.Presentation.DTOs.Responses.Position;

namespace Rrhh_backend.Core.Interfaces.Services
{
    public interface IPositionService
    {
        Task<IEnumerable<PositionResponse>> GetAllAsync();
        Task<IEnumerable<PositionResponse>> GetByCompanyAsync(int companyId);
        Task<bool> CreateAsync(PositionRequest request);
        Task<bool> UpdateAsync(int id, PositionRequest request); // EDITAR
        Task<bool> ChangeStatusAsync(int id, int companyId, bool status);
    }
}
