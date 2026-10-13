using PO26B_FSA.Mesas_De_Practicas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PO26B_FSA
{
    public partial class DlgPrincipal : Form
    {

        //---------------------------------------------------------------------
        // Inicialización de atributos
        //---------------------------------------------------------------------
        private const double maxScale = 1.1;
        private const double animationDuration = 2000.0;
        private Stopwatch stopwatch = new Stopwatch();
        private Timer timerLogoAnimation = new Timer();
        int originalWidht;
        int originalHeight;
        int originalX;
        int originalY;

        //---------------------------------------------------------------------
        // Método constructor, configuración inicial del programa.
        //---------------------------------------------------------------------
        public DlgPrincipal()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            PbxLogoMenu.SizeMode = PictureBoxSizeMode.Zoom;
            timerLogoAnimation.Interval = 16;

            timerLogoAnimation.Tick += AnimationLogoTick;

            Helper.UIHelper.AplicarEstiloTerraria(LblMesas);
            Helper.UIHelper.AplicarEstiloTerraria(LblAtras);

            // Esc vuelve del listado de mesas al menú principal.
            this.KeyPreview = true;
            this.KeyDown += DlgPrincipal_KeyDown;

            // Cada fila avisa que se pulsó "abrir"; aquí se decide qué mesa abre.
            UcMesa1.AbrirClick += (s, e) => AbrirMesa(new MesaDePracticas1());
            UcMesa2.AbrirClick += (s, e) => AbrirMesa(new MesaDePracticas2());
            UcMesa3.AbrirClick += (s, e) => AbrirMesa(new MesaDePracticas3());
            UcMesa4.AbrirClick += (s, e) => AbrirMesa(new MesaDePracticas4());
        }

        //---------------------------------------------------------------------
        // Método que carga los elementos gráficos del programa.
        //---------------------------------------------------------------------
        private void DlgPrincipal_Load(object sender, EventArgs e)
        {
            originalHeight = PbxLogoMenu.Height;
            originalWidht = PbxLogoMenu.Width;
            originalX = PbxLogoMenu.Left;
            originalY = PbxLogoMenu.Top;
            StartLogoAnimation();

            Helper.UIHelper.RoundControl(PnlLabelMesas, 30, Color.FromArgb(0, 0, 0), 2);

            // Datos provisionales: colores aproximados (no son los RGB de Terraria)
            // y cifras inventadas que crecen con la dificultad de la mesa.
            Image retrato1 = Properties.Resources.Personaje2;
            Image retrato2 = Properties.Resources.Personaje1;
            Image retrato3 = Properties.Resources.Personaje3;
            Image retrato4 = Properties.Resources.Personaje4;
            UcMesa1.Configurar("Mesa de prácticas 1", "Clásico", Color.Gainsboro, retrato1, 100, 20, "2:15:08");
            UcMesa2.Configurar("Mesa de prácticas 2", "Experto", Color.Gold, retrato2, 260, 80, "18:42:31");
            UcMesa3.Configurar("Mesa de prácticas 3", "Maestro", Color.Crimson, retrato3, 400, 160, "63:07:54");
            UcMesa4.Configurar("Mesa de prácticas 4", "Legendario", Color.MediumPurple, retrato4, 500, 200, "152:39:12");
        }

        //---------------------------------------------------------------------
        // 
        //---------------------------------------------------------------------
        private void AnimationLogoTick(object sender, EventArgs e)
        {
            double elapsed = stopwatch.Elapsed.TotalMilliseconds;

            double progress = Clamp(elapsed / animationDuration, 0.0, 1.0);

            double scale = GetScale(progress);

            ApplyScale(scale);

            if (progress >= 1.0)
            {
                timerLogoAnimation.Stop();
                stopwatch.Restart();
                timerLogoAnimation.Start();
            }

        }
        //---------------------------------------------------------------------
        // Muestra el listado de mesas y oculta el menú.
        //---------------------------------------------------------------------
        private void LblMesas_Click(object sender, EventArgs e)
        {
            PnlMenu.Visible = false;
            PnlLabelMesas.Visible = true;
            PnlMesas.Visible = true;
            LblAtras.Visible = true;
        }

        //---------------------------------------------------------------------
        // Vuelve del listado de mesas al menú principal con Esc.
        //---------------------------------------------------------------------
        private void DlgPrincipal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && PnlMesas.Visible)
            {
                VolverAlMenu();
            }
        }

        //---------------------------------------------------------------------
        // Vuelve del listado de mesas al menú principal con el botón Atrás.
        //---------------------------------------------------------------------
        private void LblAtras_Click(object sender, EventArgs e)
        {
            VolverAlMenu();
        }

        //---------------------------------------------------------------------
        // Oculta el listado de mesas y muestra el menú.
        //---------------------------------------------------------------------
        private void VolverAlMenu()
        {
            PnlMesas.Visible = false;
            PnlLabelMesas.Visible = false;
            LblAtras.Visible = false;
            PnlMenu.Visible = true;
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

        private double GetScale(double progress)
        {
            return 1 + (maxScale - 1) * Math.Sin(Math.PI * progress);
        }

        //---------------------------------------------------------------------
        // Método Clamp personalizado.
        //---------------------------------------------------------------------
        public static double Clamp(double valor, double min, double max)
        {
            if (valor < min) return min;
            if (valor > max) return max;
            return valor;
        }

        private void ApplyScale(double scale)
        {
            int newHeight = (int)(originalHeight * scale);
            int newWidth = (int)(originalWidht * scale);

            int newX = originalX - (newWidth - originalWidht) / 2;
            int newY = originalY - (newHeight - originalHeight) / 2;

            PbxLogoMenu.SetBounds(newX, newY, newWidth, newHeight);


        }
        private void StartLogoAnimation()
        {
            stopwatch.Restart();
            timerLogoAnimation.Start();
        }

        private void filaMesa3_Load(object sender, EventArgs e)
        {

        }

    }
}
