using HD.Notifications.Consultas;
using HD.Notifications.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HD.Notifications
{
    public class AD_OneSignal
    {

        private string CadenaConexion;
        public AD_OneSignal(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        private const string OneSignalAppId = "04e611d6-045a-4105-af2d-04880d3c4cb9"; // Tu App ID
        private const string OneSignalApiKey = "os_v2_app_attbdvqeljaqllznasea2pcmxhabgbplkusuiiuxlb7w5pg5jbovlhtlnmxe6jytp2ikt2czoxnetpqegavnpirw6mdho6a3syqje7i";

        public async Task<bool> EnviarTodos(int idencabezado,  DateTime fecha_evento, string usuario) {

            AD_Conseguir_Mensaje_Manual datos = new AD_Conseguir_Mensaje_Manual(CadenaConexion);
            var resultado = await datos.obtenerID(idencabezado, fecha_evento, usuario);

            using var client = new HttpClient();

            var payload = new
            {
                app_id = OneSignalAppId,
                included_segments = new[] { "All" },
                //include_player_ids = new[] { data.onSignal }, // ✅ CAMBIO AQUÍ
                headings = new { en = "Humaya Digital" },
                contents = new { en = resultado.mensaje ?? "Mensaje por defecto" },
                data = new
                {
                    idlog = resultado.idlog,
                    mensaje = resultado.mensaje ?? "",
                    portafolio = resultado.portafolio ?? "",
                    parametro = resultado.parametro ?? "",
                    estado = resultado.estado ?? "",
                    cliente = resultado.cliente ?? "",
                    redireccion = resultado.redireccion ?? "",
                    redireccionweb = resultado.redireccionweb ?? "",
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

        public async Task<bool> EnviarEspecifico(mdl_Notificacion_Usuarios_Solicitudes_View mdl)
        {
            using var client = new HttpClient();

            var playerIds = mdl.notificacionUsuarios?.Select(u => u.idSuscripcion).ToArray() ?? new string[] { };

            var payload = new
            {
                app_id = OneSignalAppId,
                //included_segments = new[] { "All" },
                include_player_ids = playerIds, // ✅ CAMBIO AQUÍ

                headings = new { en = "Humaya Digital"},
                contents = new { en = mdl.notificacionCuerpo.mensaje ?? "Mensaje por defecto" },
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
