namespace Rrhh_backend.Core.Entities.ModuleEspecial
{
    public class Module : BaseEntity
    {
        public string ModuleName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        //public bool IsActive { get; set; }

        //relacion: un modulo puede estar asociado a muchos CompanyModule
        //public virtual ICollection<CompanyModule>? CompanyModules { get; set; }
        public virtual ICollection<Permission>? Permissions { get; set; }
    }
}
