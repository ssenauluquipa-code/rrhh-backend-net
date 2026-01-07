using Rrhh_backend.Core.Interfaces.Repositories;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Responses;

namespace Rrhh_backend.Infrastructure.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IPermissionTypeRepository _permissionTypeRepository;

        public PermissionService(IPermissionRepository permissionRepository, 
                                 IModuleRepository moduleRepository, 
                                 IPermissionTypeRepository permissionTypeRepository)
        {
            _permissionRepository = permissionRepository;
            _moduleRepository = moduleRepository;
            _permissionTypeRepository = permissionTypeRepository;
        }

        // En PermissionService.cs
        public async Task<PermissionResponse> GetUserPermissionsAsync(int roleId)
        {
            var permissions = await _permissionRepository.GetActiveByRoleIdAsync(roleId);

            // Agrupar por ModuleName (ej: "access") en lugar de ModuleKey
            var permissionsDict = permissions
                .GroupBy(p => p.Module.ModuleName)  // "Access and Privileges", "Gestión de Personal", etc.
                .ToDictionary(
                    g => g.Key,
                    g => g.GroupBy(p => p.Function.FunctionId)
                          .Select(fg => new FunctionPermission
                          {
                              FunctionId = fg.Key,
                              FunctionName = fg.First().Function.FunctionName,
                              Permissions = fg.Select(p => p.PermissionTypeId).ToList()
                          })
                          .ToList()
                );

            return new PermissionResponse { Permissions = permissionsDict };
        }

    }
}
