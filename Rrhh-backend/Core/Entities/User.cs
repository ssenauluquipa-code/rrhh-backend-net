using System.ComponentModel.DataAnnotations;

namespace Rrhh_backend.Core.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; }    = string.Empty;
        
        public string Password { get; set; } = string.Empty;        
        public int RoleId { get; set; }    
        
        public virtual Role? Role { get; set; }
    }
}
