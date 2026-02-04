using Microsoft.EntityFrameworkCore;
using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Interfaces.Repositories;

namespace Rrhh_backend.Infrastructure.Data.Repositories
{
    public class DepartmentRepositoryEf : IDepartmentRepository
    {
        private readonly NebulaDbContext _context;
        public DepartmentRepositoryEf(NebulaDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
        }

        public async Task<IEnumerable<Department>> GetAll()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<IEnumerable<Department>> GetAllByCompanyAsync(int companyId)
        {
            return await _context.Departments.Where(d => d.CompanyId == companyId).ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(int id, int companyId)
        {
           return await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id && d.CompanyId == companyId);
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Department department)
        {
            _context.Departments.Update(department);
            await Task.CompletedTask;
        }
    }
}
