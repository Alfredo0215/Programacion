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
    public partial class MesaDePracticas3 : Form
    {
        public MesaDePracticas3()
        {

            InitializeComponent();
            PnlPracticas1.Visible = false;
            PnlPracticas2.Visible = false;
            PnlPracticas3.Visible = false;
            PnlPracticas4.Visible = false;
        }

        private void BtnPractica1_Click(object sender, EventArgs e)
        {
            PnlPracticas1.Visible = !PnlPracticas1.Visible;
            PnlPracticas2.Visible = false;
            PnlPracticas3.Visible = false;
            PnlPracticas4.Visible = false;
        }

        private void BtnPractica2_Click(object sender, EventArgs e)
        {
            PnlPracticas2.Visible = !PnlPracticas2.Visible;
            PnlPracticas1.Visible = false;
            PnlPracticas3.Visible = false;
            PnlPracticas4.Visible = false;
        }

        private void BtnPractica3_Click(object sender, EventArgs e)
        {
            PnlPracticas3.Visible = !PnlPracticas3.Visible;
            PnlPracticas1.Visible = false;
            PnlPracticas2.Visible = false;
            PnlPracticas4.Visible = false;

        }

        private void BtnPractica4_Click(object sender, EventArgs e)
        {
            PnlPracticas4.Visible = !PnlPracticas4.Visible;
            PnlPracticas1.Visible = false;
            PnlPracticas2.Visible = false;
            PnlPracticas3.Visible = false;
        }
    }
}
