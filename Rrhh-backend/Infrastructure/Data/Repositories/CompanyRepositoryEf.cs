using Microsoft.EntityFrameworkCore;
using Rrhh_backend.Core.Entities.ModuleEspecial;
using Rrhh_backend.Core.Interfaces.Repositories;

namespace Rrhh_backend.Infrastructure.Data.Repositories
{
    public class CompanyRepositoryEf : ICompanyRepository
    {
        private readonly NebulaDbContext _context;
        public CompanyRepositoryEf(NebulaDbContext context)
        {
           _context = context;
        }
        public async Task<IEnumerable<Companies>> GetAllCompaniesAsync()
        {
            return await _context.Companies.Include(c => c.Country)// JOIN con Countries
                .AsNoTracking()// Optimización para lectura pura
                .ToListAsync();
        }
        public async Task<Companies?> GetCompanyByIdAsync(int id)
        {
            return await _context.Companies
                .Include(c => c.Country)// JOIN con Countries
                .FirstOrDefaultAsync(c => c.CompanyId == id);
        }
        public async Task<Companies> CreateCompanyAsync(Companies company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            await _context.Entry(company)
                .Reference(c => c.Country)
                .LoadAsync();
            return company;
        }
        public async Task<Companies> UpdateCompanyAsync(Companies company)
        {
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
            return company;
        }
        public async Task<bool> DeleteCompanyAsync(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if(company == null) return false;

            company.IsActive = false;
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> ChangeStatus(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null) return false;
            company.IsActive = !company.IsActive;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
