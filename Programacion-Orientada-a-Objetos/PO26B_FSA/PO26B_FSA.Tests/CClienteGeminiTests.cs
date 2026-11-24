using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace PO26B_FSA.Tests
{
    //-------------------------------------------------------------------------
    // Pruebas de CClienteGemini que no usan la red ni gastan cuota.
    //-------------------------------------------------------------------------
    [TestClass]
    public class CClienteGeminiTests
    {
        //---------------------------------------------------------------------
        // Crea un "content" de la API con un solo texto.
        //---------------------------------------------------------------------
        private static JObject Turno(string rol, string texto)
        {
            return JObject.Parse("{ \"role\": \"" + rol + "\", \"parts\": [ { \"text\": \"" + texto + "\" } ] }");
        }

        //---------------------------------------------------------------------
        // Agrega n pares numerados desde 1: "pregunta i" / "respuesta i".
        //---------------------------------------------------------------------
        private static void AgregarPares(CClienteGemini cliente, int n)
        {
            for (int i = 1; i <= n; i++)
            {
                cliente.AgregarTurno(Turno("user", "pregunta " + i), Turno("model", "respuesta " + i));
            }
        }

        //---------------------------------------------------------------------
        // Historial.
        //---------------------------------------------------------------------
        [TestMethod]
        public void AgregarTurno_SeisPares_ConservaDiezElementos()
        {
            CClienteGemini cliente = new CClienteGemini();

            AgregarPares(cliente, 6);

            Assert.AreEqual(10, cliente.GetHistorial().Count);
        }

        [TestMethod]
        public void AgregarTurno_AlRecortar_EliminaElParMasAntiguo()
        {
            CClienteGemini cliente = new CClienteGemini();

            AgregarPares(cliente, 6);

            Assert.AreEqual("pregunta 2", CClienteGemini.TextoDe(cliente.GetHistorial()[0]));
            Assert.AreEqual("respuesta 6", CClienteGemini.TextoDe(cliente.GetHistorial()[9]));
        }

        [TestMethod]
        public void AgregarTurno_AlRecortar_EmpiezaConUser()
        {
            CClienteGemini cliente = new CClienteGemini();

            for (int pares = 1; pares <= 8; pares++)
            {
                cliente.AgregarTurno(Turno("user", "p"), Turno("model", "r"));
                Assert.AreEqual("user", (string)cliente.GetHistorial()[0]["role"]);
            }
        }

        //---------------------------------------------------------------------
        // Una petición que falla no deja la pregunta en el historial. La
        // clave se quita solo en este proceso y se restaura al final.
        //---------------------------------------------------------------------
        [TestMethod]
        public async Task EnviarAsync_SinClave_NoCambiaElHistorial()
        {
            string claveOriginal = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
            try
            {
                Environment.SetEnvironmentVariable("GEMINI_API_KEY", null);
                CClienteGemini cliente = new CClienteGemini();
                AgregarPares(cliente, 1);

                await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => cliente.EnviarAsync("hola"));

                Assert.AreEqual(2, cliente.GetHistorial().Count);
            }
            finally
            {
                Environment.SetEnvironmentVariable("GEMINI_API_KEY", claveOriginal);
            }
        }

        //---------------------------------------------------------------------
        // Lectura de la respuesta.
        //---------------------------------------------------------------------
        [TestMethod]
        public void ExtraerContenido_PreguntaBloqueada_LanzaExcepcion()
        {
            JObject datos = JObject.Parse("{ \"promptFeedback\": { \"blockReason\": \"SAFETY\" } }");

            InvalidOperationException ex = Assert.ThrowsException<InvalidOperationException>(
                () => CClienteGemini.ExtraerContenido(datos));

            StringAssert.Contains(ex.Message, "SAFETY");
        }

        [TestMethod]
        public void ExtraerContenido_SinTexto_LanzaExcepcion()
        {
            JObject datos = JObject.Parse("{ \"candidates\": [ { \"finishReason\": \"MAX_TOKENS\" } ] }");

            InvalidOperationException ex = Assert.ThrowsException<InvalidOperationException>(
                () => CClienteGemini.ExtraerContenido(datos));

            StringAssert.Contains(ex.Message, "MAX_TOKENS");
        }

        [TestMethod]
        public void ExtraerContenido_ConFirma_ConservaThoughtSignature()
        {
            JObject datos = JObject.Parse(
                "{ \"candidates\": [ { \"content\": { \"role\": \"model\", \"parts\": [ "
                + "{ \"text\": \"Hola\", \"thoughtSignature\": \"firma123\" } ] }, \"finishReason\": \"STOP\" } ] }");

            JObject contenido = CClienteGemini.ExtraerContenido(datos);

            Assert.AreEqual("firma123", (string)contenido["parts"][0]["thoughtSignature"]);
            Assert.AreEqual("Hola", CClienteGemini.TextoDe(contenido));
        }

        //---------------------------------------------------------------------
        // Mensajes de error.
        //---------------------------------------------------------------------
        [TestMethod]
        public void MensajeDeError_ClaveInvalida_HablaDeLaClave()
        {
            // Respuesta real de la API con una clave inválida (Notas de la Fase 0).
            string cuerpo = @"{
  ""error"": {
    ""code"": 400,
    ""message"": ""API key not valid. Please pass a valid API key."",
    ""status"": ""INVALID_ARGUMENT"",
    ""details"": [
      {
        ""@type"": ""type.googleapis.com/google.rpc.ErrorInfo"",
        ""reason"": ""API_KEY_INVALID"",
        ""domain"": ""googleapis.com"",
        ""metadata"": { ""service"": ""generativelanguage.googleapis.com"" }
      },
      {
        ""@type"": ""type.googleapis.com/google.rpc.LocalizedMessage"",
        ""locale"": ""en-US"",
        ""message"": ""API key not valid. Please pass a valid API key.""
      }
    ]
  }
}";

            string mensaje = CClienteGemini.MensajeDeError(400, cuerpo);

            StringAssert.Contains(mensaje, "clave");
        }

        [TestMethod]
        public void MensajeDeError_429_HablaDelLimite()
        {
            string mensaje = CClienteGemini.MensajeDeError(429, "{ \"error\": { \"status\": \"RESOURCE_EXHAUSTED\" } }");

            StringAssert.Contains(mensaje, "límite");
        }

        [TestMethod]
        public void MensajeDeError_503_InvitaAReintentar()
        {
            string mensaje = CClienteGemini.MensajeDeError(503, "{ \"error\": { \"status\": \"UNAVAILABLE\" } }");

            StringAssert.Contains(mensaje, "Inténtalo de nuevo");
        }

        [TestMethod]
        public void MensajeDeError_CuerpoNoJson_NoLanzaExcepcion()
        {
            string mensaje = CClienteGemini.MensajeDeError(502, "<html>Bad Gateway</html>");

            StringAssert.Contains(mensaje, "502");
        }
    }
}
