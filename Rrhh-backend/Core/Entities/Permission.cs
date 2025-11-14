
using Rrhh_backend.Core.Entities.ModuleEspecial;

namespace Rrhh_backend.Core.Entities
{
    public class Permission : BaseEntity
    {
        public string PermissionsName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ModuleId { get; set; }
        //public bool IsActive { get; set; } = true;

        //relacion
        public virtual Module? Module { get; set; }
        public virtual ICollection<RolePermission>? RolePermissions { get; set; }

    }
}
