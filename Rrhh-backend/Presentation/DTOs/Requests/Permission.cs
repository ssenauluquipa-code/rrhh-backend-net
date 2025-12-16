namespace Rrhh_backend.Presentation.DTOs.Requests
{
    public class PermissionAssignmentRequest
    {
        public int RoleId { get; set; }
        public List<ModulePermissionDto> Modules { get; set; } = new();
    }
    public class ModulePermissionDto
    {
        public int ModuleId { get; set; }
        public Dictionary<string, bool> Permissions { get; set; } = new();
    }
}
