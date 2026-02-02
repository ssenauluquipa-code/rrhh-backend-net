namespace Rrhh_backend.Presentation.DTOs.Requests.Company
{
    public class CompanyRequest
    {
        public string CompanyName { get; set; } = string.Empty;
        public string TaxId { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int CountryId { get; set; }
    }
}
