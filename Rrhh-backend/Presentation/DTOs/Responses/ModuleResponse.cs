namespace Rrhh_backend.Presentation.DTOs.Responses
{
    public class ModuleResponse
    {
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public List<string> Permissions { get; set; } = new List<string>();
    }
}
