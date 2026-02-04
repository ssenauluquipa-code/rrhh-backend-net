namespace Rrhh_backend.Presentation.DTOs.Responses.Departament
{
    public class DepartamentResponse
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int CompanyId { get; set; }
        public bool IsActive { get; set; }
    }
}
