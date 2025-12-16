namespace Rrhh_backend.Core.Entities
{
    public class PermissionType
    {
        public int PermissionTypeId { get; set; }
        public string PermissionTypeName { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int SortOrder { get; set; }
    }
}
