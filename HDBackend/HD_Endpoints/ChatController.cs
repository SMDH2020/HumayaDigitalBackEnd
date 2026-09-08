using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace HD_Endpoints.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        [HttpPost]
        [Route("Enviar")]
        public async Task<IActionResult> EnviarMensaje([FromBody] ChatRequest request)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "true");

                var payload = JsonSerializer.Serialize(new
                {
                    message = request.Message,
                    historial = request.Historial
                });

                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(
                   "https://theme-sailing-psychological-showers.trycloudflare.com/webhook/humaya-chat",
                    content
                );

                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; }
        public List<object> Historial { get; set; }
    }
}