namespace Rrhh_backend.Presentation.DTOs.Requests
{
    public class UpdateUserRequest
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; } // ADMIN, HR, EMPLOYEE
    }
}
