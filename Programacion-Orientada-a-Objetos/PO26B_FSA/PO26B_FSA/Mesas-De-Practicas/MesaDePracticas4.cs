using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PO26B_FSA.Mesas_De_Practicas
{
    public partial class MesaDePracticas4 : Form
    {

        private int contador;
        public MesaDePracticas4()
        {

            InitializeComponent();
            PnlProyectoF.Visible = false;
            PnlProyectoFIA.Visible = false;
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

        }

        private void ActualizarLabel(Label label)
        {
            label.Text = $"Ticks: {contador.ToString()}";
        }

        private void PnlProyectoF_VisibleChanged(object sender, EventArgs e)
        {
            if (!PnlProyectoF.Visible)
            { TimerAcuario.Stop(); return; }

            TimerAcuario.Start();
        }
    }
}
