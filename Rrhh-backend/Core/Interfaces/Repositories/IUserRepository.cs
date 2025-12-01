using Rrhh_backend.Core.Entities;

namespace Rrhh_backend.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        //IEnumerable<UserDto> getAllUsers();
        //UserDto? GetById(long id);
        Task<List<User>> GetUsers();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> CreateUserAsync(User user);
        Task<User?> UpdateUser(User user);
        Task<bool> DeleteUser(int id);

        Task<bool> ActiveAsync(int id);
        User? Authenticate(string email, string password);
    }
}
