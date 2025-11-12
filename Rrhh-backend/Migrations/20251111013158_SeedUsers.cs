using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rrhh_backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insertar usuarios iniciales
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Username", "Email", "PasswordHash", "Role", "IsActive" },
                values: new object[,]
                {
                    { "admin", "admin@empresa.com", "123456", "ADMIN", true },
                    { "hr_user", "hr@empresa.com", "123456", "HR", true },
                    { "juan", "juan@empresa.com", "123456", "EMPLOYEE", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eliminar los usuarios insertados
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "admin@empresa.com");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "hr@empresa.com");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "juan@empresa.com");
        }
    }
}
