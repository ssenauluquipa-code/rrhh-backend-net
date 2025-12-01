using System.ComponentModel.DataAnnotations;

namespace Rrhh_backend.Presentation.DTOs.Requests.Employee
{
    public class CreateEmployeeRequest
    {
        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;
        [Required] public string Position { get; set; } = string.Empty;
        [Required] public string Department { get; set; } = string.Empty;
        [EmailAddress] public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
