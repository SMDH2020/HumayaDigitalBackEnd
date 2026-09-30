using HD.Generales.MCP.Config;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HD.Generales.MCP.Services
{
    public class McpJwtService
    {
        private readonly McpAuthConfig _config;

        public McpJwtService(McpAuthConfig config) => _config = config;

        public string GenerarAccessToken(Guid usuarioId, string email, string nombre)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.JwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuarioId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email ?? string.Empty),
                new Claim("nombre", nombre ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config.JwtIssuer,
                audience: _config.JwtAudiencia,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_config.TokenHoras),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string HashToken(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }

        // Métodos para el flujo alternativo con token temporal (comentado en McpAuthController)
        public string GenerarTokenTemporal(Guid usuarioId, string email, string redirectUri, string state)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.JwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuarioId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email ?? string.Empty),
                new Claim("redirect", redirectUri ?? string.Empty),
                new Claim("state", state ?? string.Empty)
            };

            var token = new JwtSecurityToken(
                issuer: _config.JwtIssuer,
                audience: _config.JwtAudiencia,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_config.TempTokenMins),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal ValidarTokenTemporal(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.JwtSecret));
            try
            {
                return new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _config.JwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = _config.JwtAudiencia,
                    ValidateLifetime = true,
                    IssuerSigningKey = key
                }, out _);
            }
            catch
            {
                return null;
            }
        }
    }
}
