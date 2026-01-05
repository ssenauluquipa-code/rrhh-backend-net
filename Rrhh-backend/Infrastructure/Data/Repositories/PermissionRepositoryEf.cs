using Microsoft.EntityFrameworkCore;
using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Interfaces.Repositories;

namespace Rrhh_backend.Infrastructure.Data.Repositories
{
    public class PermissionRepositoryEf : IPermissionRepository
    {
        private readonly NebulaDbContext _context;

        public PermissionRepositoryEf(NebulaDbContext context)
        {
            _context = context;
        }        

        public async Task<List<Permission>> GetActiveByRoleIdAsync(int roleId)
        {
            var permissions = await _context.Permissions
                .Include(p => p.Module)
                .Include(p => p.Function)
                .Include(p => p.PermissionType)
                .Where(p => p.RoleId == roleId && p.IsActive)
                .ToListAsync();

            // Verificación adicional: asegura que las relaciones no sean nulas
            foreach (var perm in permissions)
            {
                if (perm.Module == null)
                {
                    var moduleId = perm.ModuleId;
                    perm.Module = await _context.Modules.FirstOrDefaultAsync(m => m.ModuleId == moduleId);
                }
                if (perm.PermissionType == null)
                {
                    var typeId = perm.PermissionTypeId;
                    perm.PermissionType = await _context.PermissionTypes.FirstOrDefaultAsync(pt => pt.PermissionTypeId == typeId);
                }
            }

            return permissions;
        }
    }
}
