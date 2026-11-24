using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PO26B_FSA
{
    //-------------------------------------------------------------------------
    // Conversación con la API de Gemini: envía la pregunta con el historial
    // y devuelve el texto de la respuesta.
    //-------------------------------------------------------------------------
    internal class CClienteGemini
    {
        //---------------------------------------------------------------------
        // Atributos.
        //---------------------------------------------------------------------
        private const string Modelo = "gemini-3.6-flash";
        private const string UrlBase = "https://generativelanguage.googleapis.com/v1beta/models/";
        private const int MaxElementos = 10;   // 5 pares pregunta-respuesta

        // Una sola instancia para toda la app: HttpClient está pensado para reutilizarse.
        private static readonly HttpClient http = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };

        // Cada elemento es un "content" de la API: { role, parts }. Las respuestas
        // se guardan tal como llegan, con su thoughtSignature.
        private readonly List<JObject> historial = new List<JObject>();

        //---------------------------------------------------------------------
        // Envía la pregunta con el historial y devuelve el texto de la respuesta.
        //---------------------------------------------------------------------
        public async Task<string> EnviarAsync(string pregunta)
        {
            string clave = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
            if (string.IsNullOrEmpty(clave))
            {
                throw new InvalidOperationException("Falta la variable de entorno GEMINI_API_KEY.");
            }

            JObject turnoUsuario = new JObject(
                new JProperty("role", "user"),
                new JProperty("parts", new JArray(new JObject(new JProperty("text", pregunta)))));

            // Se envía una copia: el historial solo cambia si la petición sale bien.
            JArray contenidos = new JArray(historial);
            contenidos.Add(turnoUsuario);
            string json = new JObject(new JProperty("contents", contenidos)).ToString(Formatting.None);

            using (var peticion = new HttpRequestMessage(HttpMethod.Post, UrlBase + Modelo + ":generateContent"))
            {
                peticion.Headers.Add("x-goog-api-key", clave);
                peticion.Content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage respuesta;
                try
                {
                    respuesta = await http.SendAsync(peticion);
                }
                catch (HttpRequestException ex)
                {
                    throw new HttpRequestException("No hay conexión con Gemini. Revisa tu internet.", ex);
                }
                catch (TaskCanceledException ex)
                {
                    // Hipótesis: en .NET Framework, el tiempo agotado llega como TaskCanceledException.
                    throw new TimeoutException("Gemini tardó más de 60 segundos. Inténtalo de nuevo.", ex);
                }

                using (respuesta)
                {
                    string cuerpo = await respuesta.Content.ReadAsStringAsync();
                    if (!respuesta.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException(MensajeDeError((int)respuesta.StatusCode, cuerpo));
                    }

                    JObject contenidoModelo = ExtraerContenido(JObject.Parse(cuerpo));
                    AgregarTurno(turnoUsuario, contenidoModelo);
                    return TextoDe(contenidoModelo);
                }
            }
        }

        //---------------------------------------------------------------------
        // Historial: se añade por pares y se recorta por pares desde el inicio.
        //---------------------------------------------------------------------
        internal void AgregarTurno(JObject usuario, JObject modelo)
        {
            historial.Add(usuario);
            historial.Add(modelo);
            while (historial.Count > MaxElementos)
            {
                historial.RemoveRange(0, 2);   // por pares: nunca empieza con "model"
            }
        }

        internal IReadOnlyList<JObject> GetHistorial()
        {
            return historial;
        }

        //---------------------------------------------------------------------
        // Lectura de la respuesta: el "content" del modelo o un error claro.
        //---------------------------------------------------------------------
        internal static JObject ExtraerContenido(JObject datos)
        {
            string bloqueo = (string)datos.SelectToken("promptFeedback.blockReason");
            if (bloqueo != null)
            {
                throw new InvalidOperationException("Gemini bloqueó la pregunta (" + bloqueo + "). Reformúlala.");
            }

            JObject contenido = datos.SelectToken("candidates[0].content") as JObject;
            if (contenido == null || TextoDe(contenido).Length == 0)
            {
                string motivo = (string)datos.SelectToken("candidates[0].finishReason") ?? "desconocido";
                throw new InvalidOperationException("Gemini no devolvió texto (motivo: " + motivo + ").");
            }
            return contenido;
        }

        internal static string TextoDe(JObject contenido)
        {
            var texto = new StringBuilder();
            foreach (JToken parte in contenido["parts"] ?? new JArray())
            {
                texto.Append((string)parte["text"]);   // una parte sin "text" aporta null: no suma nada
            }
            return texto.ToString();
        }

        //---------------------------------------------------------------------
        // Traduce un error HTTP de la API a un mensaje claro.
        //---------------------------------------------------------------------
        internal static string MensajeDeError(int codigo, string cuerpo)
        {
            string razon = null;
            string detalle = null;
            try
            {
                JObject datos = JObject.Parse(cuerpo);
                detalle = (string)datos.SelectToken("error.message");
                razon = (string)datos.SelectTokens("error.details[*].reason").FirstOrDefault();
            }
            catch (JsonReaderException)
            {
                // El cuerpo no era JSON: se clasifica solo por el código.
            }

            if (razon == "API_KEY_INVALID")
            {
                return "La clave de Gemini no es válida. Revisa GEMINI_API_KEY.";
            }
            switch (codigo)
            {
                case 429:
                    return "Se alcanzó el límite de Gemini (5 por minuto o 20 por día). Espera un minuto; "
                         + "si sigue, la cuota se reinicia a medianoche, hora del Pacífico.";
                case 500:
                case 503:
                case 504:
                    return "Gemini está saturado o falló por un momento. Inténtalo de nuevo en unos minutos.";
                case 403:
                    return "La clave no tiene permiso para usar Gemini (403).";
                case 404:
                    return "No se encontró el modelo " + Modelo + " (404); puede haber cambiado de nombre.";
                default:
                    return "Gemini devolvió un error " + codigo + (detalle != null ? ": " + detalle : ".");
            }
        }
    }
}
