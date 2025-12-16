using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rrhh_backend.Core.Interfaces.Repositories;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Infrastructure.Data;
using Rrhh_backend.Infrastructure.Data.Repositories;
using Rrhh_backend.Infrastructure.Services;
using Rrhh_backend.Security;
using Rrhh_backend.Shared.Security;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Configuración de controladores con serialización camelCase
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        //converti de enums a string en lugar de numeros
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


// Configuración de Swagger (solo en desarrollo)
builder.Services.AddEndpointsApiExplorer();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen(c =>
    {
        // 1. Información general de la API (ajusta según tu proyecto)
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "API de NEBULA",
            Description = "Sistema de gestión empresarial modular",
            TermsOfService = new Uri("https://nebula.app/terms"),
            Contact = new OpenApiContact
            {
                Name = "Soporte NEBULA",
                Url = new Uri("https://nebula.app/contact")
            },
            License = new OpenApiLicense
            {
                Name = "MIT",
                Url = new Uri("https://opensource.org/licenses/MIT")
            }
        });
        // 2. Configuración de autenticación JWT en Swagger UI
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Ingrese 'Bearer' [espacio] y su token JWT aquí."
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });

        // 3. Opcional: Incluir comentarios XML en la documentación
        // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        // c.IncludeXmlComments(xmlPath); // Descomenta si tienes XML activado
    });
}else
{
    // En producción, no incluyas Swagger
    builder.Services.AddSwaggerGen(); // Aunque no se use, es buena práctica incluirlo si el paquete está instalado
}
//// 🧾 Configurar Swagger con definición de seguridad JWT
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "API del Sistema de RRHH",
//        Version = "v1",
//        Description = "Documentación de la API del sistema de Recursos Humanos"
//    });

//    // 🔐 Definir el esquema de seguridad: Bearer Token (JWT)
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        Type = SecuritySchemeType.Http,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        In = ParameterLocation.Header,
//        Description = "Ingrese 'Bearer' [espacio] y luego su token en el campo de texto a continuación.\n\nEjemplo: Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.xxxxx"
//    });

//    // 🔐 Hacer que todos los endpoints requieran el token (opcional global)
//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//    {
//        new OpenApiSecurityScheme
//        {
//            Reference = new OpenApiReference
//            {
//                Type = ReferenceType.SecurityScheme,
//                Id = "Bearer"
//            }
//        },
//        Array.Empty<string>()
//    }
//    });
//});

// Entity Framework Core (MySQL con Pomelo)
// 4. Obtener cadena de conexión desde variables de entorno o appsettings
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<NebulaDbContext>(options =>
 {
        // Usar una versión específica de MySQL (la de Aiven es 8.0.35)
        var serverVersion = ServerVersion.Parse("8.0.35-mysql");
        options.UseMySql(connectionString, serverVersion);
        // No llames a EnsureCreated ni migraciones automáticas aquí si no es intencional
 });

//builder.Services.AddDbContext<NebulaDbContext>(options =>
//{
//    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
//});

// JWT Settings (desde variables de entorno o appsettings)
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt") // Asegúrate que coincida con la variable Jwt__Secret en Render
    );
// JWT Util (si lo usas)
//builder.Services.AddScoped<JwtUtil>();

// Repositories and Services Auth
builder.Services.AddScoped<IAuthService, AuthService>();
/******* USER *******/
builder.Services.AddScoped<IUserRepository, UserRepositoryEf>();
builder.Services.AddScoped<IUserService, UserService>();
/*** roles ***/
builder.Services.AddScoped<IRolesRepository, RolesRepositoryEF>();
builder.Services.AddScoped<IRolesService,  RolesService>();
/*** empleados**/
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepositoryEf>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
/*** Pemrision**/
builder.Services.AddScoped<IPermissionRepository, PermissionRepositoryEf>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IPermissionAssignmentService, PermissionAssignmentService>();
/**module**/
builder.Services.AddScoped<IModuleRepository, ModuleRepositoryEf>();
/***permision type**/
builder.Services.AddScoped<IPermissionTypeRepository, PermissionTypeRepositoryEf>();


//JWT AUTHENTICATION
//var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName);
//var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);
var secret = builder.Configuration["Jwt:Secret"];
if (string.IsNullOrWhiteSpace(secret))
{    
        throw new InvalidOperationException("JWT Secret Key no está configurado en variables de entorno (Jwt__Secret).");
}
var key = Encoding.ASCII.GetBytes(secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
});
//autorizacion basica
builder.Services.AddAuthorization();

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "https://mantis-weld.vercel.app")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// En producción, no se ejecuta UseSwagger ni UseSwaggerUI

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
