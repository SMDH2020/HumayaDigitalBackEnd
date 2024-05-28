using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HD.Security
{
    public  class JwtManager
    {

        public static Task<string> GenerarTocken(string _idusuario,
            string _nombre,
            string _securitykey,
            string _Iussuer,
            string _Audience,
            int _minutossesion)
        {
            // Generamos un token según los claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, _idusuario),
                new Claim(ClaimTypes.Name, _nombre),
                new Claim(ClaimTypes.GivenName, $"{_idusuario} {_nombre}")
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securitykey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _Iussuer,
                audience: _Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(_minutossesion),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return Task.FromResult(jwt);

        }

    }
}
