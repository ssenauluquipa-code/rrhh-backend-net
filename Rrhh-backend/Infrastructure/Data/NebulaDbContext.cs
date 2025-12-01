using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Entities.ModuleEspecial;

namespace Rrhh_backend.Infrastructure.Data
{
    public class NebulaDbContext : DbContext
    {
        public NebulaDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }               
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);
                entity.HasOne(u => u.Employee).WithMany(e => e.Users).HasForeignKey(u => u.EmployeeId);
            });
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(rp => rp.Role).WithMany(r => r.RolePermissions).HasForeignKey(rp => rp.RoleId);
                entity.HasOne(rp => rp.Permission).WithMany(p => p.RolePermissions).HasForeignKey(rp => rp.PermissionId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
