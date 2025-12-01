using Microsoft.EntityFrameworkCore;
using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Interfaces.Repositories;

namespace Rrhh_backend.Infrastructure.Data.Repositories
{
    public class EmployeeRepositoryEf : IEmployeeRepository
    {
        private readonly NebulaDbContext _context;

        public EmployeeRepositoryEf(NebulaDbContext context)
        {
            _context = context;
        }
        public async Task<List<Employee>> GetEmployeeAll()
        {
            return await _context.Employees.Where(e => e.IsActive).ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(Guid id)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee> CreateByAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateByAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return employee;
        }
    }
}
