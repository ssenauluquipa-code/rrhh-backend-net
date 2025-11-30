using Microsoft.EntityFrameworkCore;
using Rrhh_backend.Core.Entities.ModuleEspecial;
using Rrhh_backend.Core.Interfaces.Repositories;

namespace Rrhh_backend.Infrastructure.Data.Repositories
{
    public class CompanyRepositoryEf : ICompanyRepository
    {
        private readonly RrhhDbContext _context;

        public CompanyRepositoryEf(RrhhDbContext context)
        {
            _context = context;
        }

        public async Task<Company> CreateAsync(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<Company?> GetByNameAsync(string name)
        {
            return await _context.Companies.FirstOrDefaultAsync(
                c => c.Name == name);
        }
    }
}
