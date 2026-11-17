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

        private int contador;
        private CEcosistema ecosistema;
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
            contador++;

            ActualizarLabel(LblTicks);

            // El Timer solo arranca después de crear el ecosistema
            // (PnlProyectoF_VisibleChanged), así que aquí nunca es null.
            ecosistema.Paso();
            PbxAcuario.Invalidate();

        }

        private void ActualizarLabel(Label label)
        {
            label.Text = $"Ticks: {contador.ToString()}";
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
                PbxAcuario.Invalidate();
            }

            TimerAcuario.Start();
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
    }
}
