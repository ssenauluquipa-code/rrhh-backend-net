using System.ComponentModel.DataAnnotations;

namespace Rrhh_backend.Core.Entities
{
    public class User : BaseEntity
    {
        //public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; }    = string.Empty;        
        public string PasswordHash { get; set; } = string.Empty;        
        public Guid RoleId { get; set; }
        public Guid? EmployeeId { get; set; }
        public string RoleName { get; set; } = string.Empty;

        // Navigation
        public virtual Role Role { get; set; } = null!;
        public virtual Employee? Employee { get; set; }
    }
}
