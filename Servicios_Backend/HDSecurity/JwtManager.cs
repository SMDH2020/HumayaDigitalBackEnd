using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HDSecurity
{
    public class JwtManager
    {

        private const string Secret = "mysecretkey123Maquinari4DelHum@ll@45.*+(ASbv?/23)a;a!ER34Ewsawea"; // Clave secreta para firmar y validar el token
        private const string domain = "https://www.humayadigital.mx";
        public static string GenerateToken(string username)
        {
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512Signature);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, username));
            claims.Add(new Claim(ClaimTypes.SerialNumber, "1"));
            claims.Add(new Claim(ClaimTypes.Role, "Administrador,Manager"));
            var tokenDescripcion = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), // Fecha de expiración del token
                SigningCredentials = signingCredentials,
                Issuer = domain,
                Audience = domain,
            };
            var tokenmanejador = new JwtSecurityTokenHandler();
            var token = tokenmanejador.CreateToken(tokenDescripcion);
            return tokenmanejador.WriteToken(token);
        }

        public static JwtValidateResult GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = symmetricKey,
                    ValidateIssuer = true,
                    ValidIssuer = domain,
                    ValidateAudience = true,
                    ValidAudience = domain,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                SecurityToken validatedToken;
                var result = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                return new JwtValidateResult(validatedToken, result);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
