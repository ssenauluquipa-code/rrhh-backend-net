using Rrhh_backend.Core.Entities;

namespace Rrhh_backend.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        //IEnumerable<UserDto> getAllUsers();
        //UserDto? GetById(long id);
        Task<List<User>> GetUsers();
        Task<User?> GetUserById(int id);
        Task<User?> GetUserByEmail(string email);
        Task<User?> CreateUser(User user);
        Task<User?> UpdateUser(int id, User user);
        Task<bool> DeleteUser(int id);
        User? Authenticate(string email, string password);
    }
}
