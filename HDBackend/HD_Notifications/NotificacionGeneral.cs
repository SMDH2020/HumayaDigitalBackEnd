using HD.Notifications.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Notifications
{
    public class NotificacionGeneral
    {
        private const string RutaLogo = "C:\\SMDH\\logo.jpg";

        public static async Task<bool> Enviar(
            mdl_Correo_M365 config,
            string asunto,
            mdl_Datos_Correo datos,
            List<string> destinatarios)
        {
            destinatarios = (destinatarios ?? new List<string>())
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (destinatarios.Count == 0)
            {
                Console.WriteLine("⚠ Sin destinatarios, no se envía correo: " + asunto);
                return false;
            }

            try
            {
                string html = GenerarCorreo.Renderizar(datos);

                // El logo va como imagen en línea (cid:logoHumaya).
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

                await NEEnviarM365.Enviar(config, asunto, html, destinatarios.ToArray(), null, adjuntos);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR AL ENVIAR CORREO ({asunto}): {ex.Message}");
                throw;
            }
        }

        public static bool LogoDisponible() => File.Exists(RutaLogo);
    }
}
