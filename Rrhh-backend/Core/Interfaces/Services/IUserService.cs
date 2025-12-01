using Rrhh_backend.Presentation.DTOs.Requests.Users;
using Rrhh_backend.Presentation.DTOs.Responses.Users;

namespace Rrhh_backend.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<List<UserResponse>> GetAllAsync();
        Task<UserResponse?> GetByIdAsync(Guid id);
        Task<UserResponse?> GetByEmailAsync(string email);
        Task<UserResponse> CreateAsync(CreateUserRequest request);
        Task<UserResponse?> UpdateAsync(Guid id, UpdateUserRequest request);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ActicatedAsync(Guid id);
    }
}
