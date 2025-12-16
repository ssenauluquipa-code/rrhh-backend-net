
using Microsoft.IdentityModel.Tokens;
using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Exceptions;
using Rrhh_backend.Core.Interfaces.Repositories;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Requests.Auth;
using Rrhh_backend.Presentation.DTOs.Responses.Auth;
using Rrhh_backend.Security;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Rrhh_backend.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher _passwordHasher;

        public AuthService(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher();
        }

        //public async Task<LoginResponse?> Login(LoginRequest reques)
        //{
        //    var users = await _userRepository.GetUserByEmail(reques.Email);
        //    if(users == null || users.Password != reques.Password)
        //    {
        //        return null;
        //    }
        //    var token = GenerateTokenJWT(users);
        //    return new LoginResponse
        //    {
        //        Token = token,
        //        ExpiresAt = DateTime.UtcNow.AddMinutes(_config.GetValue<int>("JwtSettings:ExpirationMinutes")),
        //        Role= users.Role
        //    };
        //}


        //public bool ValidateToken(string token)
        //{
        //    var key = Encoding.ASCII.GetBytes(_config["JwtSettings:SecretKey"]);
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    try
        //    {
        //        tokenHandler.ValidateToken(token, new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(key),
        //            ValidateIssuer = false,
        //            ValidateAudience = false,
        //            ValidateLifetime = true,
        //            ClockSkew = TimeSpan.Zero
        //        }, out SecurityToken validatedToken);

        //        return validatedToken != null;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //        throw;
        //    }
        //}

        //public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        //{
        //    var user = await _userRepository.GetUserByEmailAsync(request.Email);
        //    if (user == null)
        //    {
        //        Console.WriteLine($"❌ Usuario no encontrado con email: {request.Email}");
        //        return null;
        //    }

        //    Console.WriteLine($"✅ Usuario encontrado: {user.UserName}, IsActive: {user.IsActive}");

        //    if (!user.IsActive)
        //    {
        //        Console.WriteLine($"❌ Usuario inactivo: {user.Email}");
        //        return null;
        //    }
        //    var token = GenerateTokenJWT(user);
        //    return new LoginResponse
        //    {
        //        Token = token,
        //        ExpiresAt = DateTime.UtcNow.AddMinutes(_config.GetValue<int>("JwtSettings:ExpirationMinutes")),
        //        //Role = user.Role
        //    };

        //}

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                throw new BusinessException("Credenciales inválidas.");
            }

            // 3. Validar la contraseña
            var isValidPassword = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
            if (!isValidPassword) // ❗ ¡Ahora es NOT!
            {
                throw new BusinessException("Credenciales inválidas.");
            }

            var token = GenerateTokenJWT(user);
            return new AuthResponse
            {
                Token = token,
                UserId = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Role = user.Role.RoleName
            };
        }

        public async Task<bool> LogoutAsync(string token)
        {
            return await Task.FromResult(false);
        }

      

        public bool ValidateToken(string token)
        {
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Secret"]);
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return validatedToken != null;
            }
            catch
            {
                return false;
            }
        }

        private string GenerateTokenJWT(User user)
        {
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.RoleName),
                    new Claim("Id", user.RoleId.ToString())
                }),
                //Expires = DateTime.UtcNow.AddMinutes(_config.GetValue<int>("JwtSettings:ExpirationMinutes")),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

            
    }
}
