using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PO26B_FSA
{
    //-------------------------------------------------------------------------
    // Envía una pregunta a la API de Gemini y devuelve el texto de la respuesta.
    //-------------------------------------------------------------------------
    internal class CClienteGemini
    {
        //---------------------------------------------------------------------
        // Atributos.
        //---------------------------------------------------------------------
        private const string Modelo = "gemini-3.6-flash";
        private const string UrlBase = "https://generativelanguage.googleapis.com/v1beta/models/";

        // Una sola instancia para toda la app: HttpClient está pensado para reutilizarse.
        private static readonly HttpClient http = new HttpClient();

        //---------------------------------------------------------------------
        // Envía la pregunta y devuelve el texto de la respuesta.
        //---------------------------------------------------------------------
        public async Task<string> EnviarAsync(string pregunta)
        {
            string clave = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
            if (string.IsNullOrEmpty(clave))
            {
                throw new InvalidOperationException("Falta la variable de entorno GEMINI_API_KEY.");
            }

            // Misma forma que la petición de PowerShell de la Fase 0.
            var cuerpo = new
            {
                contents = new[]
                {
                    new { role = "user", parts = new[] { new { text = pregunta } } }
                }
            };
            string json = JsonConvert.SerializeObject(cuerpo);

            using (var peticion = new HttpRequestMessage(HttpMethod.Post, UrlBase + Modelo + ":generateContent"))
            {
                peticion.Headers.Add("x-goog-api-key", clave);
                peticion.Content = new StringContent(json, Encoding.UTF8, "application/json");

                using (HttpResponseMessage respuesta = await http.SendAsync(peticion))
                {
                    string texto = await respuesta.Content.ReadAsStringAsync();
                    if (!respuesta.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException("Error " + (int)respuesta.StatusCode + ": " + texto);
                    }

                    // Mismo camino que en PowerShell: candidates[0].content.parts[0].text
                    JObject datos = JObject.Parse(texto);
                    return (string)datos["candidates"][0]["content"]["parts"][0]["text"];
                }
            }
        }
    }
}
