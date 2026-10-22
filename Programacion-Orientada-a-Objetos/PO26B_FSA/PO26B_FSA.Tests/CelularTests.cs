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
        // El constructor guarda marca y modelo en el campo correcto.
        //---------------------------------------------------------------------
        [TestMethod]
        public void Constructor_GuardaMarcaYModelo()
        {
            Celular celular = new Celular("Android", "Galaxy", "Samsung");

            Assert.AreEqual("Samsung", celular.GetMarca());
            Assert.AreEqual("Galaxy", celular.GetModelo());
        }

        //---------------------------------------------------------------------
        // El celular empieza apagado y Encender/Apagar cambian su estado,
        // aunque se llamen dos veces seguidas.
        //---------------------------------------------------------------------
        [TestMethod]
        public void EncenderYApagar_CambianElEstado()
        {
            Celular celular = new Celular("Android", "Galaxy", "Samsung");
            Assert.IsFalse(celular.GetEncendido());

            celular.Encender();
            celular.Encender();
            Assert.IsTrue(celular.GetEncendido());

            celular.Apagar();
            celular.Apagar();
            Assert.IsFalse(celular.GetEncendido());
        }
    }
}
