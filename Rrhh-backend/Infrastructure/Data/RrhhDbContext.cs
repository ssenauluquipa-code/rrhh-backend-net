using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Entities.ModuleEspecial;

namespace Rrhh_backend.Infrastructure.Data
{
    public class RrhhDbContext : DbContext
    {
        public RrhhDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
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
            //Conguracionb de Module
            modelBuilder.Entity<Module>(entity =>
            {
                entity.ToTable("Modules");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ModuleName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.DisplayName).HasMaxLength(100);
                entity.HasIndex(e => e.ModuleName).IsUnique();
            });
            // Configuración de Permission
            modelBuilder.Entity<Permission>(static entity =>
            {
                entity.ToTable("Permissions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PermissionsName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.HasIndex(e => e.PermissionsName).IsUnique();
                
                // Relación: Permission → Module
                entity.HasOne(p => p.Module)
                      .WithMany(m => m.Permissions)
                      .HasForeignKey(p => p.ModuleId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            // Configuración de RolePermission (muchos a muchos)
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(rp => rp.Id);

                entity.HasOne(rp => rp.Role)
                      .WithMany(r => r.RolePermissions)
                      .HasForeignKey(rp => rp.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(rp => rp.Permission)
                      .WithMany(p => p.RolePermissions)
                      .HasForeignKey(rp => rp.PermissionId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(rp => new { rp.RoleId, rp.PermissionId }).IsUnique();
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
