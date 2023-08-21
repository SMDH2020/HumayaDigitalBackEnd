using System.Security.Claims;
using HD.Security.interfaces;
using Microsoft.AspNetCore.Http;

namespace HD.Security.clases
{
    public class UsuarioSesion : IUsuarioSesion
    {
        private readonly IHttpContextAccessor http;
        public UsuarioSesion(IHttpContextAccessor _http)
        {
            http = _http;
        }
        public string ID()
        {
            var userName = http.HttpContext.User?.Claims?.
                    FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var token = http.HttpContext.Request.Headers.FirstOrDefault(x => x.Key.Equals("Authorization"));
            if (userName is null || token.Key is null)
                return "";

            string[] datos = userName.Split('%');

            string[] retorno = new string[3];
            retorno[0] = datos[0];
            //retorno[1] = datos[1];
            //retorno[2] = token.Value.ToString().Substring(7);

            return datos[0];
        }
    }
}