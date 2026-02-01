namespace Rrhh_backend.Core.Entities
{
    public class Countries
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public string IsoCode { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string IdentityLabel { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
