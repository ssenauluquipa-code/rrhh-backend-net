
using Rrhh_backend.Core.Entities.ModuleEspecial;

namespace Rrhh_backend.Core.Entities
{
    public class Permission : BaseEntity
    {        
        public string PermissionsName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;

        //relacion
        //public virtual Module? Module { get; set; }
        public virtual ICollection<RolePermission>? RolePermissions { get; set; } = new List<RolePermission>();

    }
}
