using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PO26B_FSA.Tests
{
    [TestClass]
    public class CHombreCMujerTests
    {
        //---------------------------------------------------------------------
        // ToString debe mostrar siempre la misma tasa para el mismo objeto.
        // La pausa evita que new Random() repita la semilla (usa el reloj).
        //---------------------------------------------------------------------
        [TestMethod]
        public void ToString_Hombre_EsEstableEntreLlamadas()
        {
            CHombre hombre = new CHombre(175, 70);
            string primero = hombre.ToString();

            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(20);

                Assert.AreEqual(primero, hombre.ToString());
            }
        }

        [TestMethod]
        public void ToString_Mujer_EsEstableEntreLlamadas()
        {
            CMujer mujer = new CMujer(165, 60);
            string primero = mujer.ToString();

            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(20);

                Assert.AreEqual(primero, mujer.ToString());
            }
        }

        [TestMethod]
        public void ToString_Hombre_NoMuestraTasaEnCero()
        {
            CHombre hombre = new CHombre(175, 70);

            Assert.IsFalse(hombre.ToString().Contains("es: 0 millones"));
        }

        //---------------------------------------------------------------------
        // El valor es aleatorio, así que solo se comprueba el rango.
        //---------------------------------------------------------------------
        [TestMethod]
        public void IniciaProduccionEspermatozoides_EstaEnRango()
        {
            CHombre hombre = new CHombre(175, 70);

            for (int i = 0; i < 50; i++)
            {
                int tasa = hombre.IniciaProduccionEspermatozoides();

                Assert.IsTrue(tasa >= 100000000 && tasa < 170000000, $"Fuera de rango: {tasa}");
            }
        }

        [TestMethod]
        public void IniciaProduccionOvulos_EstaEnRango()
        {
            CMujer mujer = new CMujer(165, 60);

            for (int i = 0; i < 50; i++)
            {
                int tasa = mujer.IniciaProduccionOvulos();

                Assert.IsTrue(tasa >= 1000000 && tasa < 2000000, $"Fuera de rango: {tasa}");
            }
        }
    }
}
