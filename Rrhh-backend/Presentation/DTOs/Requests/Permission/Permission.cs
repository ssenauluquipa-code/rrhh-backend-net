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

        // CAMBIO AQUÍ: Ya no es una lista de ints, es una lista de objetos de estatus
        public List<PermissionTypeStatusRequest> PermissionTypes { get; set; } = new();
    }

    // Nueva clase para capturar el estado de cada check
    public class PermissionTypeStatusRequest
    {
        public int PermissionTypeId { get; set; }
        public bool IsActive { get; set; }
    }
}
