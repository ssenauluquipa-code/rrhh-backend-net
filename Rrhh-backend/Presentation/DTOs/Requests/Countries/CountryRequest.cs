using System.ComponentModel.DataAnnotations;

namespace Rrhh_backend.Presentation.DTOs.Requests.Countries
{
    public class CountryRequest
    {
        [Required(ErrorMessage = "El nombre del país es obligatorio")]
        [StringLength(100)]
        public string CountryName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El código ISO es obligatorio")]
        [StringLength(3, MinimumLength = 2, ErrorMessage = "El código ISO debe tener entre 2 y 3 caracteres")]
        public string IsoCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "La moneda es obligatoria")]
        [StringLength(10)]
        public string Currency { get; set; } = string.Empty;

        [Required(ErrorMessage = "La etiqueta de identidad es obligatoria (ej: CI, NIT)")]
        [StringLength(20)]
        public string IdentityLabel { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
