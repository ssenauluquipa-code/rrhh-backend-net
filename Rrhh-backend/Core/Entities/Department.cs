using Rrhh_backend.Core.Entities.ModuleEspecial;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rrhh_backend.Core.Entities
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        [StringLength(150)]
        public string DepartmentName { get; set; }

        public bool IsActive { get; set; } = true;

        // Relación con Company
        [ForeignKey("CompanyId")]
        public virtual Companies Company { get; set; }
    }
}
