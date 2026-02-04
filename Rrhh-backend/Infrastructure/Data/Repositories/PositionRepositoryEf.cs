using Microsoft.EntityFrameworkCore;
using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Interfaces.Repositories;

namespace Rrhh_backend.Infrastructure.Data.Repositories
{
    public class PositionRepositoryEf : IPositionRepository
    {
        private readonly NebulaDbContext _context;
        public PositionRepositoryEf(NebulaDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Position position)
        {
             await _context.Positions.AddAsync(position);
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Position>> GetAllPosition()
        {
            return await _context.Positions.Include(p => p.Department).ToListAsync();
        }

        public async Task<IEnumerable<Position>> GetByCompanyAsync(int companyId)
        {
            return await _context.Positions.Include(p => p.Department).Where(p => p.CompanyId == companyId).ToListAsync();
        }

        public async Task<Position?> GetByIdAsync(int id, int companyId)
        {
            return await _context.Positions.FirstOrDefaultAsync(p => p.PositionId == id && p.CompanyId == companyId);
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Position position)
        {
            _context.Positions.Update(position);
            await Task.CompletedTask;
        }
    }
}
