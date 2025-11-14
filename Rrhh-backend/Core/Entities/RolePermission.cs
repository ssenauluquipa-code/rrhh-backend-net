
namespace Rrhh_backend.Core.Entities
{
    public class RolePermission : BaseEntity
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public bool IsGranted { get; set; }
        public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
        public int? GrantedBy { get; set; }

        //Relaciones
        public virtual Role? Role { get; set; }
        public virtual Permission? Permission { get; set; }
    }
}
