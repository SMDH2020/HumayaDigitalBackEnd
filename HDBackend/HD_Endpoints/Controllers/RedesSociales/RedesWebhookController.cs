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
        public async Task<IActionResult> GenerarImagen(RedesSocialesBody payload)
        {
            //EscribirLog("CONTROLLER GENERAR IMAGEN ");
            //EscribirLog("Payload: " + payload.ToString());

            //return Ok(payload);
            return await ReenviarAN8n("redes-generar-imagen", payload);
        }

        [HttpPost("ConfirmarImagen")]
        public async Task<IActionResult> ConfirmarImagen(RedesSocialesBody payload)
        {
            return await ReenviarAN8n("redes-confirmar-imagen", payload);
        }

        private async Task<IActionResult> ReenviarAN8n(
    string ruta,
    RedesSocialesBody payload)
        {
            try
            {
                //EscribirLog($"INICIO ReenviarAN8n");
                //EscribirLog($"Ruta: {ruta}");
                //EscribirLog($"ValueKind: {payload.Folio}");

                if (string.IsNullOrWhiteSpace(payload.Folio))
                {
                    //EscribirLog("ERROR: payload está Undefined");

                    return BadRequest(new
                    {
                        ok = false,
                        mensaje = "El servidor no recibió el contenido JSON."
                    });
                }

                var json = JsonSerializer.Serialize(payload);

                //EscribirLog($"Payload recibido: {json}");

                var url = $"{N8N_BASE_URL}{ruta}";

                //EscribirLog($"URL n8n: {url}");

                var client = _httpClientFactory.CreateClient();

                client.Timeout = TimeSpan.FromSeconds(120);

                //EscribirLog("HttpClient creado");
                //EscribirLog("Enviando petición a n8n...");

                var contenido = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                var respuesta = await client.PostAsync(
                    url,
                    contenido);

                //EscribirLog(
                //    $"Respuesta n8n: {(int)respuesta.StatusCode} {respuesta.StatusCode}");

                var cuerpo = await respuesta.Content.ReadAsStringAsync();

                //EscribirLog($"Respuesta n8n: {cuerpo}");

                //EscribirLog("FIN OK");

                return new ContentResult
                {
                    Content = cuerpo,
                    ContentType = "application/json",
                    StatusCode = (int)respuesta.StatusCode
                };
            }
            catch (Exception ex)
            {
                //EscribirLog("EXCEPCIÓN:");
                //EscribirLog(ex.ToString());

                return StatusCode(502, new
                {
                    ok = false,
                    mensaje = "Error comunicando con n8n.",
                    error = ex.Message
                });
            }
        }
        //    private async Task<IActionResult> ReenviarAN8n(
        //string ruta,
        //JsonElement payload)
        //    {
        //        try
        //        {
        //            var url = $"{N8N_BASE_URL}{ruta}";

        //            EscribirLog($"URL n8n: {url}");
        //            EscribirLog($"Payload: {payload.GetRawText()}");

        //            var client = _httpClientFactory.CreateClient();

        //            client.Timeout = TimeSpan.FromSeconds(120);

        //            EscribirLog("HttpClient creado");
        //            EscribirLog("Enviando petición a n8n...");

        //            var contenido = new StringContent(
        //                payload.GetRawText(),
        //                Encoding.UTF8,
        //                "application/json");

        //            var respuesta = await client.PostAsync(url, contenido);

        //            EscribirLog(
        //                $"Respuesta n8n: {(int)respuesta.StatusCode} {respuesta.StatusCode}");

        //            var cuerpo = await respuesta.Content.ReadAsStringAsync();

        //            EscribirLog($"Respuesta body: {cuerpo}");

        //            EscribirLog("FIN OK");

        //            return new ContentResult
        //            {
        //                Content = cuerpo,
        //                ContentType = "application/json",
        //                StatusCode = (int)respuesta.StatusCode
        //            };
        //        }
        //        catch (Exception ex)
        //        {
        //            EscribirLog("ERROR");
        //            EscribirLog(ex.ToString());

        //            return StatusCode(502, new
        //            {
        //                ok = false,
        //                mensaje = "No se pudo conectar con n8n.",
        //                error = ex.Message
        //            });
        //        }
        //    }

        //private async Task<IActionResult> ReenviarAN8n(string ruta, JsonElement payload)
        //{
        //    try
        //    {
        //        if (System.IO.File.Exists(@"C:\\SMDH\\Log.txt")) {
        //            System.IO.File.Delete(@"C:\\SMDH\\Log.txt");
        //        }


        //        var client = _httpClientFactory.CreateClient();
        //        client.Timeout = TimeSpan.FromSeconds(120);

        //        var contenido = new StringContent(payload.GetRawText(), Encoding.UTF8, "application/json");
        //        var respuesta = await client.PostAsync($"{N8N_BASE_URL}{ruta}", contenido);
        //        var cuerpo = await respuesta.Content.ReadAsStringAsync();

        //        return new ContentResult
        //        {
        //            Content = cuerpo,
        //            ContentType = "application/json",
        //            StatusCode = (int)respuesta.StatusCode
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(502, new
        //        {
        //            ok = false,
        //            mensaje = "No se pudo conectar con el servicio de generación de imágenes. Verifica que el equipo de n8n esté encendido y conectado a la red. Exceptio: " + ex.InnerException.Message+ " MEnsajes: " + ex.Message
        //        });
        //    }
        //}
        private static void EscribirLog(string mensaje)
        {
            try
            {
                string LOG_FILE = @"C:\SMDH\Log.txt";
                var directorio = Path.GetDirectoryName(LOG_FILE);

                if (!string.IsNullOrEmpty(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                var linea =
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} | {mensaje}";

                System.IO.File.AppendAllText(
                    LOG_FILE,
                    linea + Environment.NewLine);
            }
            catch
            {
                // No permitir que un error del log rompa la API
            }
        }
    }
    public class RedesSocialesBody
    {
        public string Folio { get; set; }
        public string ImagenGeneradaBase64 { get; set; }
        public string CopyGenerado { get; set; }
    }
    public class RedesSocialesBodyConfirmar
    {
        public string Folio { get; set; }
        public string ImagenGeneradaBase64 { get; set; }
        public string CopyGenerado { get; set; }
    }
}
