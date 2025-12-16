using System.ComponentModel.DataAnnotations;

namespace Rrhh_backend.Core.Entities
{
    public class Role: BaseEntity
    {        
        public string RoleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public virtual ICollection<User> Users { get; set; } = new List<User>();        
        
    }
}
