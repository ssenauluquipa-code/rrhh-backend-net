namespace Rrhh_backend.Presentation.DTOs.Requests.Position
{
    public class PositionRequest
    {
        public string PositionName { get; set; }
        public int DepartmentId { get; set; }
        public int CompanyId { get; set; }
        public decimal BaseSalary { get; set; }
    }
}
