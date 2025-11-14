namespace Rrhh_backend.Presentation.DTOs.Requests.Users
{
    public class UpdateUserRequest
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public int? RoleId { get; set; } // ADMIN, HR, EMPLOYEE
    }
}
