namespace Rrhh_backend.Presentation.DTOs.Requests.Permission
{
    public class PermissionAssignmentRequest
    {
        public int RoleId { get; set; }
        public List<PermissionAssignmentDetail> Permissions { get; set; } = new();
    }
    public class PermissionAssignmentDetail
    {
        public int ModuleId { get; set; }
        public int? FunctionId { get; set; }
        public List<int> PermissionTypeIds { get; set; } = new();
    }
}
