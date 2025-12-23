using Microsoft.EntityFrameworkCore;
using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Entities.ModuleEspecial;
using Rrhh_backend.Core.Interfaces.Repositories;
using Rrhh_backend.Presentation.DTOs.Requests.Permission;
using Rrhh_backend.Presentation.DTOs.Responses.Permissions;

namespace Rrhh_backend.Infrastructure.Data.Repositories
{
    public class PermissionAssignmentRepositoryEf : IPermissionAssignmentRepository
    {
        private readonly NebulaDbContext _context;

        public PermissionAssignmentRepositoryEf(NebulaDbContext nebulaDbContext)
        {
            _context = nebulaDbContext;
        }

        public async Task AssignPermissionsAsync(PermissionAssignmentRequest request)
        {
            // Eliminar permisos existentes para el rol
            var existingPermissions = await _context.Permissions
                .Where(p => p.RoleId == request.RoleId)
                .ToListAsync();

            _context.Permissions.RemoveRange(existingPermissions);

            // Crear nuevos permisos
            var newPermissions = new List<Permission>();
            foreach (var permissionDetail in request.Permissions)
            {
                foreach (var permissionTypeId in permissionDetail.PermissionTypeIds)
                {
                    newPermissions.Add(new Permission
                    {
                        RoleId = request.RoleId,
                        ModuleId = permissionDetail.ModuleId,
                        PermissionTypeId = permissionTypeId,
                        IsActive = true,
                        AssignedAt = DateTime.UtcNow
                    });
                }
            }

            await _context.Permissions.AddRangeAsync(newPermissions);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Module>> GetAllModulesAsync()
        {
            return await _context.Modules.ToListAsync();
        }

        public async Task<List<PermissionType>> GetAllPermissionTypesAsync()
        {
            return await _context.PermissionTypes.ToListAsync();
        }

        public async Task<PermissionAssignmentResponse> GetByRoleIdAsync(int roleId)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
            if (role == null) return null;

            var permissions = await _context.Permissions
                .Include(p => p.Module)
                .Include(p => p.PermissionType)
                .Where(p => p.RoleId == roleId)
                .ToListAsync();

            var response = new PermissionAssignmentResponse
            {
                RoleId = roleId,
                RoleName = role.RoleName,
                Modules = new List<ModulePermissionAssignment>()
            };

            // Agrupar permisos por módulo
            var groupedByModule = permissions
                .GroupBy(p => p.ModuleId)
                .ToList();
            foreach (var moduleGroup in groupedByModule)
            {
                var module = await _context.Modules.FirstOrDefaultAsync(m => m.ModuleId == moduleGroup.Key);
                if (module != null)
                {
                    var moduleAssignment = new ModulePermissionAssignment
                    {
                        ModuleId = module.ModuleId,
                        ModuleName = module.ModuleName,
                        ModuleKey = module.ModuleKey,
                        Permissions = moduleGroup.Select(p => new PermissionTypeAssignment
                        {
                            PermissionTypeId = p.PermissionTypeId,
                            PermissionTypeName = p.PermissionType.PermissionTypeName,
                            IsActive = p.IsActive
                        }).ToList()
                    };

                    response.Modules.Add(moduleAssignment);
                }
            }

            return response;
        }

        public async Task RemovePermissionsByRoleAsync(int roleId)
        {
            var permissions = await _context.Permissions
    .Where(p => p.RoleId == roleId)
    .ToListAsync();

            _context.Permissions.RemoveRange(permissions);
            await _context.SaveChangesAsync();
        }
    }
}
