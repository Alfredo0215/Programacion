using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PO26B_FSA.Acuario;

namespace PO26B_FSA.Tests
{
    [TestClass]
    public class CEcosistemaTests
    {
        //---------------------------------------------------------------------
        // Ninguna entidad nace fuera del área. Se repite muchas veces porque
        // las posiciones son aleatorias y una sola corrida podría pasar por
        // suerte.
        //---------------------------------------------------------------------
        [TestMethod]
        public void PosicionInicial_SiempreDentroDelArea()
        {
            const float ancho = 300f;
            const float alto = 200f;

            for (int repeticion = 0; repeticion < 100; repeticion++)
            {
                CEcosistema ecosistema = new CEcosistema(ancho, alto);

                foreach (CEntidad entidad in ecosistema.GetEntidades())
                {
                    RectangleF r = entidad.GetLimites();
                    Assert.IsTrue(r.Left >= 0 && r.Top >= 0, "Sale por arriba o por la izquierda.");
                    Assert.IsTrue(r.Right <= ancho && r.Bottom <= alto, "Sale por abajo o por la derecha.");
                }
            }
        }

        //---------------------------------------------------------------------
        // Se crea exactamente la población inicial definida en CParametros.
        //---------------------------------------------------------------------
        [TestMethod]
        public void PoblacionInicial_CoincideConParametros()
        {
            CEcosistema ecosistema = new CEcosistema(300f, 200f);

            int esperado = CParametros.PlantasIniciales
                         + CParametros.HerbivorosIniciales
                         + CParametros.DepredadoresIniciales;

            Assert.AreEqual(esperado, ecosistema.GetEntidades().Count);
        }

        //---------------------------------------------------------------------
        // Un herbívoro pegado al borde derecho y yendo a la derecha (ángulo 0)
        // rebota: queda dentro y su dirección X ya no apunta afuera. Sale igual
        // aunque en ese tick toque un giro, porque la corrección va al final.
        //---------------------------------------------------------------------
        [TestMethod]
        public void PezEnBordeDerecho_RebotaHaciaAdentro()
        {
            const float ancho = 300f;
            const float alto = 200f;
            float x = ancho - CParametros.LadoHerbivoro;
            CHerbivoro pez = new CHerbivoro(x, 100f, 0.0);

            pez.Actualizar(ancho, alto, new Random(1));

            RectangleF r = pez.GetLimites();
            Assert.IsTrue(r.Left >= 0 && r.Right <= ancho, "Salió por un lado.");
            Assert.IsTrue(pez.GetDireccion().X <= 0, "Sigue apuntando hacia afuera.");
        }

        //---------------------------------------------------------------------
        // Con semilla fija el resultado es repetible: en 1000 pasos ningún
        // pez sale del área.
        //---------------------------------------------------------------------
        [TestMethod]
        public void MilPasos_NingunPezSaleDelArea()
        {
            const float ancho = 300f;
            const float alto = 200f;
            Random azar = new Random(1);
            CPez[] peces =
            {
                new CHerbivoro(10f, 10f, 0.5),
                new CHerbivoro(280f, 180f, 2.0),
                new CDepredador(150f, 100f, 4.0)
            };

            for (int paso = 0; paso < 1000; paso++)
            {
                foreach (CPez pez in peces)
                {
                    pez.Actualizar(ancho, alto, azar);

                    RectangleF r = pez.GetLimites();
                    Assert.IsTrue(r.Left >= 0 && r.Top >= 0, $"Paso {paso}: sale por arriba o por la izquierda.");
                    Assert.IsTrue(r.Right <= ancho && r.Bottom <= alto, $"Paso {paso}: sale por abajo o por la derecha.");
                }
            }
        }

        //---------------------------------------------------------------------
        // La planta hereda el Actualizar vacío de CEntidad: no se mueve.
        //---------------------------------------------------------------------
        [TestMethod]
        public void Planta_NoCambiaDePosicion()
        {
            CPlanta planta = new CPlanta(50f, 60f);
            RectangleF antes = planta.GetLimites();

            planta.Actualizar(300f, 200f, new Random(1));

            Assert.AreEqual(antes, planta.GetLimites());
        }

        //---------------------------------------------------------------------
        // Fase 4: energía, comer y morir.
        //---------------------------------------------------------------------
        [TestMethod]
        public void Pez_AlActualizar_GastaEnergia()
        {
            CHerbivoro pez = new CHerbivoro(100f, 100f, 0.0);

            pez.Actualizar(300f, 200f, new Random(1));

            Assert.AreEqual(CParametros.EnergiaInicialHerbivoro - CParametros.GastoPorTick, pez.GetEnergia());
        }

        [TestMethod]
        public void Herbivoro_ComePlanta_SumaEnergiaYPlantaMuere()
        {
            CHerbivoro pez = new CHerbivoro(100f, 100f, 0.0);
            CPlanta planta = new CPlanta(100f, 100f);

            bool comio = pez.Comer(planta);

            Assert.IsTrue(comio);
            Assert.AreEqual(CParametros.EnergiaInicialHerbivoro + CParametros.EnergiaPlanta, pez.GetEnergia());
            Assert.IsFalse(planta.EstaViva());
            Assert.AreEqual(0, planta.GetEnergia());
        }

        [TestMethod]
        public void Herbivoro_NoPuedeComerDepredador()
        {
            CHerbivoro herbivoro = new CHerbivoro(100f, 100f, 0.0);
            CDepredador depredador = new CDepredador(100f, 100f, 0.0);

            bool comio = herbivoro.Comer(depredador);

            Assert.IsFalse(comio);
            Assert.AreEqual(CParametros.EnergiaInicialHerbivoro, herbivoro.GetEnergia());
            Assert.AreEqual(CParametros.EnergiaInicialDepredador, depredador.GetEnergia());
            Assert.IsTrue(depredador.EstaViva());
        }

        [TestMethod]
        public void PresaYaComida_NoSePuedeComerOtraVez()
        {
            CDepredador d1 = new CDepredador(100f, 100f, 0.0);
            CDepredador d2 = new CDepredador(100f, 100f, 0.0);
            CHerbivoro presa = new CHerbivoro(100f, 100f, 0.0);

            Assert.IsTrue(d1.Comer(presa));
            Assert.IsFalse(d2.Comer(presa));
            Assert.AreEqual(CParametros.EnergiaInicialDepredador, d2.GetEnergia());
        }

        [TestMethod]
        public void EnergiaCero_PezMuereYEnergiaNoEsNegativa()
        {
            CHerbivoro pez = new CHerbivoro(100f, 100f, 0.0);
            Random azar = new Random(1);

            for (int i = 0; i < CParametros.EnergiaInicialHerbivoro + 10; i++)
            {
                pez.Actualizar(300f, 200f, azar);
                Assert.IsTrue(pez.GetEnergia() >= 0, $"Tick {i}: energía negativa.");
            }

            Assert.IsFalse(pez.EstaViva());
            Assert.AreEqual(0, pez.GetEnergia());
        }

        [TestMethod]
        public void EdadMaxima_PezMuere()
        {
            CPezDePrueba pez = new CPezDePrueba(energiaInicial: 1000, edadMaxima: 5);
            Random azar = new Random(1);

            for (int i = 0; i < 4; i++)
            {
                pez.Actualizar(300f, 200f, azar);
            }
            Assert.IsTrue(pez.EstaViva(), "Murió antes de la edad máxima.");

            pez.Actualizar(300f, 200f, azar);
            Assert.IsFalse(pez.EstaViva(), "No murió al llegar a la edad máxima.");
            Assert.IsTrue(pez.GetEnergia() > 0, "Murió de hambre, no de viejo.");
        }

        //---------------------------------------------------------------------
        // Fase 5: nacimientos.
        //---------------------------------------------------------------------
        [TestMethod]
        public void BajoUmbral_NoSeReproduce()
        {
            CHerbivoro pez = new CHerbivoro(100f, 100f, 0.0);

            Assert.IsNull(pez.Reproducirse(new Random(1)));
            Assert.AreEqual(CParametros.EnergiaInicialHerbivoro, pez.GetEnergia());
        }

        [TestMethod]
        public void AlReproducirse_PadrePierdeLoQueRecibeLaCria()
        {
            const int energiaAntes = 1500;
            CHerbivoro padre = new CHerbivoro(100f, 100f, 0.0, energiaAntes);

            CEntidad cria = padre.Reproducirse(new Random(1));

            Assert.IsNotNull(cria);
            Assert.AreEqual(CParametros.EnergiaCria, cria.GetEnergia());
            Assert.AreEqual(energiaAntes, padre.GetEnergia() + cria.GetEnergia(), "Se creó o se perdió energía.");
        }

        [TestMethod]
        public void EnEspera_NoSeReproduce()
        {
            CHerbivoro padre = new CHerbivoro(100f, 100f, 0.0, 2500);
            Random azar = new Random(1);

            Assert.IsNotNull(padre.Reproducirse(azar));
            Assert.IsTrue(padre.GetEnergia() >= CParametros.UmbralReproduccion, "La prueba necesita energía sobre el umbral.");
            Assert.IsNull(padre.Reproducirse(azar));
        }

        [TestMethod]
        public void Cria_EsDelMismoTipoYNaceDentroDelArea()
        {
            const float ancho = 300f;
            const float alto = 200f;
            CDepredador padre = new CDepredador(ancho - CParametros.LadoDepredador, alto - CParametros.LadoDepredador, 0.0, 1500);

            CEntidad cria = padre.Reproducirse(new Random(1));

            Assert.IsInstanceOfType(cria, typeof(CDepredador));
            RectangleF r = cria.GetLimites();
            Assert.IsTrue(r.Left >= 0 && r.Top >= 0 && r.Right <= ancho && r.Bottom <= alto, "La cría nació fuera del área.");
        }

        [TestMethod]
        public void Planta_NoSeReproduce()
        {
            CPlanta planta = new CPlanta(50f, 60f);

            Assert.IsNull(planta.Reproducirse(new Random(1)));
        }

        [TestMethod]
        public void Umbral_EsMayorQueEnergiaCria()
        {
            Assert.IsTrue(CParametros.UmbralReproduccion > CParametros.EnergiaCria,
                "Con estos valores el padre podría morir al reproducirse.");
        }

        [TestMethod]
        public void MuchosPasos_PoblacionNuncaSuperaElTope()
        {
            CEcosistema ecosistema = new CEcosistema(300f, 200f);

            for (int paso = 0; paso < 3000; paso++)
            {
                ecosistema.Paso();
                Assert.IsTrue(ecosistema.GetEntidades().Count <= CParametros.PoblacionMaxima,
                    $"Paso {paso}: la población superó el tope.");
            }
        }

        //---------------------------------------------------------------------
        // Fase 6: conteos, agregar y tiempo.
        //---------------------------------------------------------------------
        [TestMethod]
        public void Contar_CoincideConPoblacionInicial()
        {
            CEcosistema ecosistema = new CEcosistema(300f, 200f);

            Assert.AreEqual(CParametros.PlantasIniciales, ecosistema.Contar<CPlanta>());
            Assert.AreEqual(CParametros.HerbivorosIniciales, ecosistema.Contar<CHerbivoro>());
            Assert.AreEqual(CParametros.DepredadoresIniciales, ecosistema.Contar<CDepredador>());
        }

        [TestMethod]
        public void AgregarHerbivoro_SumaUno()
        {
            CEcosistema ecosistema = new CEcosistema(300f, 200f);
            int antes = ecosistema.Contar<CHerbivoro>();

            Assert.IsTrue(ecosistema.AgregarHerbivoro());
            Assert.AreEqual(antes + 1, ecosistema.Contar<CHerbivoro>());
        }

        [TestMethod]
        public void Agregar_RespetaElTope()
        {
            CEcosistema ecosistema = new CEcosistema(300f, 200f);
            bool ultima = true;

            for (int i = 0; i < 200; i++)
            {
                ultima = ecosistema.AgregarPlanta();
            }

            Assert.AreEqual(CParametros.PoblacionMaxima, ecosistema.GetEntidades().Count);
            Assert.IsFalse(ultima, "Agregó con el acuario lleno.");
        }

        [TestMethod]
        public void Paso_AvanzaLosTicks()
        {
            CEcosistema ecosistema = new CEcosistema(300f, 200f);

            ecosistema.Paso();
            ecosistema.Paso();

            Assert.AreEqual(2, ecosistema.GetTicks());
        }

        //---------------------------------------------------------------------
        // Pez solo para pruebas: permite elegir energía y edad máxima, para
        // aislar la muerte por vejez de la muerte por hambre.
        //---------------------------------------------------------------------
        private class CPezDePrueba : CPez
        {
            public CPezDePrueba(int energiaInicial, int edadMaxima)
                : base(100f, 100f, 10f, 1f, 0.0, energiaInicial, edadMaxima)
            {
            }

            public override void Dibujar(Graphics g)
            {
            }

            protected override CPez CrearCria(double angulo, int energia)
            {
                return null;
            }
        }
    }
}
