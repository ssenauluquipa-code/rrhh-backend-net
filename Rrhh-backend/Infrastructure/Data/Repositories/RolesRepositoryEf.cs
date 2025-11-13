using Microsoft.EntityFrameworkCore;
using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Interfaces.Repositories;

namespace Rrhh_backend.Infrastructure.Data.Repositories
{
    public class RolesRepositoryEF : IRolesRepository
    {
        private readonly RrhhDbContext _context;

        public RolesRepositoryEF(RrhhDbContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetRolesAll()
        {
            return await _context.Roles.Where(r => r.IsActive).ToListAsync();
        }
        public async Task<Role?> GetRolesById(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
        }
        public async Task<Role> CreatedRoles(Role roles)
        {
            _context.Roles.Add(roles);
            await _context.SaveChangesAsync();
            return roles;
        }
        public async Task<Role?> UpdatedRoles(int id, Role roles)
        {
            var existing = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
            if(existing != null)
            {
                existing.RoleName = roles.RoleName;
                existing.Descripcion = roles.Descripcion;
                existing.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            return existing;
        }
        public async Task<bool> Deleted(int id)
        {
            var deleted = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
            if(deleted != null)
            {
                deleted.IsActive = false;
                await _context.SaveChangesAsync();
                return false;
            }
            return false;
        }
        public async Task<bool> IsActivateRoles(int id)
        {
            var activated = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if(activated != null && !activated.IsActive)
            {
                activated.IsActive = true;
                await _context.SaveChangesAsync();
                return false;
            }
            return true;
        }
    }
}
