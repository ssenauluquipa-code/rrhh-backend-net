using System.ComponentModel.DataAnnotations;

namespace Rrhh_backend.Core.Entities
{
    public class Role: BaseEntity
    {
        [Required]
        public string RoleName { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public virtual ICollection<User>? Users { get; set; }
        
    }
}
