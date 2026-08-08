using System.Net.Mail;
using System.Net.Mime;

namespace HD.Notifications.SeguimientoActividades
{
    // Mecánica de envío (SMTP + logo embebido) compartida por
    // NotificacionSeguimientoAct y NotificacionSeguimientoActComentario --
    // antes cada una traía su propio bloque de SmtpClient/MailMessage
    // casi idéntico.
    internal static class EnvioCorreoSeguimientoAct
    {
        private const string Password = "!HD_Hum4y4D1g1t4l*T1?";
        private const string CorreoOrigen = "HumayaDigital@humaya.com.mx";
        private const string RutaLogo = "C:\\SMDH\\logo.jpg";

        public static async Task<bool> Enviar(string asunto, string html, List<string> destinatarios)
        {
            destinatarios = (destinatarios ?? new List<string>())
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Distinct()
                .ToList();

            if (destinatarios.Count == 0)
            {
                Console.WriteLine("⚠ Seguimiento de Actividades: sin destinatarios, no se envía correo.");
                return false;
            }

            try
            {
                bool incluirLogo = File.Exists(RutaLogo);

                using var client = new SmtpClient
                {
                    Port = 587,
                    Host = "correo.humaya.com.mx",
                    Timeout = 20000,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(CorreoOrigen, Password),
                    EnableSsl = false
                };

                using var mensaje = new MailMessage
                {
                    From = new MailAddress(CorreoOrigen),
                    Subject = asunto,
                    IsBodyHtml = true
                };

                foreach (var correo in destinatarios)
                    mensaje.To.Add(new MailAddress(correo));

                var vistaHtml = AlternateView.CreateAlternateViewFromString(html, null, "text/html");

                if (incluirLogo)
                {
                    var logo = new LinkedResource(RutaLogo, "image/jpeg")
                    {
                        ContentId = "logoHumaya",
                        TransferEncoding = TransferEncoding.Base64
                    };
                    vistaHtml.LinkedResources.Add(logo);
                }

                mensaje.AlternateViews.Add(vistaHtml);

                await client.SendMailAsync(mensaje);
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
