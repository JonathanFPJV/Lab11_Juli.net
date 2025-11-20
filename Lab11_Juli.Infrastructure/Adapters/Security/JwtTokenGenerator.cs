using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Lab11_Juli.Domain.Ports.Services;
using Lab11_Juli.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Lab11_Juli.Infrastructure.Adapters.Security;

public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly SymmetricSecurityKey _key;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            // Leemos la configuración desde appsettings.json
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!));
            _issuer = configuration["Jwt:Issuer"]!;
            _audience = configuration["Jwt:Audience"]!;
        }

        public string GenerateToken(User user, IEnumerable<string> roles)
        {
            // 1. Crear los "Claims" (información dentro del token)
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()), // 'Subject' (el ID del usuario)
                new Claim(JwtRegisteredClaimNames.Name, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // ID único del token
            };

            // 2. Agregar los roles del usuario como claims
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // 3. Crear las credenciales de firma
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            // 4. Crear el token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8), // Duración del token
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 5. Devolver el token como string
            return tokenHandler.WriteToken(token);
        }
    }