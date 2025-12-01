using System.ComponentModel.DataAnnotations;

namespace Rrhh_backend.Presentation.DTOs.Requests.Users
{
    public class CreateUserRequest
    {
        [Required] public string UserName { get; set; } = string.Empty;
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
        [Required] public Guid RoleId { get; set; }
        public Guid? EmployeeId { get; set; }
    }
}
