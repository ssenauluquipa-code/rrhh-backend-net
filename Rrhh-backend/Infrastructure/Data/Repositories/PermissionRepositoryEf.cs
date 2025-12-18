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

        public async Task<List<Permission>> GetAllAsync()
        {
            return await _context.Permissions.Where(p => p.IsActive).ToListAsync();
        }
        public async Task<Permission?> GetByIdAsync(int id)
        {
            return await _context.Permissions.FirstOrDefaultAsync(p => p.PermissionId == id);
        }
        public async Task<Permission> CreateAsync(Permission permission)
        {
            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();
            return permission;
        }
        public Task<Permission> UpdateAsync(Permission permission)
        {
            throw new NotImplementedException();
        }
        public Task<Permission> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Permission>> GetActiveByRoleIdAsync(int roleId)
        {
            try
            {
                var permissions = await _context.Permissions
                    .Include(p => p.Module)
                    .Include(p => p.PermissionType)
                    .Where(p => p.RoleId == roleId && p.IsActive)
                    .ToListAsync();

                // Depuración: verifica que las relaciones se cargaron
                foreach (var p in permissions)
                {
                    Console.WriteLine($"Permiso: ModuleId={p.ModuleId}, ModuleKey={p.Module?.ModuleKey}, Code={p.PermissionType?.Code}");
                }

                return permissions;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 ERROR EN REPOSITORIO: {ex}");
                throw;
            }
        }
    }
}
