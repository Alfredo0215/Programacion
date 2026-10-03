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

        //---------------------------------------------------------------------
        // Método constructor de la clase.
        //---------------------------------------------------------------------

        public MesaDePracticas1()
        {

            InitializeComponent();
            PnlPracticas1.Visible = false;
            PnlPracticas2.Visible = false;
            PnlPracticas3.Visible = false;
            PnlPracticas4.Visible = false;
        }

        //---------------------------------------------------------------------
        // Botón que activa el panel de la práctica 1 y desactiva los demás.
        //---------------------------------------------------------------------
        private void BtnPractica1_Click(object sender, EventArgs e)
        {
            PnlPracticas1.Visible = !PnlPracticas1.Visible;
            PnlPracticas2.Visible = false;
            PnlPracticas3.Visible = false;
            PnlPracticas4.Visible = false;
        }

        //---------------------------------------------------------------------
        // Botón que activa el panel de la práctica 2 y desactiva los demás.
        //---------------------------------------------------------------------
        private void BtnPractica2_Click(object sender, EventArgs e)
        {
            PnlPracticas2.Visible = !PnlPracticas2.Visible;
            PnlPracticas1.Visible = false;
            PnlPracticas3.Visible = false;
            PnlPracticas4.Visible = false;
        }

        //---------------------------------------------------------------------
        // Botón que activa el panel de la práctica 3 y desactiva los demás.
        //---------------------------------------------------------------------
        private void BtnPractica3_Click(object sender, EventArgs e)
        {
            PnlPracticas3.Visible = !PnlPracticas3.Visible;
            PnlPracticas1.Visible = false;
            PnlPracticas2.Visible = false;
            PnlPracticas4.Visible = false;

        }

        //---------------------------------------------------------------------
        // Botón que activa el panel de la práctica 4 y desactiva los demás.
        //---------------------------------------------------------------------
        private void BtnPractica4_Click(object sender, EventArgs e)
        {
            PnlPracticas4.Visible = !PnlPracticas4.Visible;
            PnlPracticas1.Visible = false;
            PnlPracticas2.Visible = false;
            PnlPracticas3.Visible = false;
        }

        //---------------------------------------------------------------------
        // Botón 1 practica 1, crea el objeto Celular.
        //---------------------------------------------------------------------
        private void BtnP1Pnl1_Click(object sender, EventArgs e)
        {
            string Captura1 = TbxCaptura1.Text;
            string Captura2 = TbxCaptura2.Text;

            CelularAndroid Celular1 = new CelularAndroid(Captura1, Captura2);
            Celular1.Encender();
            MessageBox.Show("Celular encendido");

        }

        //---------------------------------------------------------------------
        // Botón 1 práctica 2, crea los objetos hombre y mujer y muestra sus atributos iniciales.
        //---------------------------------------------------------------------

        private void BtnP1Pnl2_Click(object sender, EventArgs e)
        {
            CMujer mujer1 = new CMujer(25, 2.5);
            CHombre hombre1 = new CHombre(32, 3.2);

            hombre1.RegistrarPersona("Alfredo", "Fletes", "Sánchez", new DateTime(2007, 02, 15));
            MessageBox.Show(hombre1.ToString());


            mujer1.RegistrarPersona("Valentina", "Velázquez", "López", new DateTime(2007, 12, 14));
            MessageBox.Show(mujer1.ToString());
        }

        //---------------------------------------------------------------------
        // Botón 2 práctica 2, crea los objetos hombre y mujer y muestra sus atributos iniciales.
        //---------------------------------------------------------------------
        private void BtnP2Pnl2_Click(object sender, EventArgs e)
        {

            CAnimal animal1 = new CAnimal(30, 30, 1);
            CPersona persona1 = new CPersona(30, 30, 1);
            CHombre hombre1 = new CHombre(30, 30);
            CMujer mujer1 = new CMujer(30, 30);

            MessageBox.Show(animal1.Respirar().ToString());
            MessageBox.Show(persona1.Respirar().ToString());

            MessageBox.Show(hombre1.Respirar().ToString());

            MessageBox.Show(mujer1.Respirar().ToString());

        }

        private void BtnP3Pnl2_Click(object sender, EventArgs e)
        {
            CMujer mujer1 = new CMujer(25, 2.5);
            CHombre hombre1 = new CHombre(32, 3.2);

            hombre1.RegistrarPersona("Alfredo", "Fletes", "Sánchez", new DateTime(2007, 02, 15));
            MessageBox.Show(hombre1.ToString());


            mujer1.RegistrarPersona("Valentina", "Velázquez", "López", new DateTime(2007, 12, 14));
            MessageBox.Show(mujer1.ToString());

            hombre1.IniciaProduccionEspermatozoides();

            DgvTabla2.Rows.Add(hombre1);
            DgvTabla2.Rows.Add(mujer1);
        }
    }
}
