using System.ComponentModel.DataAnnotations;

namespace Rrhh_backend.Core.Entities
{
    public class Role: BaseEntity
    {
        [Required]
        public string RoleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public virtual ICollection<User>? Users { get; set; }
        public virtual ICollection<RolePermission>? RolePermissions { get; set; }
        
    }
}
