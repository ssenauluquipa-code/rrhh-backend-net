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
                    return new PermissionResponse { Permissions = new Dictionary<int, List<FunctionPermission>>() };

                var permissionsDict = new Dictionary<int, List<FunctionPermission>>();


                foreach (var perm in permissions)
                {
                    if (perm.Module?.ModuleKey == null ||
                        perm.Function?.FunctionName == null ||
                        perm.PermissionType?.Code == null)
                        continue;

                    var moduleId = perm.Module.ModuleId;
                    var functionId = perm.Function.FunctionId;
                    var permCode = perm.PermissionType.Code.Trim();

                    if (string.IsNullOrEmpty(permCode))
                        continue;

                    if (!permissionsDict.ContainsKey(moduleId))
                        permissionsDict[moduleId] = new List<FunctionPermission>();

                    var functionPerm = permissionsDict[moduleId]
                        .FirstOrDefault(f => f.FunctionId == functionId);

                    if (functionPerm == null)
                    {
                        // Crear nueva función
                        functionPerm = new FunctionPermission
                        {
                            FunctionId = functionId,
                            FunctionName = perm.Function.FunctionName,
                            Permissions = new List<string>()
                        };
                        permissionsDict[moduleId].Add(functionPerm);
                    }

                    functionPerm.Permissions.Add(permCode);
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
