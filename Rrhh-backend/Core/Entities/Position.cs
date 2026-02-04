using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rrhh_backend.Core.Entities
{
    [Table("Positions")]
    public class Position
    {
        [Key]
        public int PositionId { get; set; }
        public int DepartmentId { get; set; }
        public int CompanyId { get; set; }
        public string PositionName { get; set; }
        public decimal BaseSalary { get; set; }
        public bool IsActive { get; set; } = true;

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
    }
}
