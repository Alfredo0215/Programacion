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
        public MesaDePracticas2()
        {

            InitializeComponent();
            PnlPracticas1.Visible = false;
            PnlPracticas2.Visible = false;
            PnlPracticas3.Visible = false;
            PnlPracticas4.Visible = false;
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
        private void PnlPracticas1_MouseDown(object sender, MouseEventArgs e)
        {
            TbxCaptura1.Text = e.X.ToString();
            TbxCaptura2.Text = e.Y.ToString();
        }

        private void PnlPracticas1_MouseMove(object sender, MouseEventArgs e)
        {
            PbxDron.Left = e.X;
            PbxDron.Top = e.Y;

            TbxCaptura1.Text = PbxDron.Left.ToString();
            TbxCaptura2.Text = PbxDron.Top.ToString();

        }

        private void BtnP1Pnl1_Click(object sender, EventArgs e)
        {
            CDron Dron = new CDron(PnlPracticas1);
            Dron.Encender(2000);
        }
    }
}
