using Microsoft.EntityFrameworkCore;
using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Entities.ModuleEspecial;
using Rrhh_backend.Core.Exceptions;
using Rrhh_backend.Core.Interfaces.Repositories;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Infrastructure.Data;
using Rrhh_backend.Presentation.DTOs.Requests.Permission;
using Rrhh_backend.Presentation.DTOs.Responses.Permissions;

namespace Rrhh_backend.Infrastructure.Services
{
    public class PermissionAssignmentService : IPermissionAssignmentService
    {
        private readonly NebulaDbContext _context;
        private readonly IRolesRepository _roleRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IFunctionRepository _functionRepository;
        private readonly IPermissionTypeRepository _permissionTypeRepository;
        private readonly IPermissionAssignmentRepository _permissionAssignmentRepository;

        public PermissionAssignmentService(NebulaDbContext context,
                                           IRolesRepository roleRepository,
                                           IModuleRepository moduleRepository,
                                           IFunctionRepository functionRepository,
                                           IPermissionTypeRepository permissionTypeRepository,
                                           IPermissionAssignmentRepository permissionAssignmentRepository
            )
        {
            _context = context;
            _roleRepository = roleRepository;
            _moduleRepository = moduleRepository;
            _functionRepository = functionRepository;
            _permissionTypeRepository = permissionTypeRepository;
            _permissionAssignmentRepository = permissionAssignmentRepository;
        }

        public async Task AssignPermissionsAsync(PermissionAssignmentRequest request)
        {
            var role = await _roleRepository.GetRolesByIdAsync(request.RoleId);
            if (role == null)
                throw new BusinessException("Rol no encontrado");

            // Validar que los módulos y tipos de permiso existan
            foreach (var permissionDetail in request.Permissions)
            {
                var module = await _moduleRepository.GetModulesByIdAsync(permissionDetail.ModuleId);
                if (module == null)
                    throw new BusinessException($"Módulo con ID {permissionDetail.ModuleId} no encontrado");

                if (permissionDetail.FunctionId.HasValue) // Validar función si existe
                {
                    var function = await _functionRepository.GetFunctionByIdAsync(permissionDetail.FunctionId.Value);
                    if (function == null)
                        throw new BusinessException($"Función con ID {permissionDetail.FunctionId} no encontrada");
                }


                foreach (var permissionTypeId in permissionDetail.PermissionTypeIds)
                {
                    var permissionType = await _permissionTypeRepository.GetByIdAsync(permissionTypeId);
                    if (permissionType == null)
                        throw new BusinessException($"Tipo de permiso con ID {permissionTypeId} no encontrado");
                }
            }

            await _permissionAssignmentRepository.AssignPermissionsAsync(request);
        }

        public async Task<List<ModulePermissionAssignment>> GetAllModulesAsync()
        {
            return await _permissionAssignmentRepository.GetAllModulesAsync();
        }

        public async Task<List<PermissionType>> GetAllPermissionTypesAsync()
        {
            return await _permissionAssignmentRepository.GetAllPermissionTypesAsync();
        }

        public async Task<PermissionAssignmentResponse> GetAssignmentByRoleAsync(int roleId)
        {
            var role = await _roleRepository.GetRolesByIdAsync(roleId);
            if (role == null)
                throw new BusinessException("Rol no encontrado");

            return await _permissionAssignmentRepository.GetByRoleIdAsync(roleId);
        }
    }
}
