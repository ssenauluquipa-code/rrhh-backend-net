using Microsoft.EntityFrameworkCore;
using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Interfaces.Repositories;

namespace Rrhh_backend.Infrastructure.Data.Repositories
{
    public class UserRepositoryEf : IUserRepository
    {
        private readonly NebulaDbContext _context;

        public UserRepositoryEf(NebulaDbContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.Include(u => u.Role)
                .Where(u => u.IsActive).ToListAsync();
        }
        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id 
            && u.IsActive);            
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.Include(u => u.Role)
                .FirstOrDefaultAsync(u =>
            u.Email.ToLower() == email.ToLower() && u.IsActive);
        }
        public async Task<User?> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User?> UpdateUser(User user)
        {
            //var existing = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
            //if (existing != null) {
            //    existing.UserName = user.UserName;
            //    existing.Email = user.Email;
            //    existing.Role = user.Role;
            //    existing.UpdatedAt = DateTime.UtcNow;
            //    await _context.SaveChangesAsync();
            //}
            //return existing;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<bool> DeleteUser(Guid id)
        {
            var deleted = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
            if(deleted != null)
            {
                deleted.IsActive = false;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> ActiveAsync(Guid id)
        {
            var users = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if(users != null && !users.IsActive)
            {
                users.IsActive = true;
                users.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public User? Authenticate(string email, string password)
        {
            throw new NotImplementedException();
        }

       
    }
}
