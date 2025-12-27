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
            try
            {
                var permissions = await _permissionRepository.GetActiveByRoleIdAsync(roleId);
                if (permissions == null || !permissions.Any())
                    return new PermissionResponse { Permissions = new Dictionary<string, List<string>>() };

                var permissionsDict = new Dictionary<string, List<string>>();

                foreach (var perm in permissions)
                {
                    if (perm.Module?.ModuleKey == null || perm.PermissionType?.Code == null)
                        continue;

                    var moduleKey = perm.Module.ModuleKey.Trim();
                    var permCode = perm.PermissionType.Code.Trim();

                    if (string.IsNullOrEmpty(moduleKey) || string.IsNullOrEmpty(permCode))
                        continue;

                    if (!permissionsDict.ContainsKey(moduleKey))
                        permissionsDict[moduleKey] = new List<string>();

                    permissionsDict[moduleKey].Add(permCode);
                }

                return new PermissionResponse { Permissions = permissionsDict };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR EN SERVICIO: {ex}");
                throw;
            }
        }
        
    }
}
