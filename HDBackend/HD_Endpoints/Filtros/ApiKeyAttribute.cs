using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HD.Endpoints.Filtros
{
    /// <summary>
    /// Autoriza la accion comparando el header X-Api-Key contra la clave guardada en
    /// configuracion (appsettings o variable de entorno). Pensado para integraciones
    /// de sistema a sistema que no tienen JWT. Se combina con [AllowAnonymous] para
    /// saltar el [Authorize] de MyBase.
    /// Si la clave no esta configurada se rechaza todo (falla cerrado).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAttribute : Attribute, IAuthorizationFilter
    {
        private const string NombreHeader = "X-Api-Key";
        private readonly string ClaveConfiguracion;

        public ApiKeyAttribute(string claveConfiguracion)
        {
            ClaveConfiguracion = claveConfiguracion;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            IConfiguration configuracion = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            string? esperada = configuracion[ClaveConfiguracion];
            string? recibida = context.HttpContext.Request.Headers[NombreHeader].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(esperada) || string.IsNullOrWhiteSpace(recibida) || !SonIguales(esperada, recibida))
            {
                context.Result = new UnauthorizedObjectResult(new { Mensaje = "API Key invalida o ausente" });
            }
        }

        // Comparacion en tiempo constante para no filtrar la clave por tiempos de respuesta.
        private static bool SonIguales(string a, string b)
        {
            return CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(a), Encoding.UTF8.GetBytes(b));
        }
    }
}
