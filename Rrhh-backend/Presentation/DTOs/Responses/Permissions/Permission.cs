namespace Rrhh_backend.Presentation.DTOs.Responses.Permissions
{
    public class PermissionAssignmentResponse
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public List<ModulePermissionAssignment> Modules { get; set; } = new();
    }

    public class ModulePermissionAssignment
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string ModuleKey { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public List<FunctionPermissionAssignment> Functions { get; set; } = new();
    }

    
    public class FunctionPermissionAssignment
    {
        public int FunctionId { get; set; }
        public string FunctionName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<PermissionTypeAssignment> Permissions { get; set; } = new();
    }
    public class PermissionTypeAssignment
    {
        public int PermissionTypeId { get; set; }
        public string PermissionTypeName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
