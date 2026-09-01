using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PE26A_FSA_
{
    public partial class DlgMesaPracticas3 : Form
    {
        public DlgMesaPracticas3()
        {
            InitializeComponent();
        }

        //---------------------------------------------------------------------
        //Toggle de paneles 
        //---------------------------------------------------------------------

        //---------------------------------------------------------------------
        //Muestra el panel 1 y esconde los demás.
        //---------------------------------------------------------------------
        private void BtnPractica1_Click(object sender, EventArgs e)
        {

            //NOTA: La siguiente es una linea simplificada.
            // PnlPracticas1.Visible = !PnlPracticas1.Visible;

            if (PnlPracticas1.Visible)
            {
                PnlPracticas1.Visible = false;
            }
            else
            {
                PnlPracticas1.Visible = true;
                PnlPracticas2.Visible = false;
                PnlPracticas3.Visible = false;
                PnlPracticas4.Visible = false;
            }
            
        }

        //---------------------------------------------------------------------
        //Muestra el panel 2 y esconde los demás.
        //---------------------------------------------------------------------
        private void BtnPractica2_Click(object sender, EventArgs e)
        {
            if (PnlPracticas2.Visible)
            {
                PnlPracticas2.Visible = false;
            }
            else
            {
                PnlPracticas2.Visible = true;
                PnlPracticas1.Visible = false;
                PnlPracticas3.Visible = false;
                PnlPracticas4.Visible = false;
            }

        }

        //---------------------------------------------------------------------
        //Muestra el panel 3 y esconde los demás.
        //---------------------------------------------------------------------
        private void BtnPractica3_Click(object sender, EventArgs e)
        {
            if (PnlPracticas3.Visible)
            {
                PnlPracticas3.Visible = false;
            }
            else
            {
                PnlPracticas3.Visible = true;
                PnlPracticas1.Visible = false;
                PnlPracticas2.Visible = false;
                PnlPracticas4.Visible = false;
            }

        }

        //---------------------------------------------------------------------
        //Muestra el panel 4 y esconde los demás.
        //---------------------------------------------------------------------
        private void BtnPractica4_Click(object sender, EventArgs e)
        {
            if (PnlPracticas4.Visible)
            {
                PnlPracticas4.Visible = false;
            }
            else
            {
                PnlPracticas4.Visible = true;
                PnlPracticas1.Visible = false;
                PnlPracticas2.Visible = false;
                PnlPracticas3.Visible = false;
            }
        }
    }
}
