using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Jhony
{
    [ApiController]
    [Route("api/[controller]")]
    public class JhonyController : MyBase
    {
        private const string N8N_JHONY_URL = "http://192.168.0.147:5678/webhook/jhony";
        private const string MENSAJE_ERROR = "Jhony no está disponible en este momento. Intenta de nuevo en unos minutos.";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<JhonyController> _logger;

        public JhonyController(IHttpClientFactory httpClientFactory, ILogger<JhonyController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpPost("Preguntar")]
        public async Task<IActionResult> Preguntar(JhonyPreguntaBody payload)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { ok = false, respuesta = "Escribe una pregunta de máximo 500 caracteres." });
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromSeconds(90);

                var json = JsonSerializer.Serialize(new { message = payload.Mensaje.Trim() });
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");

                var respuesta = await client.PostAsync(N8N_JHONY_URL, contenido);
                if (!respuesta.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Jhony: n8n respondió {Status}: {Cuerpo}", (int)respuesta.StatusCode, await respuesta.Content.ReadAsStringAsync());
                    return StatusCode(502, new { ok = false, respuesta = MENSAJE_ERROR });
                }

                var cuerpo = await respuesta.Content.ReadAsStringAsync();
                return new ContentResult
                {
                    Content = cuerpo,
                    ContentType = "application/json",
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Jhony: error al llamar a n8n");
                return StatusCode(502, new { ok = false, respuesta = MENSAJE_ERROR });
            }
        }
    }

    public class JhonyPreguntaBody
    {
        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Mensaje { get; set; } = string.Empty;
    }
}
