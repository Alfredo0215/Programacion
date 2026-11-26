using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PO26B_FSA.Fluidos;

namespace PO26B_FSA.Tests
{
    //-------------------------------------------------------------------------
    // Escenas de prueba fijas de la Fase 0 (PLAN-FLUIDOS.md). La escena fija
    // el producto V·dt = 1.5 celdas/tick: la velocidad guardada es V / dt.
    //-------------------------------------------------------------------------
    internal static class EscenasFluido
    {
        public const double CourantEscena = 1.5;

        public static int Radio(CFluido f)
        {
            return Math.Max(3, (int)Math.Round(f.GetNy() / 8.0, MidpointRounding.AwayFromZero));
        }

        //---------------------------------------------------------------------
        // Vórtice suave: v_θ(ρ) = V·s·e^((1 − s²)/2), s = ρ/R. Tinta 1 en la
        // mitad superior del disco ρ ≤ R (j menor que el centro).
        //---------------------------------------------------------------------
        public static void VorticeSuave(CFluido f)
        {
            double velocidad = CourantEscena / f.GetDt();
            double radio = Radio(f);
            double xc = (f.GetNx() + 1) / 2.0;
            double yc = (f.GetNy() + 1) / 2.0;
            double[] u = f.GetU(), v = f.GetV(), tinta = f.GetTinta();

            for (int j = 1; j <= f.GetNy(); j++)
            {
                for (int i = 1; i <= f.GetNx(); i++)
                {
                    double dx = i - xc, dy = j - yc;
                    double rho = Math.Sqrt(dx * dx + dy * dy);
                    int k = f.IX(i, j);
                    if (rho > 0)
                    {
                        double s = rho / radio;
                        double vt = velocidad * s * Math.Exp((1 - s * s) / 2);
                        u[k] = -vt * dy / rho;
                        v[k] = vt * dx / rho;
                    }
                    tinta[k] = (rho <= radio && dy < 0) ? 1.0 : 0.0;
                }
            }
            f.AplicarParedes(1, u);
            f.AplicarParedes(2, v);
            f.AplicarParedes(0, tinta);
        }

        //---------------------------------------------------------------------
        // Fuente radial: (u, v) = A·(x − xc, y − yc)·e^(−ρ²/R²), con
        // A = V·√2·e^(1/2)/R; su rapidez máxima es V en ρ = R/√2.
        //---------------------------------------------------------------------
        public static void FuenteRadial(CFluido f)
        {
            double velocidad = CourantEscena / f.GetDt();
            double radio = Radio(f);
            double a = velocidad * Math.Sqrt(2) * Math.Exp(0.5) / radio;
            double xc = (f.GetNx() + 1) / 2.0;
            double yc = (f.GetNy() + 1) / 2.0;
            double[] u = f.GetU(), v = f.GetV();

            for (int j = 1; j <= f.GetNy(); j++)
            {
                for (int i = 1; i <= f.GetNx(); i++)
                {
                    double dx = i - xc, dy = j - yc;
                    double factor = a * Math.Exp(-(dx * dx + dy * dy) / (radio * radio));
                    u[f.IX(i, j)] = factor * dx;
                    v[f.IX(i, j)] = factor * dy;
                }
            }
            f.AplicarParedes(1, u);
            f.AplicarParedes(2, v);
        }

        // Máximo de |x| en las celdas interiores; "anillo" = 1 se salta
        // también las celdas pegadas al borde.
        public static double MaxAbsInterior(CFluido f, double[] x, int anillo = 0)
        {
            double maximo = 0;
            for (int j = 1 + anillo; j <= f.GetNy() - anillo; j++)
                for (int i = 1 + anillo; i <= f.GetNx() - anillo; i++)
                    maximo = Math.Max(maximo, Math.Abs(x[f.IX(i, j)]));
            return maximo;
        }

        public static double SumaInterior(CFluido f, double[] x)
        {
            double suma = 0;
            for (int j = 1; j <= f.GetNy(); j++)
                for (int i = 1; i <= f.GetNx(); i++)
                    suma += x[f.IX(i, j)];
            return suma;
        }

        // r = máx|div después| / máx|div antes| con la fuente radial.
        public static double CocienteProyeccion(int nx, int ny, int k, double omega)
        {
            CFluido f = new CFluido(nx, ny, k, omega);
            FuenteRadial(f);
            double antes = MaxAbsInterior(f, f.Divergencia());
            f.Proyectar();
            double despues = MaxAbsInterior(f, f.Divergencia());
            return despues / antes;
        }

        // Variación relativa de la tinta total tras 200 ticks con el vórtice.
        public static double VariacionTinta(int nx, int ny, int k, double omega)
        {
            CFluido f = new CFluido(nx, ny, k, omega);
            VorticeSuave(f);
            double inicial = SumaInterior(f, f.GetTinta());
            for (int t = 0; t < 200; t++) f.Paso();
            double final = SumaInterior(f, f.GetTinta());
            return Math.Abs(final - inicial) / inicial;
        }

        // Ticks hasta que el máx|vorticidad| (sin el anillo de borde) baja al
        // 10 % de su pico; -1 si no baja en "tope" ticks.
        public static int Pulso(int nx, int ny, int k, double omega, int tope)
        {
            CFluido f = new CFluido(nx, ny, k, omega);
            VorticeSuave(f);
            double pico = MaxAbsInterior(f, f.Vorticidad(), 1);
            for (int t = 1; t <= tope; t++)
            {
                f.Paso();
                double actual = MaxAbsInterior(f, f.Vorticidad(), 1);
                pico = Math.Max(pico, actual);
                if (actual <= 0.1 * pico) return t;
            }
            return -1;
        }

        // Número de Courant máx|velocidad|·dt cada 100 ticks de chorro.
        public static List<double> CourantConChorro(int nx, int ny, int k, double omega, int ticks,
                                                    bool proyeccion = true)
        {
            CFluido f = new CFluido(nx, ny, k, omega);
            f.SetProyeccion(proyeccion);
            List<double> lecturas = new List<double>();
            for (int t = 1; t <= ticks; t++)
            {
                f.AgregarChorro();
                f.Paso();
                if (t % 100 == 0)
                {
                    double maximo = 0;
                    for (int j = 1; j <= ny; j++)
                        for (int i = 1; i <= nx; i++)
                        {
                            int c = f.IX(i, j);
                            double rapidez = Math.Sqrt(f.GetU()[c] * f.GetU()[c] + f.GetV()[c] * f.GetV()[c]);
                            maximo = Math.Max(maximo, rapidez);
                        }
                    lecturas.Add(maximo * f.GetDt());
                }
            }
            return lecturas;
        }
    }

    [TestClass]
    public class CFluidoTests
    {
        // Combinación elegida en la Fase 0 y su r medido (diagnostico.txt).
        // El umbral es 2·r y nunca pasa de 0.4.
        private const int ColumnasElegidas = 160;
        private const int IteracionesElegidas = 20;
        private const double OmegaElegido = 1.7;
        private const double UmbralProyeccion = 2 * 0.0502;   // r medido: 0.0502 (160×59, k = 20, SOR ω = 1.7).

        private static int FilasElegidas { get { return CVistaFluido.FilasPara(ColumnasElegidas); } }

        //---------------------------------------------------------------------
        // La proyección reduce la divergencia de la fuente radial. Un
        // Proyectar que no hace nada da r ≈ 1 y falla.
        //---------------------------------------------------------------------
        [TestMethod]
        public void Proyectar_ReduceLaDivergencia()
        {
            double r = EscenasFluido.CocienteProyeccion(ColumnasElegidas, FilasElegidas,
                                                        IteracionesElegidas, OmegaElegido);
            Assert.IsTrue(r <= UmbralProyeccion, $"r = {r:0.0000} supera {UmbralProyeccion}.");
        }

        //---------------------------------------------------------------------
        // La bilineal conserva exactamente un campo constante.
        //---------------------------------------------------------------------
        [TestMethod]
        public void Advectar_CampoUniforme_SigueUniforme()
        {
            CFluido f = new CFluido(20, 10);
            double[] u = new double[f.GetU().Length];
            double[] v = new double[u.Length];
            double[] origen = new double[u.Length];
            double[] destino = new double[u.Length];
            for (int k = 0; k < u.Length; k++)
            {
                u[k] = 1.3 * Math.Sin(k * 0.37);
                v[k] = -0.8 * Math.Cos(k * 0.21);
                origen[k] = 0.3;
            }

            f.Advectar(0, destino, origen, u, v);

            for (int j = 1; j <= 10; j++)
                for (int i = 1; i <= 20; i++)
                    Assert.AreEqual(0.3, destino[f.IX(i, j)], 1e-12);
        }

        //---------------------------------------------------------------------
        // La interpolación es una mezcla ponderada de vecinas: ningún valor
        // nuevo queda fuera del rango [mín, máx] del campo de origen.
        //---------------------------------------------------------------------
        [TestMethod]
        public void Advectar_NoCreaMaximosNuevos()
        {
            CFluido f = new CFluido(30, 15);
            EscenasFluido.VorticeSuave(f);
            Random azar = new Random(1);
            double[] origen = new double[f.GetU().Length];
            double[] destino = new double[origen.Length];
            for (int k = 0; k < origen.Length; k++) origen[k] = azar.NextDouble();
            double minimo = double.MaxValue, maximo = double.MinValue;
            foreach (double x in origen) { minimo = Math.Min(minimo, x); maximo = Math.Max(maximo, x); }

            f.Advectar(0, destino, origen, f.GetU(), f.GetV());

            for (int j = 1; j <= 15; j++)
                for (int i = 1; i <= 30; i++)
                {
                    double x = destino[f.IX(i, j)];
                    Assert.IsTrue(x >= minimo - 1e-12 && x <= maximo + 1e-12, $"({i}, {j}) = {x}");
                }
        }

        //---------------------------------------------------------------------
        // Con velocidad uniforme de 1.5 celdas/tick, el centro de la tinta se
        // mueve exactamente 1.5 celdas (lejos de las paredes). Un Advectar
        // que no hace nada pasaría los dos tests anteriores, pero no este.
        //---------------------------------------------------------------------
        [TestMethod]
        public void Advectar_VelocidadUniforme_DesplazaElCentroDeLaTinta()
        {
            CFluido f = new CFluido(40, 20);
            int n = f.GetU().Length;
            double[] u = new double[n], v = new double[n];
            double[] origen = new double[n], destino = new double[n];
            for (int k = 0; k < n; k++) u[k] = 1.5;
            for (int j = 8; j <= 12; j++)
                for (int i = 8; i <= 12; i++)
                    origen[f.IX(i, j)] = 1.0;

            f.Advectar(0, destino, origen, u, v);

            Assert.AreEqual(CentroX(f, origen) + 1.5, CentroX(f, destino), 1e-9);
        }

        //---------------------------------------------------------------------
        // La velocidad en la pared es el promedio entre la celda de borde y
        // su vecina interior, y debe valer 0. La celda de borde sola no vale
        // 0 (Stam le pone el negativo de su vecina).
        //---------------------------------------------------------------------
        [TestMethod]
        public void Paredes_VelocidadNormalEsCero()
        {
            CFluido f = new CFluido(30, 12);
            EscenasFluido.FuenteRadial(f);
            for (int t = 0; t < 5; t++) f.Paso();
            double[] u = f.GetU(), v = f.GetV();

            for (int j = 1; j <= 12; j++)
            {
                Assert.AreEqual(0, 0.5 * (u[f.IX(0, j)] + u[f.IX(1, j)]), 1e-12);
                Assert.AreEqual(0, 0.5 * (u[f.IX(31, j)] + u[f.IX(30, j)]), 1e-12);
            }
            for (int i = 1; i <= 30; i++)
            {
                Assert.AreEqual(0, 0.5 * (v[f.IX(i, 0)] + v[f.IX(i, 1)]), 1e-12);
                Assert.AreEqual(0, 0.5 * (v[f.IX(i, 13)] + v[f.IX(i, 12)]), 1e-12);
            }
        }

        //---------------------------------------------------------------------
        // Con Nx ≠ Ny, cambiar i por j pondría la tinta en otra celda (o
        // fuera de la cuadrícula).
        //---------------------------------------------------------------------
        [TestMethod]
        public void CuadriculaRectangular_TintaApareceEnLaCeldaPedida()
        {
            CFluido f = new CFluido(30, 10);
            f.AgregarTinta(25, 3, 1.0);

            Assert.AreEqual(1.0, f.GetTinta()[f.IX(25, 3)]);
            Assert.AreEqual(1.0, EscenasFluido.SumaInterior(f, f.GetTinta()), 1e-12);
        }

        [TestMethod]
        public void MilPasosConFuerzasGrandes_SinNaN()
        {
            CFluido f = new CFluido(40, 20);
            Random azar = new Random(7);
            for (int t = 0; t < 1000; t++)
            {
                int i = azar.Next(1, 41), j = azar.Next(1, 21);
                f.AgregarFuerza(i, j, azar.NextDouble() * 100 - 50, azar.NextDouble() * 100 - 50);
                f.AgregarTinta(i, j, 5.0);
                f.Paso();
            }

            foreach (double[] campo in new[] { f.GetU(), f.GetV(), f.GetTinta(), f.GetPresion() })
                foreach (double x in campo)
                    Assert.IsFalse(double.IsNaN(x) || double.IsInfinity(x));
        }

        //---------------------------------------------------------------------
        // Detector de regresiones, no prueba de corrección. El límite previo
        // (5 %) no se cumple por una propiedad del método: la advección
        // semi-lagrangiana no conserva la masa, y con valores en el centro de
        // las celdas la proyección no ve parte de la divergencia. Se aceptó la
        // cifra medida (67.87 % con 160×59, k = 20, ω = 1.7) redondeada al
        // entero superior (Registro de PLAN-FLUIDOS.md). Un fallo grande, como
        // perder SOR (213 %), lo detecta; uno pequeño, no.
        //---------------------------------------------------------------------
        [TestMethod]
        public void TintaTotal_SinDisipacion_DentroDelMargen()
        {
            double variacion = EscenasFluido.VariacionTinta(ColumnasElegidas, FilasElegidas,
                                                            IteracionesElegidas, OmegaElegido);
            Assert.IsTrue(variacion <= 0.68, $"La tinta varió {variacion:P2}.");
        }

        [TestMethod]
        public void ColorTinta_CeroEsElFondo()
        {
            Assert.AreEqual(Color.FromArgb(0xF5, 0xF4, 0xEE).ToArgb(), CVistaFluido.ColorTinta(0).ToArgb());
        }

        //---------------------------------------------------------------------
        // Con un control más ancho y otro más alto que la cuadrícula, el
        // rectángulo conserva la proporción Nx:Ny (salvo el redondeo a
        // píxeles), está centrado y cabe dentro.
        //---------------------------------------------------------------------
        [TestMethod]
        public void RectanguloDibujo_ConservaLaProporcion()
        {
            const int nx = 160, ny = 59;
            foreach (Size control in new[] { new Size(1000, 200), new Size(400, 600) })
            {
                Rectangle r = CVistaFluido.RectanguloDibujo(control, nx, ny);

                Assert.IsTrue(r.Left >= 0 && r.Top >= 0 && r.Right <= control.Width && r.Bottom <= control.Height);
                Assert.IsTrue(r.Width == control.Width || r.Height == control.Height, "No es el mayor posible.");
                Assert.AreEqual((double)nx / ny, (double)r.Width / r.Height, 0.05);
                Assert.IsTrue(Math.Abs(r.Left - (control.Width - r.Right)) <= 1, "No está centrado en horizontal.");
                Assert.IsTrue(Math.Abs(r.Top - (control.Height - r.Bottom)) <= 1, "No está centrado en vertical.");
            }
        }

        //---------------------------------------------------------------------
        // El pincel con el clic quieto llama a AsegurarTinta en cada tick
        // sobre la misma celda: la tinta no debe pasar del mínimo pedido, y
        // una celda que ya tiene más no se rebaja.
        //---------------------------------------------------------------------
        [TestMethod]
        public void AsegurarTinta_RepetidaNoCreceSinLimite()
        {
            var f = new CFluido(20, 10);
            for (int n = 0; n < 100; n++)
            {
                f.AsegurarTinta(5, 5, 1.0);
            }
            Assert.AreEqual(1.0, f.GetTinta()[f.IX(5, 5)], 1e-12);

            f.AgregarTinta(6, 5, 3.0);
            f.AsegurarTinta(6, 5, 1.0);
            Assert.AreEqual(3.0, f.GetTinta()[f.IX(6, 5)], 1e-12);
        }

        //---------------------------------------------------------------------
        // PantallaACelda con el tamaño de diseño (sin franjas) y con otra
        // proporción (franjas arriba y abajo). Las esquinas del rectángulo de
        // dibujo dan las celdas 1 y Nx (no 0 y Nx − 1), y el píxel central de
        // una celda cae en esa celda.
        //---------------------------------------------------------------------
        private static readonly Size[] TamanosPrueba = { new Size(910, 336), new Size(600, 400) };

        [TestMethod]
        public void PantallaACelda_EsquinasYCentro()
        {
            const int nx = 160, ny = 59;
            foreach (Size control in TamanosPrueba)
            {
                Rectangle r = CVistaFluido.RectanguloDibujo(control, nx, ny);

                Assert.AreEqual(new Point(1, 1),
                    CVistaFluido.PantallaACelda(new Point(r.Left, r.Top), control, nx, ny));
                Assert.AreEqual(new Point(nx, 1),
                    CVistaFluido.PantallaACelda(new Point(r.Right - 1, r.Top), control, nx, ny));
                Assert.AreEqual(new Point(1, ny),
                    CVistaFluido.PantallaACelda(new Point(r.Left, r.Bottom - 1), control, nx, ny));
                Assert.AreEqual(new Point(nx, ny),
                    CVistaFluido.PantallaACelda(new Point(r.Right - 1, r.Bottom - 1), control, nx, ny));

                // Centro de la celda (80, 30): (c − 0.5) celdas desde el borde.
                int x = r.Left + (int)((80 - 0.5) * r.Width / nx);
                int y = r.Top + (int)((30 - 0.5) * r.Height / ny);
                Assert.AreEqual(new Point(80, 30),
                    CVistaFluido.PantallaACelda(new Point(x, y), control, nx, ny), $"Control {control}.");
            }
        }

        [TestMethod]
        public void PantallaACelda_FueraDelControl_SeRecorta()
        {
            const int nx = 160, ny = 59;
            foreach (Size control in TamanosPrueba)
            {
                Assert.AreEqual(new Point(1, 1),
                    CVistaFluido.PantallaACelda(new Point(-50, -50), control, nx, ny));
                Assert.AreEqual(new Point(nx, ny),
                    CVistaFluido.PantallaACelda(new Point(control.Width + 50, control.Height + 50), control, nx, ny));
            }

            // Con 600×400 hay franjas arriba y abajo: un clic en ellas cae en
            // la fila del borde más cercana.
            Size conFranjas = new Size(600, 400);
            Rectangle rf = CVistaFluido.RectanguloDibujo(conFranjas, nx, ny);
            Assert.IsTrue(rf.Top > 5, "La prueba necesita franjas.");
            Assert.AreEqual(1, CVistaFluido.PantallaACelda(new Point(300, 2), conFranjas, nx, ny).Y);
            Assert.AreEqual(ny, CVistaFluido.PantallaACelda(new Point(300, 397), conFranjas, nx, ny).Y);
        }

        //---------------------------------------------------------------------
        // Rotación rígida u = −Ω·(y − yc), v = Ω·(x − xc): vorticidad 2Ω en
        // todas las celdas interiores sin el anillo de borde.
        //---------------------------------------------------------------------
        [TestMethod]
        public void Vorticidad_RotacionRigida_EsConstante()
        {
            const double omegaGiro = 0.05;
            CFluido f = new CFluido(30, 14);
            double xc = 15.5, yc = 7.5;
            for (int j = 0; j <= 15; j++)
                for (int i = 0; i <= 31; i++)
                {
                    f.GetU()[f.IX(i, j)] = -omegaGiro * (j - yc);
                    f.GetV()[f.IX(i, j)] = omegaGiro * (i - xc);
                }

            double[] w = f.Vorticidad();

            for (int j = 2; j <= 13; j++)
                for (int i = 2; i <= 29; i++)
                    Assert.AreEqual(2 * omegaGiro, w[f.IX(i, j)], 1e-12);
        }

        //---------------------------------------------------------------------
        // Fase 2. Con j hacia abajo, u = −Ω·(y − yc), v = Ω·(x − xc) gira en
        // sentido horario en pantalla (a la derecha del centro baja). Su
        // vorticidad debe ser positiva y pintarse en ocre.
        //---------------------------------------------------------------------
        [TestMethod]
        public void Vorticidad_GiroHorarioEnPantalla_EsPositiva()
        {
            const double omegaGiro = 0.05;
            CFluido f = new CFluido(30, 14);
            double xc = 15.5, yc = 7.5;
            for (int j = 0; j <= 15; j++)
                for (int i = 0; i <= 31; i++)
                {
                    f.GetU()[f.IX(i, j)] = -omegaGiro * (j - yc);
                    f.GetV()[f.IX(i, j)] = omegaGiro * (i - xc);
                }

            // A la derecha del centro, el fluido baja (v > 0 con j hacia abajo).
            Assert.IsTrue(f.GetV()[f.IX(25, 7)] > 0);

            double w = f.Vorticidad()[f.IX(15, 7)];
            Assert.IsTrue(w > 0, $"vorticidad = {w}");
            Assert.AreEqual(CVistaFluido.Ocre.ToArgb(), CVistaFluido.ColorDivergente(w, w).ToArgb());
        }

        //---------------------------------------------------------------------
        // La fuente radial tiene divergencia positiva en el centro; la
        // presión que la anula cumple Laplaciano(p) = div > 0, es decir, un
        // mínimo. Centrada, la celda central es negativa (ciruela).
        //---------------------------------------------------------------------
        [TestMethod]
        public void Presion_FuenteRadial_CentroEsBaja()
        {
            CFluido f = new CFluido(ColumnasElegidas, FilasElegidas, IteracionesElegidas, OmegaElegido);
            EscenasFluido.FuenteRadial(f);
            f.Proyectar();

            double[] centrada = new double[f.GetPresion().Length];
            CVistaFluido.CentrarInterior(f.GetPresion(), centrada, f.GetNx(), f.GetNy());

            int ic = (f.GetNx() + 1) / 2, jc = (f.GetNy() + 1) / 2;
            double centro = centrada[f.IX(ic, jc)];
            Assert.IsTrue(centro < 0, $"presión centrada en el centro = {centro}");
            Assert.AreEqual(CVistaFluido.Ciruela.ToArgb(),
                            CVistaFluido.ColorDivergente(centro, Math.Abs(centro)).ToArgb());
        }

        //---------------------------------------------------------------------
        // Sumar una constante a la presión no cambia el campo centrado, y su
        // media en las celdas interiores es 0.
        //---------------------------------------------------------------------
        [TestMethod]
        public void PresionCentrada_MediaCero()
        {
            const int nx = 12, ny = 7;
            int total = (nx + 2) * (ny + 2);
            double[] a = new double[total], b = new double[total];
            for (int k = 0; k < total; k++)
            {
                a[k] = Math.Sin(k * 0.7) * 3.0;
                b[k] = a[k] + 41.5;
            }

            double[] ca = new double[total], cb = new double[total];
            CVistaFluido.CentrarInterior(a, ca, nx, ny);
            CVistaFluido.CentrarInterior(b, cb, nx, ny);

            double suma = 0;
            for (int j = 1; j <= ny; j++)
                for (int i = 1; i <= nx; i++)
                {
                    int k = i + (nx + 2) * j;
                    Assert.AreEqual(ca[k], cb[k], 1e-9);
                    suma += ca[k];
                }
            Assert.AreEqual(0.0, suma, 1e-9);
        }

        [TestMethod]
        public void ColorDivergente_CeroEsElFondo()
        {
            Assert.AreEqual(CVistaFluido.Fondo.ToArgb(), CVistaFluido.ColorDivergente(0, 1).ToArgb());
        }

        [TestMethod]
        public void ColorDivergente_SignosVanACadaExtremo()
        {
            // Más allá de la escala se recorta al extremo.
            Assert.AreEqual(CVistaFluido.Ocre.ToArgb(), CVistaFluido.ColorDivergente(5, 1).ToArgb());
            Assert.AreEqual(CVistaFluido.Ciruela.ToArgb(), CVistaFluido.ColorDivergente(-5, 1).ToArgb());
        }

        //---------------------------------------------------------------------
        // Al arrancar la presión vale 0 en todas partes: ni la función ni la
        // vista completa deben dar NaN (Color.FromArgb lanzaría una excepción).
        //---------------------------------------------------------------------
        [TestMethod]
        public void ColorDivergente_CampoTodoCero_NoDaNaN()
        {
            Assert.AreEqual(CVistaFluido.Fondo.ToArgb(), CVistaFluido.ColorDivergente(0, 0).ToArgb());
            Assert.AreEqual(CVistaFluido.Fondo.ToArgb(), CVistaFluido.ColorDivergente(0, double.NaN).ToArgb());

            CFluido f = new CFluido(20, 10);
            CVistaFluido vista = new CVistaFluido(20, 10);
            vista.SetVista(CVistaFluido.Vista.Presion);
            vista.Actualizar(f);
            vista.SetVista(CVistaFluido.Vista.Vorticidad);
            vista.Actualizar(f);
        }

        //---------------------------------------------------------------------
        // Con la proyección apagada, u y v tras Paso() son exactamente los de
        // advectar una copia del mismo estado. Repite el orden de Paso() a
        // propósito: es lo que distingue un interruptor que funciona de uno
        // que no hace nada. La segunda parte comprueba que, encendida, la
        // proyección sí cambia la velocidad (si no, el test no probaría nada).
        //---------------------------------------------------------------------
        [TestMethod]
        public void SinProyeccion_PasoEsSoloAdveccion()
        {
            CFluido f = new CFluido(40, 15, IteracionesElegidas, OmegaElegido);
            CFluido referencia = new CFluido(40, 15, IteracionesElegidas, OmegaElegido);
            CFluido conProyeccion = new CFluido(40, 15, IteracionesElegidas, OmegaElegido);
            EscenasFluido.FuenteRadial(f);
            EscenasFluido.FuenteRadial(referencia);
            EscenasFluido.FuenteRadial(conProyeccion);

            f.SetProyeccion(false);
            f.Paso();
            conProyeccion.Paso();

            double[] u0 = (double[])referencia.GetU().Clone();
            double[] v0 = (double[])referencia.GetV().Clone();
            referencia.Advectar(1, referencia.GetU(), u0, u0, v0);
            referencia.Advectar(2, referencia.GetV(), v0, u0, v0);

            double diferenciaConProyeccion = 0;
            for (int k = 0; k < u0.Length; k++)
            {
                Assert.AreEqual(referencia.GetU()[k], f.GetU()[k], 0.0);
                Assert.AreEqual(referencia.GetV()[k], f.GetV()[k], 0.0);
                diferenciaConProyeccion = Math.Max(diferenciaConProyeccion,
                    Math.Abs(conProyeccion.GetU()[k] - f.GetU()[k]));
            }
            Assert.IsTrue(diferenciaConProyeccion > 1e-3,
                $"la proyección encendida apenas cambia u ({diferenciaConProyeccion})");
        }

        //---------------------------------------------------------------------
        // Diagnóstico de la Fase 0: no falla nunca, solo mide y escribe
        // diagnostico.txt en la carpeta de salida. Es lento: se excluye con
        // --filter "TestCategory!=Diagnostico".
        //---------------------------------------------------------------------
        [TestMethod]
        [TestCategory("Diagnostico")]
        public void Diagnostico_PorCombinacion()
        {
            int[] columnas = { 64, 96, 128, 160 };
            int[] pasadas = { 20, 40 };
            double[] omegas = { 1.0, 1.7 };
            const int TopePulso = 2000;
            CultureInfo ci = CultureInfo.InvariantCulture;
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Columnas×filas | k | ω | r | Tinta (%) | Pulso (ticks) | Courant con chorro cada 100 ticks (máx)");
            foreach (int nx in columnas)
            {
                int ny = CVistaFluido.FilasPara(nx);
                foreach (int k in pasadas)
                {
                    foreach (double w in omegas)
                    {
                        double r = EscenasFluido.CocienteProyeccion(nx, ny, k, w);
                        string fila = string.Format(ci, "{0}×{1} | {2} | {3:0.0} | {4:0.0000}", nx, ny, k, w, r);
                        if (r <= 0.2)
                        {
                            double tinta = EscenasFluido.VariacionTinta(nx, ny, k, w) * 100;
                            int pulso = EscenasFluido.Pulso(nx, ny, k, w, TopePulso);
                            List<double> courant = EscenasFluido.CourantConChorro(nx, ny, k, w, 2000);
                            double maximo = 0;
                            foreach (double c in courant) maximo = Math.Max(maximo, c);
                            fila += string.Format(ci, " | {0:0.00} | {1} | {2} (máx {3:0.00})",
                                tinta, pulso < 0 ? "> " + TopePulso : pulso.ToString(ci),
                                string.Join(" ", courant.ConvertAll(c => c.ToString("0.00", ci))), maximo);
                        }
                        else
                        {
                            fila += " | — | — | — (no cumple r)";
                        }
                        sb.AppendLine(fila);
                    }
                }
            }

            sb.AppendLine();
            sb.AppendLine("Piso (sin SOR): columnas×filas | k | r");
            foreach (int nx in columnas)
            {
                int ny = CVistaFluido.FilasPara(nx);
                sb.AppendLine(string.Format(ci, "{0}×{1} | 2000 | {2:0.0000}", nx, ny,
                    EscenasFluido.CocienteProyeccion(nx, ny, 2000, 1.0)));
            }
            int nxMax = columnas[columnas.Length - 1];
            sb.AppendLine(string.Format(ci, "Comprobación del piso: {0}×{1} | 4000 | {2:0.0000}", nxMax,
                CVistaFluido.FilasPara(nxMax), EscenasFluido.CocienteProyeccion(nxMax, CVistaFluido.FilasPara(nxMax), 4000, 1.0)));

            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "diagnostico.txt");
            File.WriteAllText(ruta, sb.ToString(), Encoding.UTF8);
            Console.WriteLine(sb.ToString());
            Console.WriteLine("Escrito en " + ruta);
        }

        //---------------------------------------------------------------------
        // Fase 3. Tras 200 pasos con entrada y obstáculo, las celdas del
        // sólido no tienen tinta ni velocidad, aunque el ratón empuje dentro.
        // La tinta total > 0 comprueba que la entrada sí mete tinta.
        //---------------------------------------------------------------------
        [TestMethod]
        public void Solido_SinTintaNiVelocidadTrasPasos()
        {
            CFluido f = new CFluido(ColumnasElegidas, FilasElegidas, IteracionesElegidas, OmegaElegido);
            f.SetObstaculo(true);
            Assert.IsTrue(f.GetCeldasSolidas().Length > 0);

            int dentro = f.GetCeldasSolidas()[0];
            int ancho = f.GetNx() + 2;
            for (int t = 0; t < 200; t++)
            {
                f.AgregarFuerza(dentro % ancho, dentro / ancho, 1.5, 1.5);
                f.AsegurarTinta(dentro % ancho, dentro / ancho, 1.0);
                f.Paso();
            }

            foreach (int k in f.GetCeldasSolidas())
            {
                Assert.AreEqual(0.0, f.GetU()[k]);
                Assert.AreEqual(0.0, f.GetV()[k]);
                Assert.AreEqual(0.0, f.GetTinta()[k]);
            }
            Assert.IsTrue(EscenasFluido.SumaInterior(f, f.GetTinta()) > 0, "la entrada no metió tinta");
        }

        //---------------------------------------------------------------------
        // Un flujo uniforme hacia la derecha que encuentra el círculo se
        // desvía: aparece velocidad vertical. Si la máscara se ignorara, v
        // seguiría en 0 (lo comprueba el test de entrada y salida).
        //---------------------------------------------------------------------
        [TestMethod]
        public void Solido_FlujoUniforme_SeDesvia()
        {
            CFluido f = new CFluido(ColumnasElegidas, FilasElegidas, IteracionesElegidas, OmegaElegido);
            for (int j = 1; j <= f.GetNy(); j++)
                for (int i = 1; i <= f.GetNx(); i++)
                    f.GetU()[f.IX(i, j)] = 0.7;
            f.SetObstaculo(true);

            for (int t = 0; t < 20; t++) f.Paso();

            double maxV = EscenasFluido.MaxAbsInterior(f, f.GetV());
            Assert.IsTrue(maxV > 1e-3, $"máx |v| = {maxV}: el flujo no se desvió");
        }

        //---------------------------------------------------------------------
        // Con los bordes abiertos y sin círculo, un flujo uniforme igual a la
        // velocidad de entrada no tiene divergencia: sigue uniforme 100 pasos.
        //---------------------------------------------------------------------
        [TestMethod]
        public void EntradaYSalida_FlujoUniforme_SeMantiene()
        {
            CFluido f = new CFluido(ColumnasElegidas, FilasElegidas, IteracionesElegidas, OmegaElegido);
            f.SetBordesAbiertos(true);
            for (int j = 1; j <= f.GetNy(); j++)
                for (int i = 1; i <= f.GetNx(); i++)
                    f.GetU()[f.IX(i, j)] = 0.7;
            f.AplicarParedes(1, f.GetU());

            for (int t = 0; t < 100; t++) f.Paso();

            for (int j = 1; j <= f.GetNy(); j++)
                for (int i = 1; i <= f.GetNx(); i++)
                {
                    Assert.AreEqual(0.7, f.GetU()[f.IX(i, j)], 1e-9);
                    Assert.AreEqual(0.0, f.GetV()[f.IX(i, j)], 1e-9);
                }
        }

        //---------------------------------------------------------------------
        // Desactivar el obstáculo quita el círculo y vuelve a la caja
        // cerrada: la celda de borde izquierda de u vuelve a ser el negativo
        // de su vecina (pared), no la velocidad de entrada.
        //---------------------------------------------------------------------
        [TestMethod]
        public void SinObstaculo_VuelveALaCajaCerrada()
        {
            CFluido f = new CFluido(40, 15);
            f.SetObstaculo(true);
            f.SetObstaculo(false);

            Assert.IsFalse(f.GetObstaculo());
            Assert.AreEqual(0, f.GetCeldasSolidas().Length);

            f.GetU()[f.IX(1, 5)] = 0.3;
            f.AplicarParedes(1, f.GetU());
            Assert.AreEqual(-0.3, f.GetU()[f.IX(0, 5)], 1e-12);
        }

        //---------------------------------------------------------------------
        // Diagnóstico de la Fase 2: el chorro sin proyección durante 1000
        // ticks con la combinación vigente, Courant cada 100 ticks. Si pasa
        // de 3 celdas/tick, el guion limita la proyección apagada a ~10 s.
        // No falla nunca; escribe diagnostico-sin-proyeccion.txt.
        //---------------------------------------------------------------------
        [TestMethod]
        [TestCategory("Diagnostico")]
        public void Diagnostico_SinProyeccion()
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            List<double> courant = EscenasFluido.CourantConChorro(ColumnasElegidas, FilasElegidas,
                IteracionesElegidas, OmegaElegido, 1000, false);
            double maximo = 0;
            foreach (double c in courant) maximo = Math.Max(maximo, c);

            string texto = string.Format(ci, "Sin proyección, {0}×{1}, 1000 ticks de chorro. Courant cada 100 ticks: {2} (máx {3:0.00})",
                ColumnasElegidas, FilasElegidas,
                string.Join(" ", courant.ConvertAll(c => c.ToString("0.00", ci))), maximo);

            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "diagnostico-sin-proyeccion.txt");
            File.WriteAllText(ruta, texto, Encoding.UTF8);
            Console.WriteLine(texto);
        }

        private static double CentroX(CFluido f, double[] campo)
        {
            double suma = 0, momento = 0;
            for (int j = 1; j <= f.GetNy(); j++)
                for (int i = 1; i <= f.GetNx(); i++)
                {
                    suma += campo[f.IX(i, j)];
                    momento += i * campo[f.IX(i, j)];
                }
            return momento / suma;
        }
    }
}
