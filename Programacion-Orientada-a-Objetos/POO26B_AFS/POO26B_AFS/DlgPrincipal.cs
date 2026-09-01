using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POO26B_AFS
{
    //---------------------------------------------------------------------
    // Diálogo del menú principal del proyecto.
    // Ingeniería en Computación.
    // Centro Universitario de la Costa.
    // FSA 25/08/2026.
    //---------------------------------------------------------------------
    public partial class DlgPrincipal : Form
    {
        //---------------------------------------------------------------------
        // Atributos.
        //---------------------------------------------------------------------

        private string TituloDialogo;
        //---------------------------------------------------------------------
        // Método Constructor.
        //---------------------------------------------------------------------
        public DlgPrincipal()
        {
            InitializeComponent();

            TituloDialogo = "POO 26B - Fletes Sánchez Alfredo";
            Text = TituloDialogo;
        }
    }
}
