namespace Rrhh_backend.Presentation.DTOs.Responses.Company
{
    public class CompanyResponse
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string TaxId { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
