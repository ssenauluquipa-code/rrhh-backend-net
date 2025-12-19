using Rrhh_backend.Presentation.DTOs.Requests.Auth;
using Rrhh_backend.Presentation.DTOs.Responses.Auth;

namespace Rrhh_backend.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(LoginRequest request);
        Task<bool> LogoutAsync(string token);
        bool ValidateToken(string token);

        string RefreshToken(string expiredToken);
        bool IsTokenExpired(string token);

    }
}
