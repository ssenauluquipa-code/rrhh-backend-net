using System.ComponentModel.DataAnnotations;

namespace Rrhh_backend.Presentation.DTOs.Requests.Users
{
    public class UpdateUserRequest
    {
        [Required] public string UserName { get; set; } = string.Empty;
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        [Required] public int RoleId { get; set; }
        public int? EmployeeId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
