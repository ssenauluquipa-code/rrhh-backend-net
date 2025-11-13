namespace Rrhh_backend.Presentation.DTOs.Requests.Roles
{
    public class UpdateRolesRequest
    {
        public string RoleName { get; set; }
        public string Description { get; set; }

        public bool IsActivate { get; set; }
    }
}
