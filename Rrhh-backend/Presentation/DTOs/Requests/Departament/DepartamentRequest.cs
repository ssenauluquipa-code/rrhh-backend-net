using System.ComponentModel.DataAnnotations;

namespace Rrhh_backend.Presentation.DTOs.Requests.Departament
{
    public class DepartamentRequest
    {
        [Required(ErrorMessage = "El nombre del departamento es obligatorio")]
        [StringLength(150)]
        public string DepartmentName { get; set; }

        [Required]
        public int CompanyId { get; set; }
    }
}
