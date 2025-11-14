namespace Rrhh_backend.Core.Entities.ModuleEspecial
{
    public class Company : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string LicenseKey { get; set; } = string.Empty;
        public DateTime SubscriptionStart { get; set; }
        public DateTime SubscriptionEnd { get; set; }
        //public bool IsActive { get; set; } = true;

        //Relañcion: una empresa puede teer muchjos modulos activos
        public virtual ICollection<CompanyModule>? CompanyModules { get; set; }
    }
}
