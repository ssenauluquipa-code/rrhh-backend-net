namespace Rrhh_backend.Presentation.DTOs.Requests.Users
{
    public class CreateUserRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int RoleId { get; set; } // ADMIN, HR, EMPLOYEE
    }
}
