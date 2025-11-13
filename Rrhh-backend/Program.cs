using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
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

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        //converti de enums a string en lugar de numeros
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//// 🧾 Configurar Swagger con definición de seguridad JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API del Sistema de RRHH",
        Version = "v1",
        Description = "Documentación de la API del sistema de Recursos Humanos"
    });

    // 🔐 Definir el esquema de seguridad: Bearer Token (JWT)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese 'Bearer' [espacio] y luego su token en el campo de texto a continuación.\n\nEjemplo: Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.xxxxx"
    });

    // 🔐 Hacer que todos los endpoints requieran el token (opcional global)
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
});

//Entity Framawork
var connectionString = builder.Configuration.GetConnectionString("AivenDbConnection");
builder.Services.AddDbContext<RrhhDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Jwt Setting
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(JwtSettings.SectionName)
    );
//JWT UTIL
builder.Services.AddScoped<JwtUtil>();
// Repositories and Services
builder.Services.AddScoped<IAuthService, AuthService>();
/******* USER *******/
builder.Services.AddScoped<IUserRepository, UserRepositoryEf>();
builder.Services.AddScoped<IUserService, UserService>();
/*** roles ***/
builder.Services.AddScoped<IRolesRepository, RolesRepositoryEF>();
builder.Services.AddScoped<IRolesService,  RolesService>();

//builder.Services.AddScoped<IEmployeeService, EmployeeService>();

//JWT AUTHENTICATION
var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName);
var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

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
    options.AddPolicy("AllowAngularLocalhost",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularLocalhost");
//Middleware de autenticacion
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
