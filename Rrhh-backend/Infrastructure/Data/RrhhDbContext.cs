using Microsoft.EntityFrameworkCore;
using Rrhh_backend.Core.Entities;

namespace Rrhh_backend.Infrastructure.Data
{
    public class RrhhDbContext : DbContext
    {
        public RrhhDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Password).IsRequired().IsUnicode(false);
                entity.Property(e => e.Role).HasMaxLength(50);
                entity.HasIndex(e => e.Email).IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
