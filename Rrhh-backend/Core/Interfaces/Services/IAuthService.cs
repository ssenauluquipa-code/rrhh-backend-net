

using Rrhh_backend.Presentation.DTOs.Requests;
using Rrhh_backend.Presentation.DTOs.Responses;

namespace Rrhh_backend.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<bool> LogoutAsync(string token);
        bool ValidateToken(string token);
    }
}
