using Microsoft.EntityFrameworkCore;
using Rrhh_backend.Core.Entities.ModuleEspecial;
using Rrhh_backend.Core.Interfaces.Repositories;

namespace Rrhh_backend.Infrastructure.Data.Repositories
{
    public class FunctionRepositoryEf : IFunctionRepository
    {
        private readonly NebulaDbContext _context;

        public FunctionRepositoryEf(NebulaDbContext context)
        {
            _context = context;
        }

        public async Task<Function?> GetFunctionByIdAsync(int id)
        {
            return await _context.Functions.FindAsync(id);
        }

        public async Task<IEnumerable<Function>> GetFunctionsByModuleAsync(int moduleId)
        {
            return await _context.Functions
           .Include(f => f.Permissions)
           .ThenInclude(p => p.PermissionType)
           .Where(f => f.ModuleId == moduleId)
           .ToListAsync();
        }
    }
}
