using PO26B_FSA.Mesas_De_Practicas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PO26B_FSA
{
    public partial class DlgPrincipal : Form
    {
        public DlgPrincipal()
        {
            InitializeComponent();
        }

        private void BtnMesaPractica1_Click(object sender, EventArgs e)
        {
            MesaDePracticas1 Mesa1 = new MesaDePracticas1();
            Mesa1.Show();
        }

        private void BtnMesaPracticas2_Click(object sender, EventArgs e)
        {
            MesaDePracticas2 Mesa2 = new MesaDePracticas2();
            Mesa2.Show();
        }

        private void BtnMesaPracticas3_Click(object sender, EventArgs e)
        {
            MesaDePracticas3 Mesa3 = new MesaDePracticas3();
            Mesa3.Show();
        }

        private void BtnMesaPracticas4_Click(object sender, EventArgs e)
        {
            MesaDePracticas4 Mesa4 = new MesaDePracticas4();
            Mesa4.Show();
        }

        private void LblMesa1_MouseHover(object sender, EventArgs e)
        {

        }
    }
}
