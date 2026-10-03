using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PO26B_FSA
{
    public class Helper
    {
        private const float MENU_FONT_NORMAL = 25f;
        private const float MENU_FONT_HOVER = 35f;
        public static class UIHelper
        {
            public static void SetLabelHover(Label label)
            {
                label.Font = new Font(label.Font.FontFamily, MENU_FONT_HOVER, label.Font.Style);
                label.ForeColor = Color.Yellow;

            }
            public static void SetLabelNormal(Label label)
            {
                label.Font = new Font(label.Font.FontFamily, MENU_FONT_NORMAL, label.Font.Style);
                label.ForeColor = Color.Gainsboro;
            }


            //-----------------------------------------------------------------
            // Método que redondea las esquinas del control.
            //-----------------------------------------------------------------
            public static void RoundControl(Control control, int diameter, Color borderColor, int borderWidth)
            {

                // Método que permite redondear esquinas aunque el control cambie de tamaño
                GraphicsPath GetPath(Rectangle rect)
                {
                    GraphicsPath path = new GraphicsPath();

                    path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
                    path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
                    path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
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
                    Rectangle rectBorde = control.ClientRectangle;
                    rectBorde.Width -= borderWidth/2;
                    rectBorde.Height -= borderWidth/2;

                    using (GraphicsPath path = GetPath(rectBorde))
                    {
                        using (Pen lapiz = new Pen(borderColor, borderWidth))
                        {
                            e.Graphics.DrawPath(lapiz, path);
                        }
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
}
