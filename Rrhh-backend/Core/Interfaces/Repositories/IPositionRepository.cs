using Rrhh_backend.Core.Entities;

namespace Rrhh_backend.Core.Interfaces.Repositories
{
    public interface IPositionRepository
    {
        Task<IEnumerable<Position>> GetAllPosition();
        Task<IEnumerable<Position>> GetByCompanyAsync(int companyId);
        Task<Position> GetByIdAsync(int id, int companyId);
        Task CreateAsync(Position position);
        Task UpdateAsync(Position position);
        Task DeleteAsync(int id);
        Task SaveChangeAsync();
    }
}
