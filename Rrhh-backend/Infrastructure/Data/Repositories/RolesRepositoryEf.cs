using Microsoft.EntityFrameworkCore;
using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Interfaces.Repositories;

namespace Rrhh_backend.Infrastructure.Data.Repositories
{
    public class RolesRepositoryEF : IRolesRepository
    {
        private readonly NebulaDbContext _context;

        public RolesRepositoryEF(NebulaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetRolesAllAsync()
        {
            return await _context.Roles.ToListAsync();
        }
        public async Task<Role?> GetRolesByIdAsync(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<Role> CreatedRoles(Role roles)
        {
            _context.Roles.Add(roles);
            await _context.SaveChangesAsync();
            return roles;
        }
        public async Task<Role?> UpdatedRoles(Role roles)
        {
            _context.Roles.Update(roles);
            await _context.SaveChangesAsync();
            return roles;
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
            var activated = await _context.Roles.FindAsync(id);
            if (activated == null) return false;

            activated.IsActive = true;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == name);
        }
    }
}
