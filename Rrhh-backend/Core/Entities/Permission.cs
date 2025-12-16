
using Rrhh_backend.Core.Entities.ModuleEspecial;

namespace Rrhh_backend.Core.Entities
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
        public int PermissionTypeId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        //relaciones (opcional para consultas con Include)
        //public virtual Module? Module { get; set; }
        //public virtual ICollection<RolePermission>? RolePermissions { get; set; } = new List<RolePermission>();
        public virtual Role? Role { get; set; }
        public virtual Module? Module { get; set; }
        public virtual PermissionType? PermissionType { get; set; }
    }
}
