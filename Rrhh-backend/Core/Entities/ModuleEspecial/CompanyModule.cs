namespace Rrhh_backend.Core.Entities.ModuleEspecial
{
    public class CompanyModule : BaseEntity
    {
        public int CompanyId { get; set; }
        public int ModuleId { get; set; }
        public bool IsEnabled { get; set; } = true;

        //Relaciones

    }
}
