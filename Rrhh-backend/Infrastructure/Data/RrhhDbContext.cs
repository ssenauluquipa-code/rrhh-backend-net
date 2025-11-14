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
        public DbSet<Role> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Password).IsRequired().IsUnicode(false);                
                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RoleName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(100);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
