using System.ComponentModel.DataAnnotations;

namespace Rrhh_backend.Presentation.DTOs.Requests.Roles
{
    public class UpdateRolesRequest
    {
        [Required, MinLength(3)]
        public string RoleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public bool IsActivate { get; set; }
    }
}
