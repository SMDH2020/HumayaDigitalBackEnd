using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HD.Security.interfaces;
using Microsoft.IdentityModel.Tokens;


namespace HD.Security.clases
{
    public class GenerarJWT : IGenerarJWT
    {
        public string CrearToken(string IdUsuario, List<string> Roles, string JWT_SECRET_KEY, int JWT_EXPIRE_MINUTES)
        {
            string value = IdUsuario;
            var claims = new List<Claim>{
               new Claim(JwtRegisteredClaimNames.NameId,value)
           };

            if (Roles != null)
            {
                foreach (var role in Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWT_SECRET_KEY));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescripcion = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credenciales
            };

            var tokenManejador = new JwtSecurityTokenHandler();
            var token = tokenManejador.CreateToken(tokenDescripcion);

            return tokenManejador.WriteToken(token);
        }

    }
}