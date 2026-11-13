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

                var historial = (payload.Historial ?? new List<JhonyHistorialItem>())
                    .Select(h => new { rol = h.Rol, texto = h.Texto.Trim() });
                var json = JsonSerializer.Serialize(new
                {
                    message = payload.Mensaje.Trim(),
                    historial,
                    contexto_anterior = NormalizarContexto(payload.ContextoAnterior)
                });
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

        // Acepta el contexto solo si es un objeto JSON válido y lo regresa compacto; si no, se descarta.
        private static string? NormalizarContexto(string? contexto)
        {
            if (string.IsNullOrWhiteSpace(contexto)) return null;
            try
            {
                using var documento = JsonDocument.Parse(contexto);
                return documento.RootElement.ValueKind == JsonValueKind.Object
                    ? JsonSerializer.Serialize(documento.RootElement)
                    : null;
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }

    public class JhonyPreguntaBody
    {
        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Mensaje { get; set; } = string.Empty;

        // Mensajes previos de la conversación (contexto para preguntas de seguimiento).
        [MaxLength(10)]
        public List<JhonyHistorialItem> Historial { get; set; } = new();

        // JSON interpretado de la pregunta anterior (lo genera n8n y el front lo regresa).
        [StringLength(4000)]
        public string? ContextoAnterior { get; set; }
    }

    public class JhonyHistorialItem
    {
        [Required]
        [RegularExpression("^(usuario|jhony)$")]
        public string Rol { get; set; } = string.Empty;

        [Required]
        [StringLength(2000, MinimumLength = 1)]
        public string Texto { get; set; } = string.Empty;
    }
}
