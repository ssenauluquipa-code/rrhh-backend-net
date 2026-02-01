namespace Rrhh_backend.Presentation.DTOs.Responses.Countries
{
    public class CountryResponse
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public string IsoCode { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string IdentityLabel { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
