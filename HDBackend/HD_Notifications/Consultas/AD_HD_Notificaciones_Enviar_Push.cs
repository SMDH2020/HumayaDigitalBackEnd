using HD.Notifications.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HD.Notifications.Consultas
{
    public class AD_HD_Notificaciones_Enviar_Push
    {
        private string CadenaConexion;
        public AD_HD_Notificaciones_Enviar_Push(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }


        public async Task<bool> Enviar_Notificacion_Solicitud(mdl_Notificacion_Usuarios_Solicitudes_View mdl, string? titulo)
        {
            using var client = new HttpClient();

            var playerIds = mdl.notificacionUsuarios?.Select(u => u.idSuscripcion).ToArray() ?? new string[] { };

            var payload = new
            {
                app_id = OneSignalAppId,
                //included_segments = new[] { "All" },
                include_player_ids = playerIds, // ✅ CAMBIO AQUÍ

                headings = new { en = titulo ?? "Título por defecto" },
                contents = new { en = mdl.notificacionCuerpo.mensaje ?? "Mensaje por defecto"},
                data = new
                {
                    idlog = mdl.notificacionCuerpo.idlog,
                    mensaje = mdl.notificacionCuerpo.mensaje ?? "",
                    portafolio = mdl.notificacionCuerpo.portafolio ?? "",
                    parametro = mdl.notificacionCuerpo.parametro ?? "",
                    estado = mdl.notificacionCuerpo.estado ?? "",
                    cliente = mdl.notificacionCuerpo.cliente ?? "",
                    redireccion = mdl.notificacionCuerpo.redireccion ?? "",
                    redireccionweb = mdl.notificacionCuerpo.redireccionweb ?? "",
                }

            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Add("Authorization", $"Basic {OneSignalApiKey}");

            var response = await client.PostAsync("https://onesignal.com/api/v1/notifications", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return true;
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return false;
            }
        }
    }
}
