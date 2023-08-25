using Microsoft.AspNetCore.Http;
using static System.Net.WebRequestMethods;
using System.Security.Claims;
using HD.AccesoDatos;

namespace HD.Security
{
    public class Sesion : ISesion
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public Sesion(IHttpContextAccessor _http)
        {
            _contextAccessor = _http;
        }

        string? ISesion.usuario()
        {
            string? userName = _contextAccessor.HttpContext?.User?.Claims?.
            FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value;

            return userName;
        }
    }
}
