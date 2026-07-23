using HD.Generales.MCP.Config;
using System.Net;
using System.Net.Mail;

namespace HD.Generales.MCP.Services
{
    public class McpEmailService
    {
        private readonly McpAuthConfig _config;

        public McpEmailService(McpAuthConfig config)
        {
            _config = config;
        }

        public void EnviarCodigoMfa(string emailDestino, string nombre, string codigo)
        {
            using var smtp = new SmtpClient(_config.SmtpHost, _config.SmtpPuerto)
            {
                Credentials = new NetworkCredential(_config.SmtpUsuario, _config.SmtpPassword),
                EnableSsl = true
            };

            var msg = new MailMessage
            {
                From = new MailAddress(_config.EmailRemitente, "Humaya Digital"),
                Subject = "Tu código de verificación MCP",
                IsBodyHtml = true,
                Body = $@"
                <div style='font-family:Arial;max-width:400px;margin:auto;padding:30px'>
                    <h2 style='color:#367C2B'>Humaya Digital – MCP</h2>
                    <p>Hola <strong>{nombre}</strong>,</p>
                    <p>Tu código de verificación es:</p>
                    <div style='font-size:36px;font-weight:bold;letter-spacing:10px;
                                color:#367C2B;text-align:center;padding:20px;
                                background:#F4F5F5;border-radius:8px;margin:20px 0'>
                        {codigo}
                    </div>
                    <p style='color:#666;font-size:12px'>
                        Expira en <strong>10 minutos</strong>.<br>
                        Si no solicitaste este acceso, ignora este correo.
                    </p>
                </div>"
            };

            msg.To.Add(emailDestino);
            smtp.Send(msg);
        }
    }
}
