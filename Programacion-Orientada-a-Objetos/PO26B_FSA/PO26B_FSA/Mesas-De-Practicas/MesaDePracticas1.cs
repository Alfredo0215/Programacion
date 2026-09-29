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
    public partial class MesaDePracticas1 : Form
    {
        public MesaDePracticas1()
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

        private void BtnP1Pnl1_Click(object sender, EventArgs e)
        {
            string Captura1 = TbxCaptura1.Text;
            string Captura2 = TbxCaptura2.Text;

            CelularAndroid Celular1 = new CelularAndroid(Captura1, Captura2);
            Celular1.Encender();
            MessageBox.Show("Celular encendido");

        }

        private void BtnP2Pnl1_Click(object sender, EventArgs e)
        {
            
        }

        private void BtnP1Pnl2_Click(object sender, EventArgs e)
        {
            CMujer mujer1 = new CMujer(25, 2.5);
            CHombre hombre1 = new CHombre(32, 3.2);

            hombre1.RegistrarPersona("Alfredo", "Fletes", "Sánchez", new DateTime(2007, 02, 15));
            MessageBox.Show(hombre1.ToString());


            mujer1.RegistrarPersona("Valentina", "Velázquez", "López", new DateTime(2007, 12, 14));
            MessageBox.Show(mujer1.ToString());
        }
        

        private void BtnP2Pnl2_Click(object sender, EventArgs e)
        {
        }

        private void BtnP3Pnl2_Click(object sender, EventArgs e)
        {
        }
    }
}
