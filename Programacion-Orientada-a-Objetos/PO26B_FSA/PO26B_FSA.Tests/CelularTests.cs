using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PO26B_FSA.Tests
{
    [TestClass]
    public class CelularTests
    {
        //---------------------------------------------------------------------
        // El constructor guarda el sistema operativo.
        //---------------------------------------------------------------------
        [TestMethod]
        public void Constructor_GuardaSistemaOperativo()
        {
            Celular celular = new Celular("Android", "Galaxy", "Samsung");

            Assert.AreEqual("Android", celular.GetSistemaOperativo());
        }

        //---------------------------------------------------------------------
        // Las subclases fijan su propio sistema operativo.
        //---------------------------------------------------------------------
        [TestMethod]
        public void CelularAndroid_UsaSistemaOperativoAndroid()
        {
            CelularAndroid celular = new CelularAndroid("Galaxy", "Samsung");

            Assert.AreEqual("Android", celular.GetSistemaOperativo());
        }

        [TestMethod]
        public void CelularIos_UsaSistemaOperativoIos()
        {
            CelularIos celular = new CelularIos("iPhone", "Apple");

            Assert.AreEqual("Ios", celular.GetSistemaOperativo());
        }

        //---------------------------------------------------------------------
        // Encender y apagar se pueden repetir sin fallar.
        //---------------------------------------------------------------------
        [TestMethod]
        public void EncenderYApagar_RepetidosNoLanzanExcepcion()
        {
            Celular celular = new Celular("Android", "Galaxy", "Samsung");

            celular.Encender();
            celular.Encender();
            celular.Apagar();
            celular.Apagar();
        }
    }
}
