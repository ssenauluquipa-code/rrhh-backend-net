namespace Rrhh_backend.Presentation.DTOs.Responses
{
    public class PermissionAssignmentResponse
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public List<ModulePermissionResponse> Modules { get; set; } = new();
    }

    public class ModulePermissionResponse
    {
        public int ModuleId { get; set; }
        public string Name { get; set; } = null!;
        public string Key { get; set; } = null!;
        public Dictionary<string, List<ModulePermissionItem>> Categories { get; set; } = new();
    }

    public class ModulePermissionItem
    {
        public int ModuleId { get; set; }
        public string Name { get; set; } = null!;
        public string Key { get; set; } = null!;
        public Dictionary<string, bool> Permissions { get; set; } = new();
    }
}
