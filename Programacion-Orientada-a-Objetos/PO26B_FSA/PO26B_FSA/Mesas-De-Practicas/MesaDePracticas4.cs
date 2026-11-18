using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PO26B_FSA.Acuario;

namespace PO26B_FSA.Mesas_De_Practicas
{
    public partial class MesaDePracticas4 : Form
    {

        private CEcosistema ecosistema;
        // La pausa la decide el usuario; ocultar el panel es otra causa
        // (navegación). Por eso van separadas.
        private bool pausado;
        public MesaDePracticas4()
        {

            InitializeComponent();
            PnlProyectoF.Visible = false;
            PnlProyectoFIA.Visible = false;
            PbxAcuario.Paint += PbxAcuario_Paint;
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
    }
}
