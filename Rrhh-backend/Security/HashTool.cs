namespace Rrhh_backend.Security
{
    public class HashTool
    {
        public static void GenerateHash()
        {
            var password = "password123";
            var hasher = new PasswordHasher();
            var hash = hasher.HashPassword(password);
            var isValid = hasher.VerifyPassword(password, hash);

            Console.WriteLine("=== HERRAMIENTA DE HASH ===");
            Console.WriteLine($"Contraseña: {password}");
            Console.WriteLine($"Hash generado: {hash}");
            Console.WriteLine($"Verificación: {isValid}");
            Console.WriteLine("Copia el hash y actualízalo en tu base de datos.");
            Console.WriteLine("============================");
        }
    }
}
