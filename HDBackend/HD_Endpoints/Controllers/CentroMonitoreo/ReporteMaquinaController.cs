using System.Text;
using System.Text.Json;
using HD.Security;
using HD_CentroMonitoreo.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers
{
    public class ReporteMaquinaController : MyBase
    {
        private const string N8N_REPORTE_MAQUINA_URL = "http://192.168.0.147:5678/webhook/reporte-maquina-pdf";
        private const string MENSAJE_ERROR = "No fue posible generar el reporte PDF en este momento. Intenta de nuevo en unos minutos.";
        private const int MAX_DIAS_PERIODO = 186;

        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ReporteMaquinaController> _logger;

        public ReporteMaquinaController(IConfiguration configuration, ISesion sesion, IHttpClientFactory httpClientFactory, ILogger<ReporteMaquinaController> logger)
        {
            Configuracion = configuration;
            Sesion = sesion;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> GenerarPdf([FromQuery] mdl_ReporteMaquinaPdf parametros)
        {
            DateTime desde = parametros.desde!.Value.Date;
            DateTime hasta = parametros.hasta!.Value.Date;

            if (desde > hasta)
                return BadRequest(new { mensaje = "La fecha inicio no puede ser posterior a la fecha fin." });
            if ((hasta - desde).TotalDays + 1 > MAX_DIAS_PERIODO)
                return BadRequest(new { mensaje = "El periodo máximo para el reporte es de 6 meses. Reduce el rango de fechas." });

            try
            {
                var client = _httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromSeconds(240);

                var json = JsonSerializer.Serialize(new
                {
                    jd_machine_id = parametros.jd_machine_id,
                    desde = desde.ToString("yyyy-MM-dd"),
                    hasta = hasta.ToString("yyyy-MM-dd")
                });
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");

                var respuesta = await client.PostAsync(N8N_REPORTE_MAQUINA_URL, contenido);
                var cuerpo = await respuesta.Content.ReadAsStringAsync();

                if (respuesta.StatusCode == System.Net.HttpStatusCode.BadRequest || respuesta.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return StatusCode((int)respuesta.StatusCode, new { mensaje = LeerMensaje(cuerpo) ?? MENSAJE_ERROR });

                if (!respuesta.IsSuccessStatusCode)
                {
                    _logger.LogWarning("ReporteMaquina: n8n respondió {Status}: {Cuerpo}", (int)respuesta.StatusCode, cuerpo);
                    return StatusCode(502, new { mensaje = MENSAJE_ERROR });
                }

                using var documento = JsonDocument.Parse(cuerpo);
                var raiz = documento.RootElement;
                string? pdfBase64 = raiz.TryGetProperty("documento", out var doc) ? doc.GetString() : null;
                if (string.IsNullOrWhiteSpace(pdfBase64))
                {
                    _logger.LogWarning("ReporteMaquina: n8n no regresó el documento.");
                    return StatusCode(502, new { mensaje = MENSAJE_ERROR });
                }

                string nombre = raiz.TryGetProperty("nombredocumento", out var nom) && !string.IsNullOrWhiteSpace(nom.GetString())
                    ? nom.GetString()!
                    : $"reporte_maquina_{parametros.jd_machine_id}_{desde:yyyy-MM-dd}_al_{hasta:yyyy-MM-dd}";

                return Ok(new { documento = pdfBase64, nombredocumento = nombre, extension = "pdf" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ReporteMaquina: error al llamar a n8n");
                return StatusCode(502, new { mensaje = MENSAJE_ERROR });
            }
        }

        private static string? LeerMensaje(string cuerpo)
        {
            try
            {
                using var documento = JsonDocument.Parse(cuerpo);
                return documento.RootElement.TryGetProperty("mensaje", out var mensaje) ? mensaje.GetString() : null;
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }
}
