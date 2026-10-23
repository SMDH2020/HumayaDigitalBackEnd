using HD.Notifications.Modelos;

namespace HD.Notifications.GestionProyectosTI
{
    // Mecanica de envio (Microsoft Graph + logo en linea) para las
    // notificaciones de Gestion de Proyectos de TI. Mismo patron que
    // HD_Notifications/SegumientoActividades/EnvioCorreoSeguimientoAct.cs.
    internal static class EnvioCorreoTI
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
                Console.WriteLine("⚠ Gestión de Proyectos de TI: sin destinatarios, no se envía correo.");
                return false;
            }

            try
            {
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
                Console.WriteLine("ERROR CORREO GESTION PROYECTOS TI: " + ex.Message);
                throw;
            }
        }

        public static bool LogoDisponible() => File.Exists(RutaLogo);
    }
}
