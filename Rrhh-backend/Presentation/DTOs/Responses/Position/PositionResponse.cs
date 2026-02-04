namespace Rrhh_backend.Presentation.DTOs.Responses.Position
{
    public class PositionResponse
    {
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } // Opcional para mostrar en tablas
        public int CompanyId { get; set; }
        public decimal BaseSalary { get; set; }
        public bool IsActive { get; set; }
    }
}
