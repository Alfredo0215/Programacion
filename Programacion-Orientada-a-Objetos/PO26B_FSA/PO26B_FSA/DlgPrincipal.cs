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
        int originalWidth;
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
        }

        //---------------------------------------------------------------------
        // Método que carga los elementos gráficos del programa.
        //---------------------------------------------------------------------
        private void DlgPrincipal_Load(object sender, EventArgs e)
        {
            originalHeight = PbxLogoMenu.Height;
            originalWidth = PbxLogoMenu.Width;
            originalX = PbxLogoMenu.Left;
            originalY = PbxLogoMenu.Top;
            StartLogoAnimation();
        }

        //---------------------------------------------------------------------
        // Botones de la barra lateral: cada uno abre su mesa.
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
        // 
        //---------------------------------------------------------------------
        private void AnimationLogoTick(object sender, EventArgs e)
        {
            double elapsed = stopwatch.Elapsed.TotalMilliseconds;

            double progress = Clamp(elapsed / animationDuration, 0.0, 1.0);

            double scale = GetScale(progress);

            ApplyScale(scale);

            // Al terminar un ciclo, reinicia el reloj; el timer sigue corriendo.
            if (progress >= 1.0)
            {
                stopwatch.Restart();
            }

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
        private static double Clamp(double valor, double min, double max)
        {
            if (valor < min) return min;
            if (valor > max) return max;
            return valor;
        }

        private void ApplyScale(double scale)
        {
            int newHeight = (int)(originalHeight * scale);
            int newWidth = (int)(originalWidth * scale);

            int newX = originalX - (newWidth - originalWidth) / 2;
            int newY = originalY - (newHeight - originalHeight) / 2;

            PbxLogoMenu.SetBounds(newX, newY, newWidth, newHeight);


        }
        private void StartLogoAnimation()
        {
            stopwatch.Restart();
            timerLogoAnimation.Start();
        }
    }
}
