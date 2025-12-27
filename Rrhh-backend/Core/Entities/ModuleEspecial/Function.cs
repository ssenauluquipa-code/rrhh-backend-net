using System.ComponentModel.DataAnnotations;

namespace Rrhh_backend.Core.Entities.ModuleEspecial
{
    public class Function
    {
        [Key]
        public int FunctionId { get; set; }

        [Required]
        [MaxLength(255)]
        public string FunctionName { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

        public int ModuleId { get; set; }

        // Relación con módulo
        public Module Module { get; set; } = null!;

        // Relación con permisos
        public List<Permission> Permissions { get; set; } = new();
    }
}
