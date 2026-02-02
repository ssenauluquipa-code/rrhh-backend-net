using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rrhh_backend.Core.Entities.ModuleEspecial
{
    public class Companies
    {
        public int CompanyId { get; set; }

        [Required, MaxLength(150)]
        public string CompanyName { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string TaxId { get; set; } = string.Empty;

        public string? Address { get; set; } = string.Empty;

        // Relación con Countries
        public int CountryId { get; set; }

        [ForeignKey("CountryId")]
        public virtual Countries? Country { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
