using Microsoft.EntityFrameworkCore;
using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Interfaces.Repositories;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Infrastructure.Data;
using Rrhh_backend.Presentation.DTOs.Requests;
using Rrhh_backend.Presentation.DTOs.Responses;

namespace Rrhh_backend.Infrastructure.Services
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly RrhhDbContext _context;
        private readonly IRolePermissionRepository _rolePermissionRepo;

        public RolePermissionService(RrhhDbContext context, IRolePermissionRepository rolePermissionRepo)
        {
            _context = context;
            _rolePermissionRepo = rolePermissionRepo;
        }

        public async Task<bool> AssignPermissionToRoleAsync(AssignPermissionRequest request)
        {
            // Verificar que el rol y el permiso existan
            var roleExists = await _context.Roles.AnyAsync(r => r.Id == request.RoleId);
            var permissionExists = await _context.Permissions.AnyAsync(p => p.Id == request.PermissionId);

            if (!roleExists || !permissionExists)
            {
                return false;
            }

            // Verificar si ya existe la asignación
            var existing = await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == request.RoleId && rp.PermissionId == request.PermissionId);

            if (existing != null)
            {
                // Actualizar si ya existe
                existing.IsGranted = true;
                existing.GrantedAt = DateTime.UtcNow;
            }
            else
            {
                // Crear nueva asignación
                var rolePermission = new RolePermission
                {
                    RoleId = request.RoleId,
                    PermissionId = request.PermissionId,
                    IsGranted = true,
                    GrantedAt = DateTime.UtcNow
                };
                _context.RolePermissions.Add(rolePermission);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<string>> GetPermissionsByRoleNameAsync(string roleName)
        {
            var roleId = await _context.Roles
            .Where(r => r.RoleName == roleName && r.IsActive)
            .Select(r => r.Id)
            .FirstOrDefaultAsync();

            if (roleId == 0) return new List<string>();

            var permissionNames = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId && rp.IsGranted)
                .Join(_context.Permissions,
                      rp => rp.PermissionId,
                      p => p.Id,
                      (rp, p) => p.PermissionsName)
                .ToListAsync();

            return permissionNames;
        }

        public async Task<bool> RevokePermissionFromRoleAsync(int roleId, int permissionId)
        {
            var assignment = await _context.RolePermissions
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (assignment != null)
            {
                assignment.IsGranted = false; // Soft revoke
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        
        public Task<List<RolePermissionResponse>> GetRoleAssignmentsAsync(int roleId)
        {
            throw new NotImplementedException();
        }
    }
}
