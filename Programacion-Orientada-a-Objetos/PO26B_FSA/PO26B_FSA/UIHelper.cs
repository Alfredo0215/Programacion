using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PO26B_FSA
{
    internal static class UIHelper
    {
        //-----------------------------------------------------------------
        // Método que redondea las esquinas del control.
        //-----------------------------------------------------------------
        public static void RoundControl(Control control, int diameter, Color borderColor, int borderWidth)
        {

            // Método que permite redondear esquinas aunque el control cambie de tamaño
            GraphicsPath GetPath(Rectangle rect)
            {
                // El diámetro no puede pasar del lado más corto, o los arcos se cruzan.
                int d = Math.Min(diameter, Math.Min(rect.Width, rect.Height));

                GraphicsPath path = new GraphicsPath();

                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                // Cierra la figura considerando los 4 arcos de referencia.

                path.CloseFigure();
                return path;
            }


            void UpdatePath(object sender, EventArgs e)
            {
                Rectangle rect = control.ClientRectangle;

                // Genera un path que se destruirá al ejecutarse.

                using (GraphicsPath path = GetPath(rect))
                {
                    // Redibuja el control al modificar su región con el path que acabamos de crear
                    control.Region = new Region(path);
                }
                control.Invalidate();
            }
            void PaintBorder(object sender, PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                // El borde va dentro de los píxeles 0..Width-1. Con borderWidth = 1:
                // rectángulo (0, 0, Width-1, Height-1), como DrawRectangle.
                int mitad = borderWidth / 2;
                Rectangle rectBorde = new Rectangle(mitad, mitad,
                    control.ClientSize.Width - borderWidth,
                    control.ClientSize.Height - borderWidth);

                using (GraphicsPath path = GetPath(rectBorde))
                using (Pen lapiz = new Pen(borderColor, borderWidth))
                {
                    e.Graphics.DrawPath(lapiz, path);
                }
            }

            // Llama al método de actualizar la region cuando el evento resize detecte algo
            control.Resize += UpdatePath;
            control.Paint += PaintBorder;

            // LLama al método por primera vez para hacer el dibujado
            UpdatePath(null, EventArgs.Empty);
        }
    }
}
