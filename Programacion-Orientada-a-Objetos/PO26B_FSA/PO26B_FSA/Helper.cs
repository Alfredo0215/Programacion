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
            //-----------------------------------------------------------------
            // Dibuja el texto del label con sombra y borde oscuro (estilo
            // Terraria) y anima suavemente el hover: crece y pasa a amarillo.
            //-----------------------------------------------------------------
            public static void AplicarEstiloTerraria(Label label)
            {
                string texto = label.Text;
                FontFamily familia = label.Font.FontFamily;
                FontStyle estilo = label.Font.Style;
                float progreso = 0f;
                bool hover = false;
                Timer timer = new Timer { Interval = 16 };

                // El texto lo dibuja el Paint; se vacía para que el Label no lo pinte dos veces.
                label.Text = string.Empty;
                label.Cursor = Cursors.Hand;

                timer.Tick += (s, e) =>
                {
                    progreso += hover ? 0.15f : -0.15f;
                    if (progreso >= 1f || progreso <= 0f)
                    {
                        progreso = Math.Max(0f, Math.Min(1f, progreso));
                        timer.Stop();
                    }
                    label.Invalidate();
                };

                label.MouseEnter += (s, e) => { hover = true; timer.Start(); };
                label.MouseLeave += (s, e) => { hover = false; timer.Start(); };
                label.Disposed += (s, e) => timer.Dispose();

                label.Paint += (s, e) =>
                {
                    Graphics g = e.Graphics;
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    float puntos = MENU_FONT_NORMAL + (MENU_FONT_HOVER - MENU_FONT_NORMAL) * progreso;
                    float pixeles = puntos * g.DpiY / 72f;
                    Color relleno = Interpolar(Color.Gainsboro, Color.Yellow, progreso);

                    using (StringFormat formato = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    })
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddString(texto, familia, (int)estilo, pixeles, label.ClientRectangle, formato);

                        // Sombra desplazada
                        using (Matrix desplazar = new Matrix())
                        using (GraphicsPath sombra = (GraphicsPath)path.Clone())
                        {
                            desplazar.Translate(3, 3);
                            sombra.Transform(desplazar);
                            using (SolidBrush pincelSombra = new SolidBrush(Color.FromArgb(150, 0, 0, 0)))
                            {
                                g.FillPath(pincelSombra, sombra);
                            }
                        }

                        // Borde oscuro y relleno
                        using (Pen borde = new Pen(Color.FromArgb(20, 20, 30), 6f) { LineJoin = LineJoin.Round })
                        using (SolidBrush pincel = new SolidBrush(relleno))
                        {
                            g.DrawPath(borde, path);
                            g.FillPath(pincel, path);
                        }
                    }
                };
            }

            private static Color Interpolar(Color a, Color b, float t)
            {
                return Color.FromArgb(
                    (int)(a.R + (b.R - a.R) * t),
                    (int)(a.G + (b.G - a.G) * t),
                    (int)(a.B + (b.B - a.B) * t));
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
