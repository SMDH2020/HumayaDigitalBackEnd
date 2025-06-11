using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HD.Endpoints.Controllers.Eventos
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacionesController : ControllerBase
    {
        private const string OneSignalAppId = "04e611d6-045a-4105-af2d-04880d3c4cb9"; // Tu App ID
        private const string OneSignalApiKey = "os_v2_app_attbdvqeljaqllznasea2pcmxgflnvyahosesendbr7qoaleizbkfh73lqkpbwxfdb53m3f5gjlmcme6bhgtbrczxvi5uyvlbqnrwvy"; // ⚠️ Tu REST API Key

        [HttpPost("enviar")]
        public async Task<IActionResult> EnviarNotificacion([FromBody] NotificacionDto data)
        {
            using var client = new HttpClient();

            var payload = new
            {
                app_id = OneSignalAppId,
                included_segments = new[] { "All" },
                headings = new { en = data.Titulo ?? "Título por defecto" },
                contents = new { en = data.Mensaje ?? "Mensaje por defecto" },
                data = new { targetPage = data.redireccion ?? "" }

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
            public string? Titulo { get; set; }
            public string? Mensaje { get; set; }
            public string? redireccion { get; set; }
        }
    }
}
