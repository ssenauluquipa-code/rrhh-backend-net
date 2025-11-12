using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Interfaces.Repositories;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Requests;
using Rrhh_backend.Presentation.DTOs.Responses;

namespace Rrhh_backend.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserResponse>> GetAllAsync()
        {
            var users = await _userRepository.GetUsers();
            return users.Select(MapToResponse).ToList();
        }
        public async Task<UserResponse?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetUserById(id);
            return user != null ? MapToResponse(user) : null;
        }
        public async Task<UserResponse?> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            return user != null ? MapToResponse(user) : null;
        }
        public async Task<UserResponse> CreateAsync(CreateUserRequest request)
        {
            var user = new User
            {
                UserName = request.Username,
                Email = request.Email,
                Password = request.Password, // En el futuro: encriptar
                Role = request.Role
            };

            var created = await _userRepository.CreateUser(user);
            return MapToResponse(created);
        }
        public async Task<UserResponse?> UpdateAsync(int id, UpdateUserRequest request)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null) return null;

            if (!string.IsNullOrEmpty(request.UserName))
                user.UserName = request.UserName;
            if (!string.IsNullOrEmpty(request.Email))
                user.Email = request.Email;
            if (!string.IsNullOrEmpty(request.Role))
                user.Role = request.Role;

            user.UpdatedAt = DateTime.UtcNow;

            var updated = await _userRepository.UpdateUser(id, user);
            return updated != null ? MapToResponse(updated) : null;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await _userRepository.DeleteUser(id);
        }
        private static UserResponse MapToResponse(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}
