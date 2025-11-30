using System.ComponentModel.DataAnnotations;

namespace Rrhh_backend.Presentation.DTOs.Requests.Tenants
{
    public class CreateTenantRequest
    {
        [Required(ErrorMessage = "El nombre de la empresa es obligatorio")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "formato de email invalido")]
        public string AdminEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatorio")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string AdminPassword { get; set; } = string.Empty;
    }
}
