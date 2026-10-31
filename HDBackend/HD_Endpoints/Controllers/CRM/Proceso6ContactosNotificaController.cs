using HD.Clientes.Consultas.CRM.Proceso6Contactos;
using HD.Endpoints.Filtros;
using HD.Notifications;
using HD.Notifications.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HD.Security;

namespace HD.Endpoints.Controllers.CRM
{
    [AllowAnonymous]
    [ApiKey("Proceso6ContactosNotifica:ApiKey")]
    public class Proceso6ContactosNotificaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public Proceso6ContactosNotificaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> NotificarProceso6Contactos()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Proceso_6_Contactos datos = new AD_Proceso_6_Contactos(CadenaConexion);

            var pendientes = await datos.Proceso6ContactosNotificar();

            if (!pendientes.Any())
                return Ok(new { mensaje = "No hay contactos programados para mañana" });

            var config = Configuracion.GetSection("CorreoM365").Get<mdl_Correo_M365>();
            int enviados = 0;

            var grupos = pendientes
                .Where(p => !string.IsNullOrWhiteSpace(p.correo))
                .GroupBy(p => p.correo.Trim().ToLowerInvariant());

            foreach (var grupo in grupos)
            {
                var primero = grupo.First();
                int total = grupo.Count();

                string listado = string.Join("\n\n",
                grupo.Select(p =>
                    $"• {p.razon_social}\n   {p.modelo} · Entregado el {p.fecha_entrega}"));

                var datosCorreo = new mdl_Datos_Correo
                {
                    Seccion = "Proceso de seguimiento de 6 contactos",
                    Titulo = "Como parte del seguimiento del proceso de 6 contactos se le comparte un listado con los clientes que deben ser contactados el dia de mañana.",
                    Accion = total == 1 ? "1 cliente" : $"{total} clientes",
                    Cuerpo =
                    {
                            //new("Responsable", primero.nombre_responsable),
                            new("Clientes a contactar", listado),
                    },
                    Instrucciones = "Ingresa al sistema para registrar el contacto con el cliente."
                };

                string asunto = "Contactos pendientes para mañana - Proceso de 6 contactos";

                try
                {
                    if (await NotificacionGeneral.Enviar(
                            config, asunto, datosCorreo, new List<string> { primero.correo }))
                        enviados++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Omitido {primero.correo}: {ex.Message}");
                }
            }

            return Ok(new { mensaje = $"Se enviaron {enviados} notificaciones" });
        }
    }
}
