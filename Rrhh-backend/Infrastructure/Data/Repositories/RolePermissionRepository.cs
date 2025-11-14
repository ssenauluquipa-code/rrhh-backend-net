
using Microsoft.EntityFrameworkCore;
using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Interfaces.Repositories;

namespace Rrhh_backend.Infrastructure.Data.Repositories
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly RrhhDbContext _context;

        public RolePermissionRepository(RrhhDbContext context)
        {
            _context = context;
        }

        public async Task<List<RolePermission>> GetAllByRoleIdAsync(int roleId)
        {
            return await _context.RolePermissions
            .Include(rp => rp.Role)
            .Include(rp => rp.Permission)
            .Where(rp => rp.RoleId == roleId && rp.IsGranted)
            .ToListAsync();
        }
        public async Task<List<RolePermission>> GetAllByPermissionIdAsync(int permissionId)
        {
            return await _context.RolePermissions
            .Include(rp => rp.Role)
            .Include(rp => rp.Permission)
            .Where(rp => rp.PermissionId == permissionId && rp.IsGranted)
            .ToListAsync();
        }
        public async Task<RolePermission> AssignPermissionToRoleAsync(int roleId, int permissionId)
        {
            var existing = await _context.RolePermissions
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (existing != null)
            {
                existing.IsGranted = true;
                existing.GrantedAt = DateTime.UtcNow;
            }
            else
            {
                existing = new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permissionId,
                    IsGranted = true,
                    GrantedAt = DateTime.UtcNow
                };
                _context.RolePermissions.Add(existing);
            }

            await _context.SaveChangesAsync();
            return existing;
        }
        public async Task<bool> RevokePermissionFromRoleAsync(int roleId, int permissionId)
        {
            var assignment = await _context.RolePermissions
             .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (assignment != null)
            {
                assignment.IsGranted = false;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<bool> CheckPermissionForRoleAsync(int roleId, int permissionId)
        {
            var assignment = await _context.RolePermissions
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId && rp.IsGranted);

            return assignment != null;
        }


    }
}
