using System.ComponentModel.DataAnnotations;

namespace Rrhh_backend.Core.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength (100)]
        public string Email { get; set; }    = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Role { get; set; }     = string.Empty;       
    }
}
