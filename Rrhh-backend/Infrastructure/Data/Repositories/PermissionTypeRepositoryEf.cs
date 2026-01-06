using Microsoft.EntityFrameworkCore;
using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Interfaces.Repositories;
using Rrhh_backend.Presentation.DTOs.Responses.Permissions;

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

        public async Task<PermissionType?> GetByIdAsync(int id)
        {
            return await _context.PermissionTypes.FindAsync(id);
        }         
    }
}
