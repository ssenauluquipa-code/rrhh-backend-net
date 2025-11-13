using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Interfaces.Repositories;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Requests.Roles;
using Rrhh_backend.Presentation.DTOs.Responses;

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
            var roles = await _rolesRepository.GetRolesAll();
            return roles.Select(MapToResponse).ToList();
        }
        public async Task<RolesResponse?> GetRolesByIdAsync(int id)
        {
            var roles = await _rolesRepository.GetRolesById(id);
            return roles != null ? MapToResponse(roles) : null;
        }
        public async Task<RolesResponse> CreatedRolesAsync(CreateRolesRequest request)
        {
            var roles = new Role
            {
                RoleName = request.RoleName,
                Descripcion = request.Description,

            };
            var created = await _rolesRepository.CreatedRoles(roles);
            return MapToResponse(created);
        }
        public async Task<RolesResponse?> UpdateRolesAsync(int id ,UpdateRolesRequest request)
        {
            var roles = await _rolesRepository.GetRolesById(id);
            if (roles == null) return null;

            if (!string.IsNullOrEmpty(request.RoleName))
                roles.RoleName = request.RoleName;
            if (!string.IsNullOrEmpty(request.Description))
                roles.Descripcion = request.Description;

            roles.UpdatedAt = DateTime.UtcNow;
            var update = await _rolesRepository.UpdatedRoles(id, roles);
            return update != null ? MapToResponse(update) : null;
        }
        public async Task<bool> DeletedAsync(int id)
        {
            return await _rolesRepository.Deleted(id);
        }
        public async Task<bool> ActivatedAsync(int id)
        {
            return await _rolesRepository.IsActivateRoles(id);
        }

        private static RolesResponse MapToResponse(Role roles)
        {
            return new RolesResponse
            {
                Id = roles.Id,
                RoleName = roles.RoleName,
                Description = roles.Descripcion,
                IsActivate = roles.IsActive,
                CreatedAt = roles.CreatedAt,
                UpdatedAt = roles.UpdatedAt
            };
        }
    }
}
