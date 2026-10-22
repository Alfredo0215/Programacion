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
    public partial class FilaMesa : UserControl
    {
        //---------------------------------------------------------------------
        // Atributos.
        //---------------------------------------------------------------------
        // Colores provisionales: no son los RGB exactos de Terraria.
        private static readonly Color colorNormal = Color.FromArgb(52, 68, 126);
        private static readonly Color colorHover = Color.FromArgb(73, 94, 171);
        private static readonly Color colorPaneles = Color.FromArgb(43, 56, 101);
        


        // Avisa que se pulsó "abrir"; quien contiene la fila decide qué mesa abre.
        public event EventHandler AbrirClick;

        //---------------------------------------------------------------------
        // Constructor.
        //---------------------------------------------------------------------
        public FilaMesa()
        {
            InitializeComponent();
            BackColor = colorNormal;
            PnlStats.BackColor = colorPaneles;
            PnlTiempo.BackColor = colorPaneles;
            PnlDificultad.BackColor = colorPaneles;
            this.Cursor = Cursors.Hand;
            UIHelper.RoundControl(this, 30, Color.FromArgb(75, 98, 183), 2);
            UIHelper.RoundControl(PnlRetrato, 20, Color.FromArgb(87, 102, 156), 2);
            UIHelper.RoundControl(PnlDificultad, 20, Color.FromArgb(87, 102, 156), 2);
            UIHelper.RoundControl(PnlStats, 20, Color.FromArgb(87, 102, 156), 2);
            UIHelper.RoundControl(PnlTiempo, 20, Color.FromArgb(87, 102, 156), 2);

            

            ConectarEventos(this);
        }

        //---------------------------------------------------------------------
        // Rellena la fila con los datos de una mesa.
        //---------------------------------------------------------------------
        public void Configurar(string nombre, string dificultad, Color colorDificultad,
                               Image retrato, int vida, int mana, string tiempoJugado)
        {
            LblNombre.Text = nombre;
            LblNombre.ForeColor = colorDificultad;
            LblDificultad.Text = dificultad;
            LblDificultad.ForeColor = colorDificultad;
            PbxRetrato.Image = retrato;
            LblVida.Text = vida + " HP";
            LblMana.Text = mana + " MP";
            LblTiempo.Text = tiempoJugado;
        }

        //---------------------------------------------------------------------
        // Conecta el hover y el clic a la fila y a todos sus controles hijos.
        // En WinForms el Click de un hijo no llega al padre.
        //---------------------------------------------------------------------
        private void ConectarEventos(Control control)
        {
            control.MouseEnter += Fila_MouseEnter;
            control.MouseLeave += Fila_MouseLeave;
            // "this" ya tiene Click desde el diseñador; conectarlo otra vez
            // abriría la mesa dos veces.
            if (control != this)
            {
                control.Click += FilaMesa_Click;
            }
            foreach (Control hijo in control.Controls)
            {
                ConectarEventos(hijo);
            }
        }

        private void Fila_MouseEnter(object sender, EventArgs e)
        {
            BackColor = colorHover;
        }

        // Pasar de la fila a un hijo también dispara MouseLeave; solo se quita
        // el resaltado si el cursor salió de verdad del rectángulo de la fila.
        private void Fila_MouseLeave(object sender, EventArgs e)
        {
            if (!ClientRectangle.Contains(PointToClient(Cursor.Position)))
            {
                BackColor = colorNormal;
            }
        }

        //---------------------------------------------------------------------
        // Lanza AbrirClick; lo conecta el diseñador.
        //---------------------------------------------------------------------
        private void FilaMesa_Click(object sender, EventArgs e)
        {
            AbrirClick?.Invoke(this, EventArgs.Empty);

        }
    }
}
