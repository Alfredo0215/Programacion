using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PO26B_FSA.Tests
{
    [TestClass]
    public class CPersonaTests
    {
        //---------------------------------------------------------------------
        // ToString incluye los datos registrados de la persona.
        //---------------------------------------------------------------------
        [TestMethod]
        public void ToString_IncluyeNombreApellidosYMedidas()
        {
            CHombre persona = new CHombre(175, 70.5);
            persona.RegistrarPersona("Alfredo", "Fletes", "Aguilar", new DateTime(2000, 1, 15));

            string texto = persona.ToString();

            StringAssert.Contains(texto, "Alfredo");
            StringAssert.Contains(texto, "Fletes");
            StringAssert.Contains(texto, "Aguilar");
            StringAssert.Contains(texto, "175");
        }

        //---------------------------------------------------------------------
        // El factor reproductivo depende del tipo de persona.
        //---------------------------------------------------------------------
        [TestMethod]
        public void ToString_Hombre_MencionaEspermatozoides()
        {
            CHombre persona = new CHombre(175, 70);

            StringAssert.Contains(persona.ToString(), "espermatozoides");
        }

        [TestMethod]
        public void ToString_Mujer_MencionaOvulos()
        {
            CMujer persona = new CMujer(165, 60);

            StringAssert.Contains(persona.ToString(), "óvulos");
        }
    }
}
