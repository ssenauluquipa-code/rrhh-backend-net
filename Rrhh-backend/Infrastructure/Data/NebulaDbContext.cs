using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Entities.ModuleEspecial;
using System.Reflection.Emit;

namespace Rrhh_backend.Infrastructure.Data
{
    public class NebulaDbContext : DbContext
    {
        public NebulaDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; } = null!;            
        /*Module Access*/
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Module> Modules { get; set; } = null!;
        public DbSet<PermissionType> PermissionTypes { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;
        
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
            /*access*/
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => e.PermissionId);
                //relaciones
                entity.HasOne(p => p.Role).WithMany().HasForeignKey(p => p.RoleId);

                entity.HasOne(p => p.Module).WithMany().HasForeignKey(p => p.ModuleId);

                entity.HasOne(p => p.PermissionType).WithMany().HasForeignKey(p => p.PermissionTypeId);

                //indice unico: un rol no puede tener el mismo permiso duplicado
                entity.HasIndex(p => new { p.RoleId, p.ModuleId, p.PermissionTypeId }).IsUnique();
            });
            /// =========== CONFIGURACION DE MODULE=================//
            modelBuilder.Entity<Module>(entity =>
            {
                entity.HasKey(m => m.ModuleId);
            });
            ///
            modelBuilder.Entity<PermissionType>(entity =>
            {
                entity.HasKey(t => t.PermissionTypeId);
                entity.HasIndex(t => t.Code).IsUnique();
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
