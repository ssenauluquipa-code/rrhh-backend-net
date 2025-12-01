using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Exceptions;
using Rrhh_backend.Core.Interfaces.Repositories;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Requests.Roles;
using Rrhh_backend.Presentation.DTOs.Responses.Roles;

namespace Rrhh_backend.Infrastructure.Services
{
    public class RolesService : IRolesService
    {
        private readonly IRolesRepository _rolesRepository;

        public RolesService(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        public async Task<List<RolesResponse>> GetRolesAllAsync()
        {
            var roles = await _rolesRepository.GetRolesAllAsync();
            return roles.Select(MapToResponse).ToList();
        }
        public async Task<RolesResponse?> GetRolesByIdAsync(Guid id)
        {
            var roles = await _rolesRepository.GetRolesByIdAsync(id);
            //return roles != null ? MapToResponse(roles) : null;
            return new RolesResponse
            {
                Id = roles.Id,
                RoleName = roles.RoleName,
                Description = roles.Description,
                IsActivate = roles.IsActive
            };
        }
        public async Task<RolesResponse> CreatedRolesAsync(CreateRolesRequest request)
        {
            var existingRole = await _rolesRepository.GetByNameAsync(request.RoleName);
            if (existingRole != null)
                throw new BusinessException("El nombre del rol ya existe.");

            var role = new Role
            {
                RoleName = request.RoleName,
                Description = request.Description
            };

            await _rolesRepository.CreatedRoles(role);

            return new RolesResponse
            {
                Id = role.Id,
                RoleName = role.RoleName,
                Description = role.Description,
                IsActivate = role.IsActive
            };
        }
        public async Task<RolesResponse?> UpdateRolesAsync(Guid id ,UpdateRolesRequest request)
        {
            //var roles = await _rolesRepository.GetRolesByIdAsync(id);
            var role = await _rolesRepository.GetRolesByIdAsync(id);
            //if (roles == null) return null;
            if (role == null)
                throw new BusinessException("Rol no encontrado.");

            role.RoleName = request.RoleName;
            role.Description = request.Description;
            role.IsActive = request.IsActivate;

            //if (!string.IsNullOrEmpty(request.RoleName))
            //    roles.RoleName = request.RoleName;
            //if (!string.IsNullOrEmpty(request.Description))
            //    roles.Description = request.Description;

            //roles.UpdatedAt = DateTime.UtcNow;
            //var update = await _rolesRepository.UpdatedRoles(id, roles);
            await _rolesRepository.UpdatedRoles(role);
            //return update != null ? MapToResponse(update) : null;
            return new RolesResponse
            {
                Id = role.Id,
                RoleName = role.RoleName,
                Description = role.Description,
                IsActivate = role.IsActive
            };
        }
        public async Task<bool> DeletedAsync(Guid id)
        {
            return await _rolesRepository.Deleted(id);
        }
        public async Task<bool> ActivatedAsync(Guid id)
        {
            return await _rolesRepository.IsActivateRoles(id);
        }

        private static RolesResponse MapToResponse(Role roles)
        {
            return new RolesResponse
            {
                Id = roles.Id,
                RoleName = roles.RoleName,
                Description = roles.Description,
                IsActivate = roles.IsActive,
                CreatedAt = roles.CreatedAt,
                UpdatedAt = roles.UpdatedAt
            };
        }
    }
}
