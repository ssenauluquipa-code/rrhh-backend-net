using Microsoft.EntityFrameworkCore;
using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Interfaces.Repositories;

namespace Rrhh_backend.Infrastructure.Data.Repositories
{
    public class PermissionTypeRepositoryEf : IPermissionTypeRepository
    {
        private readonly NebulaDbContext _context;

        public PermissionTypeRepositoryEf(NebulaDbContext context)
        {
            _context = context;
        }

        public async Task<List<PermissionType>> GetAllAsync()
        {
            return await _context.PermissionTypes.ToListAsync();
        }
    }
}
