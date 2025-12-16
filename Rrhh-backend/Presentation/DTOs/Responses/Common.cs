namespace Rrhh_backend.Presentation.DTOs.Responses
{
    public class PermissionResponse
    {
        public Dictionary<string, List<string>> Permissions { get; set; } = new();
        // Ejemplo: { "users": ["CREATE", "READ"], "clients": ["READ"] }
    }
}
