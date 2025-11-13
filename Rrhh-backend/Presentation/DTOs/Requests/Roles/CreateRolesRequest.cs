namespace Rrhh_backend.Presentation.DTOs.Requests.Roles
{
    public class CreateRolesRequest
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActivate { get; set; }
    }
}
