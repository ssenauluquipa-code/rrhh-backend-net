using Microsoft.EntityFrameworkCore;
using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Entities.ModuleEspecial;
using Rrhh_backend.Core.Interfaces.Repositories;
using Rrhh_backend.Presentation.DTOs;
using Rrhh_backend.Presentation.DTOs.Requests.Permission;
using Rrhh_backend.Presentation.DTOs.Responses.Permissions;
using System.Collections.Generic;

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
                        FunctionId = permissionDetail.FunctionId,
                        PermissionTypeId = permissionTypeId,
                        IsActive = true,
                        AssignedAt = DateTime.UtcNow
                    });
                }
            }

            await _context.Permissions.AddRangeAsync(newPermissions);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ModulePermissionAssignment>> GetAllModulesAsync()
        {
            var modules = await _context.Modules
                .Include(m => m.Functions)
                .ToListAsync();
            var result = new List<ModulePermissionAssignment>();

            foreach(var module in modules)
            {
                var moduleDto = new ModulePermissionAssignment
                {
                    ModuleId = module.ModuleId,
                    ModuleName = module.ModuleName,
                    ModuleKey = module.ModuleKey,
                    Category = module.Category,
                    Functions = module.Functions.Select(f => new FunctionPermissionAssignment
                    {
                        FunctionId = f.FunctionId,
                        FunctionName = f.FunctionName,
                        Description = f.Description,
                        Permissions = new List<PermissionTypeAssignment>() // ✅ Vacío porque no se cargan permisos aquí
                    }).ToList()
                };
                result.Add(moduleDto);
            }
            return result;
        }

        public async Task<List<PermissionType>> GetAllPermissionTypesAsync()
        {
            return await _context.PermissionTypes.ToListAsync();
        }

        public async Task<PermissionAssignmentResponse> GetByRoleIdAsync(int roleId)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
            if (role == null) return null;

            // 1. Cargamos solo los módulos y funciones que tienen permisos asignados para este rol
            // Usamos Join para asegurarnos de traer solo "la verdad" de la tabla Permissions
            var assignmentPermission = await _context.Permissions
                .Include(p => p.PermissionType)
                .Where(p => p.RoleId == roleId)
                .ToListAsync();

            // Obtenemos los IDs únicos de módulos y funciones que tienen algún permiso
            var moduleIdsWithPermissions = assignmentPermission.Select(ap => ap.ModuleId).Distinct();
            var functionIdsWithPermissions = assignmentPermission.Select(ap => ap.FunctionId).Distinct();

            // 2. Cargamos la estructura completa de módulos y funciones 
            // pero solo de aquellos que están en la tabla de permisos
            var modules = await _context.Modules
                .Include(m => m.Functions)
                .Where(m => moduleIdsWithPermissions.Contains(m.ModuleId))
                .ToListAsync();

            var response = new PermissionAssignmentResponse
            {
                RoleId = roleId,
                RoleName = role.RoleName,
                Modules = new List<ModulePermissionAssignment>()
            };

            foreach (var module in modules)
            {
                var moduleAssignment = new ModulePermissionAssignment
                {
                    ModuleId = module.ModuleId,
                    ModuleName = module.ModuleName,
                    ModuleKey = module.ModuleKey,
                    Category = module.Category,
                    Functions = new List<FunctionPermissionAssignment>()
                };

                // Filtramos solo las funciones de este módulo que tienen permisos
                var functionsWithPermissions = module.Functions
                    .Where(f => functionIdsWithPermissions.Contains(f.FunctionId));

                foreach (var function in functionsWithPermissions)
                {
                    var functionAssignment = new FunctionPermissionAssignment
                    {
                        FunctionId = function.FunctionId,
                        FunctionName = function.FunctionName,
                        Description = function.Description,
                        // CLAVE: Aquí ya NO usamos allPermissionTypes.Select
                        // Usamos directamente lo que hay en assignmentPermission para esta función
                        Permissions = assignmentPermission
                            .Where(ap => ap.FunctionId == function.FunctionId)
                            .Select(ap => new PermissionTypeAssignment
                            {
                                PermissionTypeId = ap.PermissionTypeId,
                                PermissionTypeName = ap.PermissionType.PermissionTypeName,
                                IsActive = ap.IsActive
                            }).ToList()
                    };
                    moduleAssignment.Functions.Add(functionAssignment);
                }

                if (moduleAssignment.Functions.Any())
                {
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
