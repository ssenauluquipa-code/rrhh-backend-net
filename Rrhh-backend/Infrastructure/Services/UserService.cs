using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Exceptions;
using Rrhh_backend.Core.Interfaces.Repositories;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Requests.Users;
using Rrhh_backend.Presentation.DTOs.Responses.Users;
using Rrhh_backend.Security;

namespace Rrhh_backend.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly PasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, 
                           IEmployeeRepository employeeRepository, 
                           IRolesRepository rolesRepository){
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
            _rolesRepository = rolesRepository;
            _passwordHasher = new PasswordHasher();
        }

        public async Task<List<UserResponse>> GetAllAsync()
        {
            //var users = await _userRepository.GetUsers();
            //return users.Select(MapToResponse).ToList();
            var users = await _userRepository.GetUsers();
            return users.Select(u => new UserResponse
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                RoleId = u.RoleId,
                RoleName = u.Role.RoleName,
                EmployeeId = u.EmployeeId,
                EmployeeName = u.Employee != null ? $"{u.Employee.FirstName} {u.Employee.LastName}" : null,
                IsActive = u.IsActive
            }).ToList();
        }
        public async Task<UserResponse?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return user != null ? MapToResponse(user) : null;
        }
        public async Task<UserResponse?> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            return user != null ? MapToResponse(user) : null;
        }
        public async Task<UserResponse> CreateAsync(CreateUserRequest request)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if(existingUser != null)
            {
                throw new BusinessException("El Email ya esta registrado");
            }

            var role = await _rolesRepository.GetRolesByIdAsync(request.RoleId);
            if (role ==null){
                throw new BusinessException("Rol no encontrado.");
            }

            Employee? employee = null;
            if (request.EmployeeId.HasValue)
            {
                employee = await _employeeRepository.GetEmployeeByIdAsync(request.EmployeeId.Value);
                if (employee == null)
                    throw new BusinessException("Empleado no encontrado.");
            }
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                RoleId = request.RoleId,
                Role = role,
                EmployeeId = request.EmployeeId,
                Employee = employee
            };

            await _userRepository.CreateUserAsync(user);

            return new UserResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                RoleId = user.RoleId,
                RoleName = user.Role.RoleName,
                EmployeeId = user.EmployeeId,
                EmployeeName = employee != null ? $"{employee.FirstName} {employee.LastName}" : null,
                IsActive = user.IsActive
            };

        }
        public async Task<UserResponse?> UpdateAsync(int id, UpdateUserRequest request)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) throw new BusinessException("Usuario no encontrado.");

            user.UserName = request.UserName;
            user.Email = request.Email;
            user.RoleId = request.RoleId;
            user.EmployeeId = request.EmployeeId;
            user.IsActive = request.IsActive;

            if (!string.IsNullOrEmpty(request.Password))
                user.PasswordHash = _passwordHasher.HashPassword(request.Password);

            await _userRepository.UpdateUser(user);

            //var updated = await _userRepository.UpdateUser(user);
            //return updated != null ? MapToResponse(updated) : null;
            return new UserResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                RoleId = user.RoleId,
                RoleName = user.Role.RoleName,
                EmployeeId = user.EmployeeId,
                EmployeeName = user.Employee != null ? $"{user.Employee.FirstName} {user.Employee.LastName}" : null,
                IsActive = user.IsActive
            };
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
                UserName = user.UserName,
                Email = user.Email,
                RoleName = user.Role?.RoleName ?? "Sin Rol",
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        public async Task<bool> ActicatedAsync(int id)
        {
            return await _userRepository.ActiveAsync(id);
        }
    }
}
