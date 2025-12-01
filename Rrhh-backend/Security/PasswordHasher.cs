using BC =  BCrypt.Net.BCrypt;
namespace Rrhh_backend.Security
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            return BC.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hash)
        {
            return BC.Verify(password, hash);
        }
    }
}
