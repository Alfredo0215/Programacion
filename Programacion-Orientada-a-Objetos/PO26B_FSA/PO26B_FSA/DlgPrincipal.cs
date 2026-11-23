using PO26B_FSA.Mesas_De_Practicas;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PO26B_FSA
{
    public partial class DlgPrincipal : Form
    {
        //---------------------------------------------------------------------
        // Método constructor, configuración inicial del programa.
        //---------------------------------------------------------------------
        public DlgPrincipal()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            RedondearControles();
        }

        //---------------------------------------------------------------------
        // Esquinas redondeadas del área principal: chips, caja y botón.
        //---------------------------------------------------------------------
        private void RedondearControles()
        {
            Color borde = Color.FromArgb(221, 217, 206);   // #DDD9CE
            Color acento = Color.FromArgb(201, 100, 66);   // #C96442

            foreach (Button chip in new[] { BtnChip1, BtnChip2, BtnChip3, BtnChip4 })
            {
                // Diámetro = alto del chip: los extremos son semicírculos (píldora).
                UIHelper.RoundControl(chip, chip.Height, borde, 1);
            }

            UIHelper.RoundControl(PnlMensaje, 16, borde, 1);   // más ligero
            UIHelper.RoundControl(BtnEnviar, 10, acento, 1);
        }

        //---------------------------------------------------------------------
        // Barra lateral y chips: cada botón abre su mesa.
        //---------------------------------------------------------------------
        private void BtnMesa1_Click(object sender, EventArgs e)
        {
            AbrirMesa(new MesaDePracticas1());
        }

        private void BtnMesa2_Click(object sender, EventArgs e)
        {
            AbrirMesa(new MesaDePracticas2());
        }

        private void BtnMesa3_Click(object sender, EventArgs e)
        {
            AbrirMesa(new MesaDePracticas3());
        }

        private void BtnMesa4_Click(object sender, EventArgs e)
        {
            AbrirMesa(new MesaDePracticas4());
        }

        //---------------------------------------------------------------------
        // Abre una mesa ocultando el menú; al cerrarla, el menú vuelve.
        //---------------------------------------------------------------------
        private void AbrirMesa(Form mesa)
        {
            this.Hide();
            using (mesa)
            {
                mesa.ShowDialog();
            }
            this.Show();
        }
    }
}
