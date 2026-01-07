namespace Rrhh_backend.Presentation.DTOs.Responses
{
    public class PermissionResponse
    {
        public Dictionary<string, List<FunctionPermission>> Permissions { get; set; } = new();
        // Ejemplo: { "users": ["CREATE", "READ"], "clients": ["READ"] }
    }
    public class FunctionPermission
    {
        public int FunctionId { get; set; } // ✅ ID numérico
        public string FunctionName { get; set; } = string.Empty;
        public List<int> Permissions { get; set; } = new();
    }
}
