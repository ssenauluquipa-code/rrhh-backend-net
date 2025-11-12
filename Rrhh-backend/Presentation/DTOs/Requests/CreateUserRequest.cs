namespace Rrhh_backend.Presentation.DTOs.Requests
{
    public class CreateUserRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "EMPLOYEE"; // ADMIN, HR, EMPLOYEE
    }
}
