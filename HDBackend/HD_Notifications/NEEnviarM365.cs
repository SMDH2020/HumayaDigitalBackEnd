using HD.AccesoDatos;
using HD.Notifications.Modelos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace HD.Notifications
{
    /// <summary>
    /// Envio de correo por Microsoft Graph (Microsoft 365), con OAuth 2.0 y flujo
    /// client credentials. Sustituye a NEEnviar, que usaba SMTP con usuario y
    /// contrasena: Exchange Online deshabilita la autenticacion basica de SMTP a
    /// finales de diciembre de 2026 y a partir de entonces esos envios se rechazan
    /// con 550 5.7.30 sin reintento.
    ///
    /// No requiere paquetes NuGet nuevos: usa HttpClient y Newtonsoft.Json.
    /// </summary>
    public class NEEnviarM365
    {
        private const string AutoridadBase = "https://login.microsoftonline.com";
        private const string GraphBase = "https://graph.microsoft.com/v1.0";
        private const string Ambito = "https://graph.microsoft.com/.default";

        // HttpClient estatico: crear uno por envio agota los sockets del servidor.
        private static readonly HttpClient Cliente = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        // El token dura ~60 minutos. Se reutiliza mientras siga vigente.
        private static string? TokenCache;
        private static DateTime TokenExpira = DateTime.MinValue;
        private static readonly SemaphoreSlim Candado = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Envia un correo HTML a uno o varios destinatarios.
        /// </summary>
        /// <param name="config">Credenciales de la app de Entra y buzon remitente.</param>
        /// <param name="_asunto">Asunto del mensaje.</param>
        /// <param name="_body">Cuerpo en HTML.</param>
        /// <param name="para">Destinatarios.</param>
        /// <param name="copia">Destinatarios en copia. Opcional.</param>
        public static async Task<string> Enviar(mdl_Correo_M365 config, string _asunto, string _body, string[] para, string[]? copia = null)
        {
            try
            {
                ValidarConfiguracion(config);

                if (para == null || para.Length == 0)
                    throw new Excepciones(System.Net.HttpStatusCode.BadRequest, new { Mensaje = "No se indico ningun destinatario" });

                string token = await ObtenerToken(config);

                var mensaje = new
                {
                    message = new
                    {
                        subject = _asunto,
                        body = new
                        {
                            contentType = "HTML",
                            content = _body
                        },
                        toRecipients = para
                            .Where(x => !string.IsNullOrWhiteSpace(x))
                            .Select(x => new { emailAddress = new { address = x.Trim() } })
                            .ToArray(),
                        ccRecipients = (copia ?? Array.Empty<string>())
                            .Where(x => !string.IsNullOrWhiteSpace(x))
                            .Select(x => new { emailAddress = new { address = x.Trim() } })
                            .ToArray()
                    },
                    saveToSentItems = config.GuardarEnEnviados
                };

                string url = GraphBase + "/users/" + Uri.EscapeDataString(config.Remitente!.Trim()) + "/sendMail";

                using (var peticion = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    peticion.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    peticion.Content = new StringContent(JsonConvert.SerializeObject(mensaje), Encoding.UTF8, "application/json");

                    var respuesta = await Cliente.SendAsync(peticion);

                    // Graph responde 202 Accepted cuando acepta el mensaje para su envio.
                    if (!respuesta.IsSuccessStatusCode)
                    {
                        string detalle = await respuesta.Content.ReadAsStringAsync();
                        throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new
                        {
                            Mensaje = "Microsoft Graph rechazo el envio (" + (int)respuesta.StatusCode + "): " + DescribirError(detalle)
                        });
                    }
                }

                return "Mensaje enviado con exito";
            }
            catch (Excepciones)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un token de aplicacion contra Entra ID y lo reutiliza mientras
        /// siga vigente, con un margen de 5 minutos antes de su expiracion.
        /// </summary>
        private static async Task<string> ObtenerToken(mdl_Correo_M365 config)
        {
            if (!string.IsNullOrEmpty(TokenCache) && DateTime.UtcNow < TokenExpira)
                return TokenCache!;

            await Candado.WaitAsync();
            try
            {
                if (!string.IsNullOrEmpty(TokenCache) && DateTime.UtcNow < TokenExpira)
                    return TokenCache!;

                var formulario = new Dictionary<string, string>
                {
                    { "client_id", config.ClientId!.Trim() },
                    { "client_secret", config.ClientSecret! },
                    { "scope", Ambito },
                    { "grant_type", "client_credentials" }
                };

                string url = AutoridadBase + "/" + config.TenantId!.Trim() + "/oauth2/v2.0/token";
                var respuesta = await Cliente.PostAsync(url, new FormUrlEncodedContent(formulario));
                string contenido = await respuesta.Content.ReadAsStringAsync();

                if (!respuesta.IsSuccessStatusCode)
                {
                    throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new
                    {
                        Mensaje = "No se pudo autenticar contra Microsoft Entra ID: " + DescribirError(contenido)
                    });
                }

                var json = JObject.Parse(contenido);
                string? token = json["access_token"]?.ToString();
                int segundos = json["expires_in"]?.Value<int>() ?? 3600;

                if (string.IsNullOrEmpty(token))
                {
                    throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new
                    {
                        Mensaje = "Microsoft Entra ID no devolvio un token de acceso"
                    });
                }

                TokenCache = token;
                TokenExpira = DateTime.UtcNow.AddSeconds(segundos - 300);
                return token!;
            }
            finally
            {
                Candado.Release();
            }
        }

        private static void ValidarConfiguracion(mdl_Correo_M365 config)
        {
            if (config == null)
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = "Falta la configuracion de correo de Microsoft 365" });

            var faltantes = new List<string>();
            if (string.IsNullOrWhiteSpace(config.TenantId)) faltantes.Add("TenantId");
            if (string.IsNullOrWhiteSpace(config.ClientId)) faltantes.Add("ClientId");
            if (string.IsNullOrWhiteSpace(config.ClientSecret)) faltantes.Add("ClientSecret");
            if (string.IsNullOrWhiteSpace(config.Remitente)) faltantes.Add("Remitente");

            if (faltantes.Count > 0)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new
                {
                    Mensaje = "Configuracion de correo incompleta, faltan: " + string.Join(", ", faltantes)
                });
            }
        }

        /// <summary>
        /// Extrae el mensaje legible de una respuesta de error de Graph o de Entra ID.
        /// </summary>
        private static string DescribirError(string contenido)
        {
            if (string.IsNullOrWhiteSpace(contenido)) return "sin detalle";

            try
            {
                var json = JObject.Parse(contenido);
                string? mensaje = json["error"]?["message"]?.ToString()      // Graph
                               ?? json["error_description"]?.ToString();      // Entra ID
                return string.IsNullOrWhiteSpace(mensaje) ? contenido : mensaje!;
            }
            catch
            {
                return contenido;
            }
        }
    }
}
