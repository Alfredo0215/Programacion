using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PO26B_FSA.Acuario;
using PO26B_FSA.Fluidos;

namespace PO26B_FSA.Mesas_De_Practicas
{
    public partial class MesaDePracticas4 : Form
    {

        private CEcosistema ecosistema;
        // La pausa la decide el usuario; ocultar el panel es otra causa
        // (navegación). Por eso van separadas.
        private bool pausado;

        //---------------------------------------------------------------------
        // Proyecto Final IA: fluidos. N, k y ω se fijan con el diagnóstico y
        // la prueba de tiempo de la Fase 0 (PLAN-FLUIDOS.md).
        //---------------------------------------------------------------------
        private const int ColumnasFluido = 160;
        private const int IteracionesFluido = 20;
        private const double OmegaFluido = 1.7;
        private const double DisipacionFluido = 0.003;
        private static readonly InterpolationMode InterpolacionFluido = InterpolationMode.NearestNeighbor;

        // true: LblFluido muestra también ticks/s y ms (la Fase 4 lo usa
        // para la lista previa). false: solo "Ticks: N".
        private const bool MedirFluido = false;
        private const int MuestrasMedicion = 60;

        // Ratón. El tope de la fuerza no supera V = 1.5 celdas/tick, la
        // velocidad de la escena de prueba de la Fase 0. Con dt = 1 (el de
        // esta mesa), la fuerza en celdas/tick es el desplazamiento del tick.
        private const double TopeFuerzaRaton = 1.5;
        private const int RadioPincel = 2;          // En celdas.
        // Tinta mínima bajo el pincel. No se suma: con el clic quieto, la
        // celda se queda en 1 (ocre) en vez de crecer hasta el color oscuro.
        private const double TintaPincel = 1.0;

        private CFluido fluido;
        private CVistaFluido vistaFluido;
        private bool fluidoPausado;
        private bool arrastrandoFluido;
        // Elecciones del usuario en la Fase 2. Viven en el formulario para
        // que Reiniciar las conserve, igual que respeta la pausa.
        private CVistaFluido.Vista vistaElegidaFluido = CVistaFluido.Vista.Tinta;
        private bool proyeccionFluido = true;
        private bool obstaculoFluido;
        // Posiciones del ratón (en píxeles) desde el último tick. Se gastan en
        // el siguiente Tick: un solo reloj manda en la simulación.
        private readonly List<Point> trazoFluido = new List<Point>();
        // Celdas que toca el trazo en un tick, sin repetir: así cada celda
        // recibe la fuerza una sola vez y el tope se respeta.
        private readonly HashSet<Point> celdasTrazo = new HashSet<Point>();

        private readonly Stopwatch relojFluido = new Stopwatch();
        private readonly double[] msPaso = new double[MuestrasMedicion];
        private readonly double[] msDibujo = new double[MuestrasMedicion];
        private readonly double[] msTotal = new double[MuestrasMedicion];
        private readonly long[] marcasTick = new long[MuestrasMedicion];
        private int muestrasTomadas;
        private long ultimaEtiquetaMs = -1000;

        public MesaDePracticas4()
        {

            InitializeComponent();
            PnlProyectoF.Visible = false;
            PnlProyectoFIA.Visible = false;
            PbxAcuario.Paint += PbxAcuario_Paint;

            // Se suscriben después de ocultar el panel, para no reaccionar a
            // ese primer cambio.
            PnlProyectoFIA.VisibleChanged += PnlProyectoFIA_VisibleChanged;
            TmrFluido.Tick += TmrFluido_Tick;
            PbxFluido.Paint += PbxFluido_Paint;
            PbxFluido.MouseDown += PbxFluido_MouseDown;
            PbxFluido.MouseMove += PbxFluido_MouseMove;
            PbxFluido.MouseUp += PbxFluido_MouseUp;
        }

        //---------------------------------------------------------------------
        // Alterna el panel elegido y oculta los otros tres. "mostrar" se
        // calcula antes de ocultar todo para que un segundo clic lo cierre.
        //---------------------------------------------------------------------
        private void AlternarPractica(Panel elegido)
        {
            bool mostrar = !elegido.Visible;
            PnlProyectoF.Visible = false;
            PnlProyectoFIA.Visible = false;
            elegido.Visible = mostrar;
        }



        private void BtnPractica2_Click(object sender, EventArgs e)
        {
            AlternarPractica(PnlProyectoF);
        }



        private void BtnPractica4_Click(object sender, EventArgs e)
        {
            AlternarPractica(PnlProyectoFIA);
        }
        private void TimerAcuario_Tick(object sender, EventArgs e)
        {
            // El Timer solo arranca después de crear el ecosistema
            // (PnlProyectoF_VisibleChanged), así que aquí nunca es null.
            ecosistema.Paso();
            RefrescarAcuario();
        }

        //---------------------------------------------------------------------
        // Solo muestra: pregunta al ecosistema y redibuja. No cuenta por su
        // cuenta ni guarda otro reloj. El tiempo va en ticks, la unidad de
        // las reglas; no en segundos, porque Interval es un mínimo, no una
        // duración garantizada.
        //---------------------------------------------------------------------
        private void RefrescarAcuario()
        {
            LblEstadisticas.Text =
                $"Plantas: {ecosistema.Contar<CPlanta>()}   " +
                $"Herbívoros: {ecosistema.Contar<CHerbivoro>()}   " +
                $"Depredadores: {ecosistema.Contar<CDepredador>()}   " +
                $"Ticks: {ecosistema.GetTicks()}";
            PbxAcuario.Invalidate();
        }

        private void PnlProyectoF_VisibleChanged(object sender, EventArgs e)
        {
            if (!PnlProyectoF.Visible)
            { TimerAcuario.Stop(); return; }

            // Se crea al mostrarse por primera vez: hasta entonces el Dock
            // no garantiza que PbxAcuario tenga su tamaño real.
            if (ecosistema == null)
            {
                ecosistema = new CEcosistema(PbxAcuario.ClientSize.Width, PbxAcuario.ClientSize.Height);
                RefrescarAcuario();
            }

            // Volver al panel no anula una pausa del usuario.
            if (!pausado)
            {
                TimerAcuario.Start();
            }
        }

        //---------------------------------------------------------------------
        // Dibuja el acuario. Invalidate() pide a Windows que lance este evento.
        //---------------------------------------------------------------------
        private void PbxAcuario_Paint(object sender, PaintEventArgs e)
        {
            if (ecosistema != null)
            {
                ecosistema.Dibujar(e.Graphics);
            }
        }

        //---------------------------------------------------------------------
        // Botones del acuario: solo reenvían la acción al ecosistema y
        // refrescan. Las reglas (dónde nace, si cabe) están en CEcosistema.
        //---------------------------------------------------------------------
        private void BtnIniciarPausar_Click(object sender, EventArgs e)
        {
            pausado = !pausado;
            TimerAcuario.Enabled = !pausado;
            BtnIniciarPausar.Text = pausado ? "Iniciar" : "Pausar";
        }

        //---------------------------------------------------------------------
        // Un ecosistema nuevo vuelve al estado inicial; el viejo queda sin
        // referencias y el recolector de basura lo libera. Respeta la pausa.
        //---------------------------------------------------------------------
        private void BtnReiniciar_Click(object sender, EventArgs e)
        {
            ecosistema = new CEcosistema(PbxAcuario.ClientSize.Width, PbxAcuario.ClientSize.Height);
            RefrescarAcuario();
        }

        // Si el acuario está lleno, Agregar devuelve false y no pasa nada;
        // los conteos ya lo muestran. Refrescar hace que se vea en pausa.
        private void BtnAgregarPlanta_Click(object sender, EventArgs e)
        {
            ecosistema.AgregarPlanta();
            RefrescarAcuario();
        }

        private void BtnAgregarHerbivoro_Click(object sender, EventArgs e)
        {
            ecosistema.AgregarHerbivoro();
            RefrescarAcuario();
        }

        private void BtnAgregarDepredador_Click(object sender, EventArgs e)
        {
            ecosistema.AgregarDepredador();
            RefrescarAcuario();
        }

        //---------------------------------------------------------------------
        // Fluidos. Se crea la primera vez que el panel se muestra, como el
        // acuario. La cuadrícula es fija (proporción de diseño de PbxFluido),
        // así que no depende del tamaño en ejecución.
        //---------------------------------------------------------------------
        private void PnlProyectoFIA_VisibleChanged(object sender, EventArgs e)
        {
            if (!PnlProyectoFIA.Visible)
            {
                TmrFluido.Stop();
                DescartarTrazoFluido();
                return;
            }

            if (fluido == null)
            {
                CrearFluido();
                relojFluido.Start();
            }

            // Volver al panel no anula una pausa del usuario.
            if (!fluidoPausado)
            {
                TmrFluido.Start();
            }
        }

        // Escena inicial: cuadrícula vacía; el chorro de AgregarChorro() la
        // pone en marcha desde el primer tick.
        private void CrearFluido()
        {
            int filas = CVistaFluido.FilasPara(ColumnasFluido);
            fluido = new CFluido(ColumnasFluido, filas, IteracionesFluido, OmegaFluido,
                                 1.0, DisipacionFluido);
            fluido.SetProyeccion(proyeccionFluido);
            fluido.SetObstaculo(obstaculoFluido);
            vistaFluido = new CVistaFluido(ColumnasFluido, filas);
            vistaFluido.SetVista(vistaElegidaFluido);
            vistaFluido.Actualizar(fluido);
        }

        //---------------------------------------------------------------------
        // Un tick: fuentes (chorro y ratón) + Paso() (cálculo), luego
        // Actualizar + Refresh() (dibujo). Refresh() pinta en el acto, así
        // que el cronómetro mide lo mismo que se ejecuta. Los ms miden el
        // coste del equipo; la simulación sigue contando en ticks.
        //---------------------------------------------------------------------
        private void TmrFluido_Tick(object sender, EventArgs e)
        {
            long inicio = relojFluido.ElapsedTicks;
            int n = muestrasTomadas % MuestrasMedicion;
            marcasTick[n] = inicio;

            // Con el obstáculo, la entrada de la izquierda sustituye al chorro.
            if (!obstaculoFluido)
            {
                fluido.AgregarChorro();
            }
            AplicarTrazoFluido();
            fluido.Paso();
            long trasPaso = relojFluido.ElapsedTicks;

            vistaFluido.Actualizar(fluido);
            PbxFluido.Refresh();
            long fin = relojFluido.ElapsedTicks;

            msPaso[n] = AMilisegundos(trasPaso - inicio);
            msDibujo[n] = AMilisegundos(fin - trasPaso);
            msTotal[n] = AMilisegundos(fin - inicio);
            muestrasTomadas++;

            ActualizarEtiquetaFluido();
        }

        //---------------------------------------------------------------------
        // Ratón. Los eventos solo guardan posiciones; el Tick las gasta. Así
        // no importa que MouseMove llegue más o menos veces que el Tick.
        // Mientras se arrastra, el control captura el ratón: si sale del
        // control, las posiciones siguen llegando y PantallaACelda las recorta.
        //---------------------------------------------------------------------
        private void PbxFluido_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || fluido == null || fluidoPausado)
            {
                return;
            }
            arrastrandoFluido = true;
            trazoFluido.Add(e.Location);
        }

        private void PbxFluido_MouseMove(object sender, MouseEventArgs e)
        {
            if (arrastrandoFluido)
            {
                trazoFluido.Add(e.Location);
            }
        }

        private void PbxFluido_MouseUp(object sender, MouseEventArgs e)
        {
            // Lo que quede en el trazo se gasta en el siguiente tick.
            if (e.Button == MouseButtons.Left)
            {
                arrastrandoFluido = false;
            }
        }

        private void DescartarTrazoFluido()
        {
            arrastrandoFluido = false;
            trazoFluido.Clear();
        }

        //---------------------------------------------------------------------
        // Gasta el trazo acumulado desde el último tick:
        // - Fuerza: el desplazamiento total del tick (último − primer punto)
        //   pasado a celdas, con su módulo recortado a TopeFuerzaRaton.
        // - Celdas: un disco de radio RadioPincel en cada celda del camino
        //   entre puntos seguidos, para que un arrastre rápido no deje huecos.
        // Cada celda recibe tinta y fuerza una sola vez por tick.
        //---------------------------------------------------------------------
        private void AplicarTrazoFluido()
        {
            if (trazoFluido.Count == 0)
            {
                return;
            }

            int nx = fluido.GetNx();
            int ny = fluido.GetNy();
            Size control = PbxFluido.ClientSize;
            Rectangle r = CVistaFluido.RectanguloDibujo(control, nx, ny);
            Point ultimo = trazoFluido[trazoFluido.Count - 1];

            if (r.Width > 0 && r.Height > 0)
            {
                Point primero = trazoFluido[0];
                double fu = (ultimo.X - primero.X) * (double)nx / r.Width;
                double fv = (ultimo.Y - primero.Y) * (double)ny / r.Height;
                double modulo = Math.Sqrt(fu * fu + fv * fv);
                if (modulo > TopeFuerzaRaton)
                {
                    fu *= TopeFuerzaRaton / modulo;
                    fv *= TopeFuerzaRaton / modulo;
                }

                celdasTrazo.Clear();
                Point anterior = CVistaFluido.PantallaACelda(primero, control, nx, ny);
                MarcarDisco(anterior);
                for (int s = 1; s < trazoFluido.Count; s++)
                {
                    Point actual = CVistaFluido.PantallaACelda(trazoFluido[s], control, nx, ny);
                    MarcarSegmento(anterior, actual);
                    anterior = actual;
                }

                foreach (Point celda in celdasTrazo)
                {
                    fluido.AsegurarTinta(celda.X, celda.Y, TintaPincel);
                    fluido.AgregarFuerza(celda.X, celda.Y, fu, fv);
                }
            }

            // El último punto es el inicio del trazo del siguiente tick.
            trazoFluido.Clear();
            if (arrastrandoFluido)
            {
                trazoFluido.Add(ultimo);
            }
        }

        // Recorre el segmento a pasos de una celda (los que pide el eje más
        // largo) y marca un disco en cada paso.
        private void MarcarSegmento(Point a, Point b)
        {
            int pasos = Math.Max(Math.Abs(b.X - a.X), Math.Abs(b.Y - a.Y));
            for (int s = 1; s <= pasos; s++)
            {
                double t = (double)s / pasos;
                int i = (int)Math.Round(a.X + (b.X - a.X) * t);
                int j = (int)Math.Round(a.Y + (b.Y - a.Y) * t);
                MarcarDisco(new Point(i, j));
            }
        }

        // Las celdas fuera del interior se guardan igual: AgregarTinta y
        // AgregarFuerza no hacen nada con ellas.
        private void MarcarDisco(Point centro)
        {
            for (int dj = -RadioPincel; dj <= RadioPincel; dj++)
            {
                for (int di = -RadioPincel; di <= RadioPincel; di++)
                {
                    if (di * di + dj * dj <= RadioPincel * RadioPincel)
                    {
                        celdasTrazo.Add(new Point(centro.X + di, centro.Y + dj));
                    }
                }
            }
        }

        //---------------------------------------------------------------------
        // Botones. La pausa es del usuario y es independiente de ocultar el
        // panel, como en el acuario.
        //---------------------------------------------------------------------
        private void BtnFluidoPausar_Click(object sender, EventArgs e)
        {
            fluidoPausado = !fluidoPausado;
            TmrFluido.Enabled = !fluidoPausado;
            BtnFluidoPausar.Text = fluidoPausado ? "Iniciar" : "Pausar";
            DescartarTrazoFluido();
        }

        // Un CFluido nuevo vuelve a la escena inicial; el viejo lo libera el
        // recolector de basura. Respeta la pausa: si está en pausa, se ve la
        // cuadrícula vacía y "Ticks: 0" hasta que se reanude.
        private void BtnFluidoReiniciar_Click(object sender, EventArgs e)
        {
            DescartarTrazoFluido();
            CrearFluido();
            PbxFluido.Refresh();
            ultimaEtiquetaMs = -1000;
            ActualizarEtiquetaFluido();
        }

        //---------------------------------------------------------------------
        // Etiqueta. Sin medición: "Ticks: N" en cada tick. Con medición: una
        // vez por segundo, los ticks/s y los ms de los últimos 60 ticks.
        //---------------------------------------------------------------------
        private void ActualizarEtiquetaFluido()
        {
            if (!MedirFluido)
            {
                LblFluido.Text = $"Ticks: {fluido.GetTicks()}";
                return;
            }

            if (relojFluido.ElapsedMilliseconds - ultimaEtiquetaMs >= 1000)
            {
                ultimaEtiquetaMs = relojFluido.ElapsedMilliseconds;
                MostrarMedicionFluido();
            }
        }

        //---------------------------------------------------------------------
        // Ticks/s y ms (media / máx) de los últimos 60 ticks. El total es la
        // suma de cada tick, no la suma de los máximos. "reloj" sirve para
        // saltarse los primeros 3 s (compilación JIT).
        //---------------------------------------------------------------------
        private void MostrarMedicionFluido()
        {
            int cuantas = Math.Min(muestrasTomadas, MuestrasMedicion);
            if (cuantas < 2)
            {
                LblFluido.Text = $"Ticks: {fluido.GetTicks()}";
                return;
            }

            // Ticks/s: (cuantas − 1) intervalos entre la marca más vieja y la
            // más nueva de la ventana.
            int masNueva = (muestrasTomadas - 1) % MuestrasMedicion;
            int masVieja = (muestrasTomadas - cuantas) % MuestrasMedicion;
            double segundos = (marcasTick[masNueva] - marcasTick[masVieja]) / (double)Stopwatch.Frequency;
            double ticksPorSegundo = segundos > 0 ? (cuantas - 1) / segundos : 0;

            string area = $"área {PbxFluido.ClientSize.Width}×{PbxFluido.ClientSize.Height}";
            string reloj = $"reloj {relojFluido.ElapsedMilliseconds / 1000} s";

            LblFluido.Text =
                $"Ticks: {fluido.GetTicks()} · ticks/s: {ticksPorSegundo:0.0} · " +
                $"ms paso: {Media(msPaso, cuantas):0.00} / {Maximo(msPaso, cuantas):0.00} · " +
                $"ms dibujo: {Media(msDibujo, cuantas):0.00} / {Maximo(msDibujo, cuantas):0.00} · " +
                $"ms total: {Media(msTotal, cuantas):0.00} / {Maximo(msTotal, cuantas):0.00} · " +
                $"{fluido.GetNx()}×{fluido.GetNy()}, k = {fluido.GetIteraciones()} · {area} · {reloj}";
        }

        private void PbxFluido_Paint(object sender, PaintEventArgs e)
        {
            if (vistaFluido != null)
            {
                vistaFluido.Dibujar(e.Graphics, PbxFluido.ClientSize, InterpolacionFluido);
            }
        }

        private static double AMilisegundos(long ticksReloj)
        {
            return ticksReloj * 1000.0 / Stopwatch.Frequency;
        }

        private static double Media(double[] valores, int cuantas)
        {
            double suma = 0;
            for (int i = 0; i < cuantas; i++) suma += valores[i];
            return suma / cuantas;
        }

        private static double Maximo(double[] valores, int cuantas)
        {
            double maximo = 0;
            for (int i = 0; i < cuantas; i++) maximo = Math.Max(maximo, valores[i]);
            return maximo;
        }

        //---------------------------------------------------------------------
        // Fase 2. Vista: Tinta → Presión → Vorticidad → Tinta. Proyección:
        // encendida o apagada. Los dos repintan en el acto para que el cambio
        // se vea también en pausa (el Tick no corre).
        //---------------------------------------------------------------------
        private void BtnFluidoVista_Click(object sender, EventArgs e)
        {
            switch (vistaElegidaFluido)
            {
                case CVistaFluido.Vista.Tinta:
                    vistaElegidaFluido = CVistaFluido.Vista.Presion;
                    BtnFluidoVista.Text = "Vista: Presión";
                    break;
                case CVistaFluido.Vista.Presion:
                    vistaElegidaFluido = CVistaFluido.Vista.Vorticidad;
                    BtnFluidoVista.Text = "Vista: Vorticidad";
                    break;
                default:
                    vistaElegidaFluido = CVistaFluido.Vista.Tinta;
                    BtnFluidoVista.Text = "Vista: Tinta";
                    break;
            }
            vistaFluido.SetVista(vistaElegidaFluido);
            RepintarFluido();
        }

        private void BtnFluidoProyección_Click(object sender, EventArgs e)
        {
            proyeccionFluido = !proyeccionFluido;
            fluido.SetProyeccion(proyeccionFluido);
            BtnFluidoProyección.Text = proyeccionFluido ? "Proyección: Sí" : "Proyección: No";
            RepintarFluido();
        }

        private void RepintarFluido()
        {
            vistaFluido.Actualizar(fluido);
            PbxFluido.Refresh();
        }

        // Fase 3. Activado: círculo, entrada a la izquierda y salida a la
        // derecha. Desactivado: caja cerrada con el chorro, como antes.
        private void BtnFluidoObstaculo_Click(object sender, EventArgs e)
        {
            obstaculoFluido = !obstaculoFluido;
            fluido.SetObstaculo(obstaculoFluido);
            BtnFluidoObstaculo.Text = obstaculoFluido ? "Obstáculo: Sí" : "Obstáculo: No";
            RepintarFluido();
        }
    }
}
