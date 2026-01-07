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
            // Validar existencia del Rol
            var role = await _roleRepository.GetRolesByIdAsync(request.RoleId);
            if (role == null)
                throw new BusinessException("Rol no encontrado");

            // Recorrer cada detalle de la asignación (Módulo/Función)
            foreach (var permissionDetail in request.Permissions)
            {
                // Validar que el Módulo exista
                var module = await _moduleRepository.GetModulesByIdAsync(permissionDetail.ModuleId);
                if (module == null)
                    throw new BusinessException($"Módulo con ID {permissionDetail.ModuleId} no encontrado");

                // Validar la Función si el ID está presente
                if (permissionDetail.FunctionId.HasValue)
                {
                    var function = await _functionRepository.GetFunctionByIdAsync(permissionDetail.FunctionId.Value);
                    if (function == null)
                        throw new BusinessException($"Función con ID {permissionDetail.FunctionId} no encontrada");
                }

                // Validar cada Tipo de Permiso dentro de la lista de objetos PermissionTypes
                foreach (var pTypeRequest in permissionDetail.PermissionTypes)
                {
                    // Accedemos a la propiedad .PermissionTypeId del objeto del DTO
                    var permissionType = await _permissionTypeRepository.GetByIdAsync(pTypeRequest.PermissionTypeId);
                    if (permissionType == null)
                        throw new BusinessException($"Tipo de permiso con ID {pTypeRequest.PermissionTypeId} no encontrado");
                }
            }

            // Si todo es válido, procedemos a la persistencia en el repositorio
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
