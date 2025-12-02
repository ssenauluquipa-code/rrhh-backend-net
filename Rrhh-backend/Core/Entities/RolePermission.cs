
namespace Rrhh_backend.Core.Entities
{
    public class RolePermission : BaseEntity
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        //Relaciones
        public virtual Role? Role { get; set; } = null;
        public virtual Permission? Permission { get; set; } = null;
    }
}
