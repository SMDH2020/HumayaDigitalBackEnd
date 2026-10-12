using HD.Clientes.Consultas.SolicitudCredito;
using HD.Clientes.Modelos;
using HD.Notifications.Analisis;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace HD.Endpoints.Controllers.Zoho
{
    public class ClienteZohoController : MyBase
    {

        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        private readonly HttpClient _httpClient;
        public ClienteZohoController(IConfiguration configuration, ISesion sesion, HttpClient httpClient)
        {
            Configuracion = configuration;
            Sesion = sesion;
            _httpClient = httpClient;
        }

        private string ExtractAccessToken(string tokenResponseJson)
        {
            // Usa System.Text.Json para deserializar el JSON
            using (JsonDocument doc = JsonDocument.Parse(tokenResponseJson))
            {
                if (doc.RootElement.TryGetProperty("access_token", out JsonElement tokenElement))
                {
                    return tokenElement.GetString();
                }
                throw new InvalidOperationException("Access token not found in the response.");
            }
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> GetAccounts()
        {
            try
            {
                // Configura la solicitud hacia la API de Zoho
                var request2 = new HttpRequestMessage(HttpMethod.Post, "https://accounts.zoho.com/oauth/v2/token");
                var requestData = new Dictionary<string, string>
                {
                    { "refresh_token", "1000.d55c499c98b601b58763b7815843faa7.cbf6697ee84a76819de21ef8acb00255" },
                    { "client_id", "1000.ZXMYYJCATRKK0QEJ1EX25DLC3Z6ZWU" },
                    { "client_secret", "ce643100a3f3cbd1ae2ddd9e113d0aa95ddd7c0054" },
                    { "grant_type", "refresh_token" }
                };
                request2.Content = new FormUrlEncodedContent(requestData);
                var tokenResponse = await _httpClient.SendAsync(request2);

                if (!tokenResponse.IsSuccessStatusCode)
                {
                    return StatusCode((int)tokenResponse.StatusCode, await tokenResponse.Content.ReadAsStringAsync());
                }

                var tokenData = await tokenResponse.Content.ReadAsStringAsync();
                var accessToken = ExtractAccessToken(tokenData);

                // Configura la solicitud hacia la API de Zoho
                var request = new HttpRequestMessage(HttpMethod.Get, "https://www.zohoapis.com/crm/v2/Accounts");
                request.Headers.Authorization = new AuthenticationHeaderValue("Zoho-oauthtoken", accessToken); // Reemplaza con tu token

                // Realiza la solicitud
                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
                }

                // Devuelve la respuesta al cliente
                var data = await response.Content.ReadAsStringAsync();
                return Content(data, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
