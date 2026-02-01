using Microsoft.EntityFrameworkCore;
using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Interfaces.Repositories;

namespace Rrhh_backend.Infrastructure.Data.Repositories
{
    public class CountryRepositoryEf :ICountryRepository
    {
        private readonly NebulaDbContext _context;
        public CountryRepositoryEf(NebulaDbContext context)
        {
            _context = context;
        }
       
        public async Task<IEnumerable<Countries>> GetCountries()
        {
           return await _context.Countries.ToListAsync();
        }
        public async Task<Countries?> GetByIdAsync(int id)
        {
            return await _context.Countries.FindAsync(id);
        }
        public async Task<Countries?> GetByNameCountries(string countriesName)
        {
            return await _context.Countries.FirstOrDefaultAsync(c => c.CountryName == countriesName);
        }
        public async Task<Countries> CreateByAsync(Countries countries)
        {
            _context.Countries.Add(countries);
            await _context.SaveChangesAsync();
            return countries;
        }

        public async Task<Countries> UpdateByAsync(Countries countries)
        {
            _context.Countries.Update(countries);
            await _context.SaveChangesAsync();
            return countries;
        }

        public async Task<bool> DeleteByAsync(int id)
        {
            var deleted = await _context.Countries.FirstAsync(r => r.CountryId == id && r.IsActive);
            if (deleted == null) return false;
            
             deleted.IsActive = false;
             return await _context.SaveChangesAsync()>0;                           
        }

        public async Task<bool> IsActivateCountry(int id)
        {
            var activated = await _context.Countries.FirstAsync(r => r.CountryId == id);
            if (activated == null) return false;
            activated.IsActive = true;
            return await _context.SaveChangesAsync() > 0;
            
        }
    }
}
