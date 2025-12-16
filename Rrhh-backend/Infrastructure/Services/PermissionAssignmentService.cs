using Microsoft.EntityFrameworkCore;
using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Interfaces.Repositories;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Infrastructure.Data;
using Rrhh_backend.Presentation.DTOs.Requests;
using Rrhh_backend.Presentation.DTOs.Responses;

namespace Rrhh_backend.Infrastructure.Services
{
    public class PermissionAssignmentService : IPermissionAssignmentService
    {
        private readonly NebulaDbContext _context;
        private readonly IRolesRepository _roleRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IPermissionTypeRepository _permissionTypeRepository;

        public PermissionAssignmentService(NebulaDbContext context,
                                           IRolesRepository roleRepository,
                                           IModuleRepository moduleRepository,
                                           IPermissionTypeRepository permissionTypeRepository)
        {
            _context = context;
            _roleRepository = roleRepository;
            _moduleRepository = moduleRepository;
            _permissionTypeRepository = permissionTypeRepository;
        }

        public async Task<PermissionAssignmentResponse> LoadAssignmentAsync(int roleId)
        {
            var role = await _roleRepository.GetRolesByIdAsync(roleId);
            if (role == null)
                throw new KeyNotFoundException("Rol no encontrado");

            var modules = await _moduleRepository.GetAllAsync();
            var permissionTypes = await _permissionTypeRepository.GetAllAsync();
            var currentPermissions = await _context.Permissions
                .Where(p => p.RoleId == roleId && p.IsActive)
                .ToListAsync();

            var permDict = currentPermissions
                .ToDictionary(p => (p.ModuleId, p.PermissionTypeId), p => true);

            var categorizedModules = new Dictionary<string, List<ModulePermissionItem>>();
            foreach (var module in modules)
            {
                var permissions = new Dictionary<string, bool>();
                foreach (var pt in permissionTypes)
                {
                    var key = (ModuleId: module.ModuleId, PermissionTypeId: pt.PermissionTypeId);
                    permissions[pt.Code] = permDict.ContainsKey(key);
                }
                var item = new ModulePermissionItem
                {
                    ModuleId = module.ModuleId,
                    Name = module.ModuleName,
                    Key = module.ModuleKey,
                    Permissions = permissions
                };

                if (!categorizedModules.ContainsKey(module.Category))
                    categorizedModules[module.Category] = new List<ModulePermissionItem>();

                categorizedModules[module.Category].Add(item);
            }

            var responseModule = new ModulePermissionResponse
            {
                ModuleId = modules.FirstOrDefault()?.ModuleId ?? 0,
                Name = modules.FirstOrDefault()?.ModuleName ?? "",
                Key = modules.FirstOrDefault()?.ModuleKey ?? "",
                Categories = categorizedModules
            };

            return new PermissionAssignmentResponse
            {
                RoleId = roleId,
                RoleName = role.RoleName,
                Modules = new List<ModulePermissionResponse> { responseModule }
            };
        }

        public async Task SaveAssignmentAsync(PermissionAssignmentRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existingPermissions = await _context.Permissions
                    .Where(p => p.RoleId == request.RoleId)
                    .ToListAsync();

                _context.Permissions.RemoveRange(existingPermissions);

                var permissionTypes = await _permissionTypeRepository.GetAllAsync();
                var permTypeDict = permissionTypes.ToDictionary(pt => pt.Code, pt => pt.PermissionTypeId);

                foreach (var moduleDto in request.Modules)
                {
                    foreach (var (permCode, isActive) in moduleDto.Permissions)
                    {
                        if (isActive && permTypeDict.TryGetValue(permCode, out var permTypeId))
                        {
                            _context.Permissions.Add(new Permission
                            {
                                RoleId = request.RoleId,
                                ModuleId = moduleDto.ModuleId,
                                PermissionTypeId = permTypeId,
                                IsActive = true,
                                AssignedAt = DateTime.UtcNow
                            });
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
