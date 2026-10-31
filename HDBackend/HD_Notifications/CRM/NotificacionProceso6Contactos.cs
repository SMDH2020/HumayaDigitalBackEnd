using HD.Clientes.Modelos.CRM.Proceso6Contactos;
using HD.Notifications.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Notifications.CRM
{
    public class NotificacionProceso6Contactos
    {
        private const string RutaLogo = "C:\\SMDH\\logo.jpg";

        public static async Task<int> Enviar(
            mdl_Correo_M365 config,
            IEnumerable<mdl_Notifica_Proceso_6_Contactos> pendientes)
        {
            int enviados = 0;

            // El logo se lee una sola vez para todos los correos del lote
            var adjuntos = new List<mdl_Correo_Adjunto>();
            if (File.Exists(RutaLogo))
            {
                adjuntos.Add(new mdl_Correo_Adjunto
                {
                    Nombre = "logo.jpg",
                    ContentType = "image/jpeg",
                    Contenido = await File.ReadAllBytesAsync(RutaLogo),
                    ContentId = "logoHumaya"
                });
            }

            var grupos = pendientes
                .Where(p => !string.IsNullOrWhiteSpace(p.correo))
                .GroupBy(p => p.correo.Trim().ToLowerInvariant());

            foreach (var grupo in grupos)
            {
                var primero = grupo.First();
                int total = grupo.Count();

                // Un cliente por línea
                string listado = string.Join("\n",
                    grupo.Select(p => $"• {p.razon_social}"));

                var datos = new mdl_Datos_Correo
                {
                    Seccion = "Proceso de 6 contactos",
                    Titulo = "Clientes por contactar mañana",
                    Accion = total == 1 ? "1 cliente" : $"{total} clientes",
                    Cuerpo =
                    {
                        new("Responsable", primero.nombre_responsable),
                        new("Clientes a contactar", listado),
                    },
                    Instrucciones = "Ingresa al sistema para registrar el resultado de cada contacto."
                };

                string asunto = total == 1
                    ? "Tienes 1 cliente por contactar mañana"
                    : $"Tienes {total} clientes por contactar mañana";

                string html = GenerarCorreo.Renderizar(datos);

                try
                {
                    await NEEnviarM365.Enviar(
                        config,
                        asunto,
                        html,
                        new[] { primero.correo.Trim() },
                        null,
                        adjuntos);

                    enviados++;
                }
                catch (Exception ex)
                {
                    // Si un destinatario falla, los demás deben seguir recibiendo
                    Console.WriteLine($"ERROR CORREO PROCESO 6 CONTACTOS ({primero.correo}): {ex.Message}");
                }
            }

            return enviados;
        }
    }
}