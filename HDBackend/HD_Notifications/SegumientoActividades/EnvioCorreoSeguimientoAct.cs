using HD.Notifications.Modelos;

namespace HD.Notifications.SeguimientoActividades
{
    // Mecánica de envío (Microsoft Graph + logo en línea) compartida por
    // NotificacionSeguimientoAct y NotificacionSeguimientoActComentario --
    // antes cada una traía su propio bloque de envío
    // casi idéntico.
    internal static class EnvioCorreoSeguimientoAct
    {
        private const string RutaLogo = "C:\\SMDH\\logo.jpg";

        public static async Task<bool> Enviar(mdl_Correo_M365 config, string asunto, string html, List<string> destinatarios)
        {
            destinatarios = (destinatarios ?? new List<string>())
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (destinatarios.Count == 0)
            {
                Console.WriteLine("⚠ Seguimiento de Actividades: sin destinatarios, no se envía correo.");
                return false;
            }

            try
            {
                // El logo va como imagen en línea (cid:logoHumaya), igual que antes con LinkedResource.
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
                Console.WriteLine("ERROR CORREO SEGUIMIENTO ACTIVIDADES: " + ex.Message);
                throw;
            }
        }

        public static bool LogoDisponible() => File.Exists(RutaLogo);
    }
}
