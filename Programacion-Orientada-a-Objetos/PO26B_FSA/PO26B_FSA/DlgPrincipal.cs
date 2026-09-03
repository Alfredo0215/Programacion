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
        private const double maxScale = 1.1;
        private const double animationDuration = 2000.0;
        private Stopwatch stopwatch = new Stopwatch();
        private Timer timerLogoAnimation = new Timer();
        int originalWidht;
        int originalHeight;
        int originalX;
        int originalY;

        public DlgPrincipal()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            PbxLogoMenu.SizeMode = PictureBoxSizeMode.Zoom;
            LblMesa1.TextAlign = ContentAlignment.MiddleCenter;
            LblMesa2.TextAlign = ContentAlignment.MiddleCenter;
            LblMesa3.TextAlign = ContentAlignment.MiddleCenter;
            LblMesa4.TextAlign = ContentAlignment.MiddleCenter;

            timerLogoAnimation.Interval = 16;

            timerLogoAnimation.Tick += AnimationLogoTick;
            


            LblMesa1.MouseEnter += LabelMouseEnter;
            LblMesa2.MouseEnter += LabelMouseEnter;
            LblMesa3.MouseEnter += LabelMouseEnter;
            LblMesa4.MouseEnter += LabelMouseEnter;

            LblMesa1.MouseLeave += LabelMouseLeave;
            LblMesa2.MouseLeave += LabelMouseLeave;
            LblMesa3.MouseLeave += LabelMouseLeave;
            LblMesa4.MouseLeave += LabelMouseLeave;
        }
        private void DlgPrincipal_Load(object sender, EventArgs e)
        {
            originalHeight = PbxLogoMenu.Height;
            originalWidht = PbxLogoMenu.Width;
            originalX = PbxLogoMenu.Left;
            originalY = PbxLogoMenu.Top;
            StartLogoAnimation();
        }


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
        private void LabelMouseEnter(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            Helper.UIHelper.SetLabelHover(label);
        }

        private void LabelMouseLeave(object sender, EventArgs e)
        {
            Label label = (Label)sender;

            Helper.UIHelper.SetLabelNormal(label);
        }

        private void LblMesa1_Click(object sender, EventArgs e)
        {
            MesaDePracticas1 Mesa1 = new MesaDePracticas1();
            Mesa1.Show();
        }
        private void LblMesa2_Click(object sender, EventArgs e)
        {
            MesaDePracticas2 Mesa2 = new MesaDePracticas2();
            Mesa2.Show();
        }

        private void LblMesa3_Click(object sender, EventArgs e)
        {
            MesaDePracticas3 Mesa3 = new MesaDePracticas3();
            Mesa3.Show();
        }

        private void LblMesa4_Click(object sender, EventArgs e)
        {
            MesaDePracticas4 Mesa4 = new MesaDePracticas4();
            Mesa4.Show();
        }

        private double GetScale(double progress)
        {
            return 1 + (maxScale - 1) * Math.Sin(Math.PI * progress);
        }

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
    }
}
