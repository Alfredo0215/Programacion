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
    public partial class MesaDePracticas2 : Form
    {
        //---------------------------------------------------------------------
        // Atributos.
        //---------------------------------------------------------------------

        // Drones creados (no bajan al destruir uno).
        private int contadorDrones;
        private int contadorVigilantes;

        //---------------------------------------------------------------------
        // Métodos.
        //---------------------------------------------------------------------

        public MesaDePracticas2()
        {

            InitializeComponent();
            PnlPracticas1.Visible = false;
            PnlPracticas2.Visible = false;
            PnlPracticas3.Visible = false;
            PnlPracticas4.Visible = false;

            // TbxCaptura1 y 2 muestran los contadores de drones.
            contadorDrones = 0;
            contadorVigilantes = 0;
            TbxCaptura1.Text = contadorDrones.ToString();
            TbxCaptura2.Text = contadorVigilantes.ToString();
        }

        //---------------------------------------------------------------------
        // Alterna el panel elegido y oculta los otros tres. "mostrar" se
        // calcula antes de ocultar todo para que un segundo clic lo cierre.
        //---------------------------------------------------------------------
        private void AlternarPractica(Panel elegido)
        {
            bool mostrar = !elegido.Visible;
            PnlPracticas1.Visible = false;
            PnlPracticas2.Visible = false;
            PnlPracticas3.Visible = false;
            PnlPracticas4.Visible = false;
            elegido.Visible = mostrar;
        }

        private void BtnPractica1_Click(object sender, EventArgs e)
        {
            AlternarPractica(PnlPracticas1);
        }

        private void BtnPractica2_Click(object sender, EventArgs e)
        {
            AlternarPractica(PnlPracticas2);
        }

        private void BtnPractica3_Click(object sender, EventArgs e)
        {
            AlternarPractica(PnlPracticas3);
        }

        private void BtnPractica4_Click(object sender, EventArgs e)
        {
            AlternarPractica(PnlPracticas4);
        }
        // Sin uso desde que TbxCaptura1 y 2 son contadores. Para quitarlo:
        // diseñador → PnlPracticas1 → Eventos → borrar MouseDown, y luego
        // borrar este método.
        private void PnlPracticas1_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void PnlPracticas1_MouseMove(object sender, MouseEventArgs e)
        {
            PbxDron.Left = e.X;
            PbxDron.Top = e.Y;
        }

        private void BtnP1Pnl1_Click(object sender, EventArgs e)
        {
            CDron Dron = new CDron(PnlPracticas1);
            Dron.Encender(2000);

            contadorDrones++;
            TbxCaptura1.Text = contadorDrones.ToString();
        }

        private void BtnP2Pnl1_Click(object sender, EventArgs e)
        {
            CDronVigilante vigilante = new CDronVigilante(PnlPracticas1);
            vigilante.Encender(2000);

            contadorVigilantes++;
            TbxCaptura2.Text = contadorVigilantes.ToString();
        }
    }
}
