using System.Text;
using System.Text.Json;
using HD.Endpoints.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.RedesSociales
{
    // Reenvía las peticiones de generación de imagen a n8n desde el servidor,
    // evitando que el navegador del cliente llame directo a la IP interna por HTTP.
    [ApiController]
    [Route("api/[controller]")]
    public class RedesWebhookController : MyBase
    {
        private const string N8N_BASE_URL = "http://192.168.0.147:5678/webhook/";
        private readonly IHttpClientFactory _httpClientFactory;

        public RedesWebhookController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("GenerarImagen")]
        public Task<IActionResult> GenerarImagen([FromBody] JsonElement payload)
            => ReenviarAN8n("redes-generar-imagen", payload);

        [HttpPost("ConfirmarImagen")]
        public Task<IActionResult> ConfirmarImagen([FromBody] JsonElement payload)
            => ReenviarAN8n("redes-confirmar-imagen", payload);

        private async Task<IActionResult> ReenviarAN8n(string ruta, JsonElement payload)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromSeconds(120);

                var contenido = new StringContent(payload.GetRawText(), Encoding.UTF8, "application/json");
                var respuesta = await client.PostAsync($"{N8N_BASE_URL}{ruta}", contenido);
                var cuerpo = await respuesta.Content.ReadAsStringAsync();

                return new ContentResult
                {
                    Content = cuerpo,
                    ContentType = "application/json",
                    StatusCode = (int)respuesta.StatusCode
                };
            }
            catch (Exception)
            {
                return StatusCode(502, new
                {
                    ok = false,
                    mensaje = "No se pudo conectar con el servicio de generación de imágenes. Verifica que el equipo de n8n esté encendido y conectado a la red."
                });
            }
        }
    }
}
