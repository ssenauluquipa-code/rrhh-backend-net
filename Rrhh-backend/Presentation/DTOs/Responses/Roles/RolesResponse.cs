namespace Rrhh_backend.Presentation.DTOs.Responses.Roles
{
    public class RolesResponse
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActivate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
