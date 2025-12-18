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

        public async Task<PermissionResponse> GetUserPermissionsAsync(int roleId)
        {
            try
            {
                // 🔍 Diagnóstico: Verifica entrada
                Console.WriteLine($"🔍 Obteniendo permisos para RoleId: {roleId}");

                var permissions = await _permissionRepository.GetActiveByRoleIdAsync(roleId);
                Console.WriteLine($"🔍 Permisos encontrados: {permissions?.Count ?? 0}");

                if (permissions == null || !permissions.Any())
                {
                    Console.WriteLine("⚠️ No se encontraron permisos para este rol");
                    return new PermissionResponse { Permissions = new Dictionary<string, List<string>>() };
                }

                var modules = await _moduleRepository.GetAllAsync();
                var permissionTypes = await _permissionTypeRepository.GetAllAsync();

                Console.WriteLine($"🔍 Módulos cargados: {modules?.Count ?? 0}");
                Console.WriteLine($"🔍 Tipos de permiso cargados: {permissionTypes?.Count ?? 0}");

                // Verifica que no haya nulos en módulos/tipos
                if (modules == null || permissionTypes == null)
                    throw new InvalidOperationException("No se pudieron cargar módulos o tipos de permiso");

                // Crea diccionarios con validación
                var moduleDict = new Dictionary<int, string>();
                var typeDict = new Dictionary<int, string>();

                foreach (var m in modules)
                {
                    if (m?.ModuleId > 0 && !string.IsNullOrEmpty(m.ModuleKey))
                        moduleDict[m.ModuleId] = m.ModuleKey;
                }

                foreach (var pt in permissionTypes)
                {
                    if (pt?.PermissionTypeId > 0 && !string.IsNullOrEmpty(pt.Code))
                        typeDict[pt.PermissionTypeId] = pt.Code;
                }

                var permissionsDict = new Dictionary<string, List<string>>();

                foreach (var perm in permissions)
                {
                    if (moduleDict.TryGetValue(perm.ModuleId, out var moduleKey) &&
                        typeDict.TryGetValue(perm.PermissionTypeId, out var permCode))
                    {
                        if (!permissionsDict.ContainsKey(moduleKey))
                            permissionsDict[moduleKey] = new List<string>();
                        permissionsDict[moduleKey].Add(permCode);
                    }
                    else
                    {
                        Console.WriteLine($"⚠️ Permiso huérfano: ModuleId={perm.ModuleId}, PermissionTypeId={perm.PermissionTypeId}");
                    }
                }

                Console.WriteLine($"✅ Permisos procesados: {permissionsDict.Count} módulos");
                return new PermissionResponse { Permissions = permissionsDict };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 ERROR EN SERVICIO: {ex}");
                throw;
            }
        }


        //public async Task<PermissionResponse> GetUserPermissionsAsync(int roleId)
        //{
        //    // 1. Obtener todos los permisos activos del rol
        //    var permissions = await _permissionRepository.GetActiveByRoleIdAsync(roleId);

        //    // 2. Obtener todos los módulos y tipos de permiso
        //    var modules = await _moduleRepository.GetAllAsync();
        //    var permissionTypes = await _permissionTypeRepository.GetAllAsync();

        //    // 3. Crear diccionarios para mapeo rápido
        //    var moduleDict = modules.ToDictionary(m => m.ModuleId, m => m.ModuleKey);
        //    var permissionTypeDict = permissionTypes.ToDictionary(pt => pt.PermissionTypeId, pt => pt.Code);

        //    // 4. Construir el diccionario de permisos { "users": ["CREATE", "READ"] }
        //    var permissionsDict = new Dictionary<string, List<string>>();

        //    foreach (var perm in permissions)
        //    {
        //        if (moduleDict.TryGetValue(perm.ModuleId, out var moduleKey) &&
        //            permissionTypeDict.TryGetValue(perm.PermissionTypeId, out var permCode))
        //        {
        //            if (!permissionsDict.ContainsKey(moduleKey))
        //                permissionsDict[moduleKey] = new List<string>();

        //            permissionsDict[moduleKey].Add(permCode);
        //        }
        //    }

        //    return new PermissionResponse { Permissions = permissionsDict };
        //}
    }
}
