using HD.Generales.MCP.Config;
using HD.Notifications;
using HD.Notifications.Modelos;

namespace HD.Generales.MCP.Services
{
    public class McpEmailService
    {
        private readonly McpAuthConfig _config;
        private readonly mdl_Correo_M365 _correo;

        public McpEmailService(McpAuthConfig config, mdl_Correo_M365 correo)
        {
            _config = config;
            _correo = correo;
        }

        public async Task EnviarCodigoMfa(string emailDestino, string nombre, string codigo)
        {
            string cuerpo = $@"
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
                </div>";

            await NEEnviarM365.Enviar(_correo, "Tu código de verificación MCP", cuerpo, new[] { emailDestino });
        }
    }
}
