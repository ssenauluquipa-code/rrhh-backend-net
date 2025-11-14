namespace Rrhh_backend.Presentation.DTOs.Responses
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public int Role { get; set; }
    }
}
