
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
                    new Claim("RoleId", user.RoleId.ToString())
                }),
                //Expires = DateTime.UtcNow.AddMinutes(_config.GetValue<int>("JwtSettings:ExpirationMinutes")),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        //verificamos si el token esta a punto de expirar
        public bool IsTokenExpired(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp");
                if (expClaim == null) return true; // Si no tiene expiración, considera expirado

                var expTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim.Value)).UtcDateTime;
                return expTime <= DateTime.UtcNow.AddMinutes(5); // Si expira en menos de 5 minutos
            }
            catch
            {
                return true; // Si no se puede leer, considera expirado
            }
        }

        //con esto refrescamos el token
        public string RefreshToken(string expiredToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(expiredToken);

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

                if (userIdClaim == null || emailClaim == null || roleClaim == null)
                    throw new BusinessException("Token inválido");

                // Busca el usuario en la BD (para generar nuevo token con datos actuales)
                var userId = int.Parse(userIdClaim.Value);
                var user = _userRepository.GetUserByIdAsync(userId).Result; // O usa async/await

                if (user == null)
                    throw new BusinessException("Usuario no encontrado");

                // Genera nuevo token
                return GenerateTokenJWT(user);
            }
            catch (Exception ex)
            {
                throw new BusinessException("No se pudo renovar el token: " + ex.Message);
            }
        }
    }
}
