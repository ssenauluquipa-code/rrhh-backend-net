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

            var allModules = await _context.Modules
                .Include(m => m.Functions)
                .ToListAsync();

            var allPermissionTypes = await _context.PermissionTypes.ToListAsync();

            var assignmentPermission = await _context.Permissions
                .Where(p => p.RoleId == roleId)
                .ToListAsync();

            var response = new PermissionAssignmentResponse
            {
                RoleId = roleId,
                RoleName = role.RoleName,
                Modules = new List<ModulePermissionAssignment>()
            };

            foreach (var module in allModules)
            {     
                    var moduleAssignment = new ModulePermissionAssignment
                    {
                        ModuleId = module.ModuleId,
                        ModuleName = module.ModuleName,
                        ModuleKey = module.ModuleKey,
                        Category = module.Category,
                        Functions = new List<FunctionPermissionAssignment>(),                        
                    };                
                    foreach (var function in module.Functions)
                    {                        
                            var functionAssignment = new FunctionPermissionAssignment
                            {
                                FunctionId = function.FunctionId,
                                FunctionName = function.FunctionName,
                                Description = function.Description,
                                Permissions = allPermissionTypes.Select(p => new PermissionTypeAssignment
                                {
                                    PermissionTypeId = p.PermissionTypeId,
                                    PermissionTypeName = p.PermissionTypeName,
                                    IsActive = assignmentPermission.Any(ap => 
                                    ap.FunctionId == function.FunctionId &&
                                    ap.PermissionTypeId == p.PermissionTypeId
                                    )
                                }).ToList()
                            };
                            moduleAssignment.Functions.Add(functionAssignment);                                            
                    }
                    response.Modules.Add(moduleAssignment);                
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
