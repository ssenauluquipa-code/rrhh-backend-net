namespace Rrhh_backend.Presentation.DTOs.Responses
{
    public class RolePermissionResponse
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public string PermissionName { get; set; } = string.Empty;
        public bool IsGranted { get; set; }
        public DateTime GrantedAt { get; set; }
    }
}
