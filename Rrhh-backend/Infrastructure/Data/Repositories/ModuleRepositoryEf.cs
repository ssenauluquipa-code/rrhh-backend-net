using Microsoft.EntityFrameworkCore;
using Rrhh_backend.Core.Entities.ModuleEspecial;
using Rrhh_backend.Core.Interfaces.Repositories;

namespace Rrhh_backend.Infrastructure.Data.Repositories
{
    public class ModuleRepositoryEf : IModuleRepository
    {
        private readonly NebulaDbContext _context;

        public ModuleRepositoryEf(NebulaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Module>> GetAllAsync()
        {
            return await _context.Modules
                .Where(m => m.IsActive)
                .ToListAsync();
        }
    }
}
