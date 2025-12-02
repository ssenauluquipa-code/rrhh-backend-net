using BC =  BCrypt.Net.BCrypt;
namespace Rrhh_backend.Security
{
    public class PasswordHasher
    {
        public  string HashPassword(string password)
        {
            return BC.HashPassword(password);
        }

        public  bool VerifyPassword(string password, string hash)
        {
            return BC.Verify(password, hash);
        }
    }
}
