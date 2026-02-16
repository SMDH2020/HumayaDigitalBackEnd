using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HD_Ventas.Consultas
{
    public class Conexion_Servicio_Mensajeria
    {
        public static async Task<string> send(string endpoint, object data)
        {
            string url = "https://53df-187-237-186-166.ngrok-free.app/" + endpoint;

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (a, b, c, d) => true
            };

            using (HttpClient client = new HttpClient(handler))
            {
                try
                {
                    client.DefaultRequestHeaders.Add("x-api-key", "A_chcuch1t4_l4_v0ls340n*2025?");
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("PostmanRuntime/7.32.2");

                    // Convertir a JSON
                    string json = JsonSerializer.Serialize(data);

                    // Crear contenido HTTP
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    //var content = new StringContent("{}", Encoding.UTF8, "application/json");

                    // Enviar POST
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    // Validar éxito
                    response.EnsureSuccessStatusCode();

                    // Leer respuesta
                    string resultado = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Respuesta de la API: " + resultado);
                    return resultado;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("Error al consultar API: " + ex.Message);
                    return null;
                }

            }
        }
    }
}
