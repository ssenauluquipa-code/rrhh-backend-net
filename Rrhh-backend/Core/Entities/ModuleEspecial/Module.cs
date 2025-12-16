namespace Rrhh_backend.Core.Entities.ModuleEspecial
{
    public class Module
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string ModuleKey { get; set; } = string.Empty;
        //public bool IsActive { get; set; }
        public string Category { get; set; } = "main";
        public string? Icono { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        //relacion: un modulo puede estar asociado a muchos CompanyModule
        //public virtual ICollection<CompanyModule>? CompanyModules { get; set; }
        public virtual ICollection<Permission>? Permissions { get; set; }
    }
}
