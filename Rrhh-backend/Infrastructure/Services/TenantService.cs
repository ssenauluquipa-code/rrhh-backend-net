using Rrhh_backend.Core.Entities.ModuleEspecial;
using Rrhh_backend.Core.Interfaces.Repositories;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Requests.Tenants;
using Rrhh_backend.Presentation.DTOs.Responses.Tenants;

namespace Rrhh_backend.Infrastructure.Services
{
    public class TenantService : ITenantService
    {
        private readonly ICompanyRepository _companyRepo;
        private readonly string _aivenTemplate;
        private readonly string _allowedEmail;

        public TenantService(ICompanyRepository companyRepo, IConfiguration configuration)
        {
            _companyRepo = companyRepo;
            _aivenTemplate = Environment.GetEnvironmentVariable("AIVEN_CONNECTION_TEMPLATE")
                            ?? "Server=mysq.aivencloud.com;Port=12345;Database={0};Uid=user;Pwd=password;";
            _allowedEmail = "tu@email.com"; // ¡CAMBIA ESTO A TU EMAIL REAL!
        }

        public async Task<TenantResponse> CreateTenantAsync(CreateTenantRequest request)
        {
            if (request.AdminEmail != _allowedEmail)
                throw new UnauthorizedAccessException("No autorizado");

            var existing = await _companyRepo.GetByNameAsync(request.CompanyName);
            if (existing != null)
                throw new InvalidOperationException("El nombre de la empresa ya existe");

            // Generar tenantId seguro
            var tenantId = request.CompanyName.ToLower().Trim().Replace(" ", "-");

            // Aquí iría la lógica real para crear DB en Aiven
            // Para el MVP, simulamos la conexión
            var dbConnection = string.Format(_aivenTemplate, $"tenant_{tenantId}_db");

            var company = new Company
            {
                Name = request.CompanyName,
                DbConnectionString = dbConnection
            };

            await _companyRepo.CreateAsync(company);

            return new TenantResponse
            {
                TenantId = tenantId,
                CompanyName = request.CompanyName
            };
        }
    }
}
