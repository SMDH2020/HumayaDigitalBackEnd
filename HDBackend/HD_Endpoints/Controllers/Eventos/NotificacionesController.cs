using HD.Notifications.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using HD.Notifications;


//namespace HD.Endpoints.Controllers.Eventos
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class NotificacionesController : ControllerBase
//    {

//        private readonly IConfiguration Configuracion;
//        private readonly ISesion Sesion;

//        public NotificacionesController(IConfiguration configuration, ISesion sesion)
//        {
//            Configuracion = configuration;
//            Sesion = sesion;
//        }

//private const string OneSignalAppId = "04e611d6-045a-4105-af2d-04880d3c4cb9"; // Tu App ID
//private const string OneSignalApiKey = "os_v2_app_attbdvqeljaqllznasea2pcmxgflnvyahosesendbr7qoaleizbkfh73lqkpbwxfdb53m3f5gjlmcme6bhgtbrczxvi5uyvlbqnrwvy"; // ⚠️ Tu REST API Key

//        [HttpPost("enviar")]
//        public async Task<IActionResult> EnviarNotificacion([FromBody] NotificacionDto data)
//        {

//            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
//            AD_Conseguir_Mensaje_Manual datos = new AD_Conseguir_Mensaje_Manual(CadenaConexion);
//            var usuario = Sesion.usuario();
//            var resultado = await datos.obtenerID(data.idencabezado, data.fecha_evento, usuario);

//using var client = new HttpClient();

//var payload = new
//{
//    app_id = OneSignalAppId,
//    included_segments = new[] { "All" },
//    headings = new { en = data.Titulo ?? "Título por defecto" },
//    contents = new { en = resultado.mensaje ?? "Mensaje por defecto" },
//    data = new { targetPage = resultado.redireccion ?? "" }

//};

//var jsonPayload = JsonSerializer.Serialize(payload);
//var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

//client.DefaultRequestHeaders.Add("Authorization", $"Basic {OneSignalApiKey}");

//var response = await client.PostAsync("https://onesignal.com/api/v1/notifications", content);

//if (response.IsSuccessStatusCode)
//{
//    var result = await response.Content.ReadAsStringAsync();
//    return Ok(JsonDocument.Parse(result));
//}
//else
//{
//    var error = await response.Content.ReadAsStringAsync();
//    return StatusCode((int)response.StatusCode, error);
//}
//        }

//        public class NotificacionDto
//        {
//            public int idencabezado { get; set; }

//            public string? Titulo { get; set; }
//            public string? Mensaje { get; set; }
//            public string? redireccion { get; set; }
//            public DateTime fecha_evento { get; set; }

//        }
//    }
//}

namespace HD.Endpoints.Controllers.Eventos
{
    public class NotificacionesController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public NotificacionesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        private const string OneSignalAppId = "04e611d6-045a-4105-af2d-04880d3c4cb9"; // Tu App ID
        private const string OneSignalApiKey = "os_v2_app_attbdvqeljaqllznasea2pcmxg3bivslzcpur34bx6g5g6i56rzp3ajx44oerbkx77useti2vmimfrjo636cikgj3axrcrqze4offja"; // ⚠️ Tu REST API Key


        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> enviar(NotificacionDto data)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_OneSignal datos = new AD_OneSignal(CadenaConexion);
            data.usuario = Sesion.usuario();
            await datos.EnviarTodos(data.idencabezado, data.fecha_evento, data.usuario);

            return Ok(new
            {
                mensaje = "Enviado Correctamente",
            });

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> enviarEspecifico(NotificacionDto data)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Conseguir_Mensaje_Manual datos = new AD_Conseguir_Mensaje_Manual(CadenaConexion);
            data.usuario = Sesion.usuario();
            var resultado = await datos.obtenerIDUsuario(data.idencabezado, data.fecha_evento, data.usuario, data.usuarioNotificar);

            using var client = new HttpClient();

            var playerIds = resultado.notificacionUsuarios?.Select(u => u.idSuscripcion).ToArray() ?? new string[] { };

            var payload = new
            {
                app_id = OneSignalAppId,
                //included_segments = new[] { "All" },
                include_player_ids = playerIds, // ✅ CAMBIO AQUÍ

                headings = new { en = data.Titulo ?? "Título por defecto" },
                contents = new { en = resultado.notificacionCuerpo.mensaje ?? "Mensaje por defecto" },
                data = new { targetPage = resultado.notificacionCuerpo.redireccion ?? "" }

            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Add("Authorization", $"Basic {OneSignalApiKey}");

            var response = await client.PostAsync("https://onesignal.com/api/v1/notifications", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Ok(JsonDocument.Parse(result));
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, error);
            }

        }

        public class NotificacionDto
        {
            public int idencabezado { get; set; }

            public string? Titulo { get; set; }
            public string? Mensaje { get; set; }
            public string? redireccion { get; set; }
            public int evento { get; set; }
            public DateTime fecha_evento { get; set; }
            public string? usuario { get; set; }
            //public IEnumerable<string>? idSuscripcion { get; set; }
            public string? usuarioNotificar { get; set; }

        }
    }
}
