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

        //---------------------------------------------------------------------
        // CPersona sobrescribe Respirar de CAnimal, y CHombre / CMujer
        // heredan la versión de CPersona. Se llama a través de una variable
        // CAnimal para comprobar que se elige el método según el objeto.
        //---------------------------------------------------------------------
        [TestMethod]
        public void Respirar_UsaLaVersionDeLaClaseDelObjeto()
        {
            CAnimal animal = new CAnimal(30, 30, 1);
            CAnimal persona = new CPersona(30, 30, 1);
            CAnimal hombre = new CHombre(30, 30);
            CAnimal mujer = new CMujer(30, 30);

            Assert.AreEqual("Animal respirando por instinto", animal.Respirar());
            Assert.AreEqual("Persona respirando de forma consciente", persona.Respirar());
            Assert.AreEqual(persona.Respirar(), hombre.Respirar());
            Assert.AreEqual(persona.Respirar(), mujer.Respirar());
        }
    }
}
