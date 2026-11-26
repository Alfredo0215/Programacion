using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace PO26B_FSA.Fluidos
{
    //-------------------------------------------------------------------------
    // Traduce los campos de CFluido a color y los dibuja. La cuadrícula es
    // fija: se dibuja en el mayor rectángulo con su proporción que cabe en el
    // control, centrado (letterbox). Las franjas quedan con el fondo del
    // control, así que no se deforma al maximizar.
    //-------------------------------------------------------------------------
    internal class CVistaFluido
    {
        //---------------------------------------------------------------------
        // Atributos.
        //---------------------------------------------------------------------

        // Tamaño de diseño de PbxFluido (lo anotó Alfredo en la Fase 0). De
        // aquí sale la proporción de la cuadrícula.
        public const int AnchoDiseno = 910;
        public const int AltoDiseno = 336;

        // d₀ de la fórmula de la tinta [Validar en Fase 0, a la vista].
        private const double EscalaTinta = 1.0;

        // Mínimo de la escala de ColorDivergente: con el campo todo en 0 al
        // arrancar, evita dividir 0 entre 0 (NaN).
        private const double EscalaMinima = 1e-6;
        // Cuánto conserva la escala de un tick al siguiente: suaviza los
        // cambios del máximo para que los colores no parpadeen.
        private const double MemoriaEscala = 0.95;
        // Intensidad de ColorDivergente = |s|^ExponenteColor. 1 = lineal (lo
        // vigente). La lista previa de la Fase 4 puede bajarlo a 0.7 si en el
        // proyector se ve pálido; 0.5 (raíz) se probó y se descartó.
        private const double ExponenteColor = 1.0;

        private const string TextoAviso = "Sin proyección: presión congelada";

        public static readonly Color Fondo = Color.FromArgb(0xF5, 0xF4, 0xEE);
        public static readonly Color Ocre = Color.FromArgb(0x8C, 0x64, 0x20);
        public static readonly Color Ciruela = Color.FromArgb(0x6B, 0x4E, 0x7A);
        private static readonly Color Oscuro = Color.FromArgb(0x2B, 0x2A, 0x27);
        private static readonly Color Borde = Color.FromArgb(0xDD, 0xD9, 0xCE);
        private static readonly Font FuenteAviso = new Font("Segoe UI", 10f);

        // Qué campo se pinta. Presión y vorticidad usan ColorDivergente:
        // positivo ocre (presión alta, giro horario en pantalla), negativo
        // ciruela.
        public enum Vista { Tinta, Presion, Vorticidad }

        private readonly int nx;
        private readonly int ny;
        private readonly Bitmap imagen;     // Un píxel por celda; se crea una vez.
        private readonly int[] pixeles;     // Búfer intermedio para LockBits.
        private readonly double[] campo;    // Presión centrada, mismo índice que CFluido.
        private readonly ImageAttributes atributos;

        private Vista vista = Vista.Tinta;
        private double escala;              // e de ColorDivergente; vuelve a 0 al cambiar de vista.
        private bool presionCongelada;      // Vista Presión con la proyección apagada.

        //---------------------------------------------------------------------
        // Constructor.
        //---------------------------------------------------------------------
        public CVistaFluido(int nx, int ny)
        {
            this.nx = nx;
            this.ny = ny;
            imagen = new Bitmap(nx, ny, PixelFormat.Format32bppArgb);
            pixeles = new int[nx * ny];
            campo = new double[(nx + 2) * (ny + 2)];

            // Con Bilinear, GDI+ mezcla el borde de la imagen con transparente;
            // repetirla en espejo evita ese marco desvanecido.
            atributos = new ImageAttributes();
            atributos.SetWrapMode(WrapMode.TileFlipXY);
        }

        public Vista GetVista() { return vista; }

        // Cambiar de vista reinicia la escala: la presión y la vorticidad
        // tienen tamaños muy distintos.
        public void SetVista(Vista nueva)
        {
            vista = nueva;
            escala = 0;
        }

        //---------------------------------------------------------------------
        // Filas para una cuadrícula de "columnas" con la proporción de diseño:
        // redondeo(columnas × alto / ancho).
        //---------------------------------------------------------------------
        public static int FilasPara(int columnas)
        {
            return (int)Math.Round(columnas * (double)AltoDiseno / AnchoDiseno,
                                   MidpointRounding.AwayFromZero);
        }

        //---------------------------------------------------------------------
        // Tinta → color. t = 1 − e^(−d/d₀) lleva [0, ∞) a [0, 1). Hasta 0.7 va
        // del fondo al ocre; de 0.7 a 1, del ocre al oscuro. Cada tramo se
        // reescala a [0, 1] para que no haya salto en t = 0.7.
        //---------------------------------------------------------------------
        public static Color ColorTinta(double d)
        {
            if (d < 0) d = 0;
            double t = 1.0 - Math.Exp(-d / EscalaTinta);

            if (t <= 0.7)
            {
                return Mezclar(Fondo, Ocre, t / 0.7);
            }
            return Mezclar(Ocre, Oscuro, (t - 0.7) / 0.3);
        }

        //---------------------------------------------------------------------
        // Campo con signo → color. s = x / escala, recortado a [−1, 1]:
        // 0 es el fondo, +1 el ocre y −1 el ciruela; intensidad |s|^ExponenteColor
        // (lineal con 1). La raíz se probó y dejaba la caja entera coloreada,
        // con líneas finas en el 0 que parecían bordes (ver Registro). Una
        // escala que no es positiva (o NaN) da el fondo en vez de un color
        // inválido.
        //---------------------------------------------------------------------
        public static Color ColorDivergente(double x, double escala)
        {
            if (!(escala > 0))
            {
                return Fondo;
            }
            double s = x / escala;
            if (double.IsNaN(s))
            {
                return Fondo;
            }
            return Mezclar(Fondo, s >= 0 ? Ocre : Ciruela, Math.Pow(Math.Abs(s), ExponenteColor));
        }

        //---------------------------------------------------------------------
        // Resta a las celdas interiores su media y lo escribe en "destino".
        // En la caja cerrada el nivel de la presión es arbitrario (sumarle
        // una constante da la misma velocidad); centrada, el signo significa
        // "más alta o más baja que la media".
        //---------------------------------------------------------------------
        public static void CentrarInterior(double[] origen, double[] destino, int nx, int ny)
        {
            int ancho = nx + 2;
            double suma = 0;
            for (int j = 1; j <= ny; j++)
            {
                for (int i = 1; i <= nx; i++)
                {
                    suma += origen[i + ancho * j];
                }
            }

            double media = suma / (nx * ny);
            for (int j = 1; j <= ny; j++)
            {
                for (int i = 1; i <= nx; i++)
                {
                    destino[i + ancho * j] = origen[i + ancho * j] - media;
                }
            }
        }

        //---------------------------------------------------------------------
        // Mayor rectángulo con proporción nx:ny que cabe en "control",
        // centrado. PantallaACelda usa este mismo rectángulo.
        //---------------------------------------------------------------------
        public static Rectangle RectanguloDibujo(Size control, int nx, int ny)
        {
            double escala = Math.Min((double)control.Width / nx, (double)control.Height / ny);
            int ancho = (int)Math.Round(nx * escala);
            int alto = (int)Math.Round(ny * escala);
            int x = (control.Width - ancho) / 2;
            int y = (control.Height - alto) / 2;
            return new Rectangle(x, y, ancho, alto);
        }

        //---------------------------------------------------------------------
        // Píxel del control → celda interior. Con r = RectanguloDibujo:
        // i = 1 + ⌊(x − r.X) · nx / r.Width⌋, recortada a [1, nx] (y lo mismo
        // en vertical). Un punto en las franjas o fuera del control cae en la
        // celda del borde más cercana. Se calcula con el tamaño actual del
        // control, así que maximizar no rompe la puntería.
        //---------------------------------------------------------------------
        public static Point PantallaACelda(Point punto, Size control, int nx, int ny)
        {
            Rectangle r = RectanguloDibujo(control, nx, ny);
            if (r.Width <= 0 || r.Height <= 0)
            {
                return new Point(1, 1);     // Control minimizado: no hay dónde apuntar.
            }

            int i = 1 + (int)Math.Floor((punto.X - r.X) * (double)nx / r.Width);
            int j = 1 + (int)Math.Floor((punto.Y - r.Y) * (double)ny / r.Height);
            return new Point(Recortar(i, 1, nx), Recortar(j, 1, ny));
        }

        private static int Recortar(int valor, int minimo, int maximo)
        {
            return Math.Max(minimo, Math.Min(maximo, valor));
        }

        //---------------------------------------------------------------------
        // Rellena la imagen con el campo de la vista actual en las celdas
        // interiores. La celda (i, j) es el píxel (i − 1, j − 1).
        // Presión y vorticidad: escala e ← máx(máx|x|, 0.95·e, ε), con el
        // máximo tomado sin el anillo de borde (allí las paredes dan valores
        // artificiales que fijarían la escala).
        //---------------------------------------------------------------------
        public void Actualizar(CFluido fluido)
        {
            presionCongelada = vista == Vista.Presion && !fluido.GetProyeccion();
            int n = 0;

            if (vista == Vista.Tinta)
            {
                double[] tinta = fluido.GetTinta();
                for (int j = 1; j <= ny; j++)
                {
                    for (int i = 1; i <= nx; i++)
                    {
                        pixeles[n++] = ColorTinta(tinta[fluido.IX(i, j)]).ToArgb();
                    }
                }
            }
            else
            {
                double[] x;
                if (vista == Vista.Presion)
                {
                    CentrarInterior(fluido.GetPresion(), campo, nx, ny);
                    x = campo;
                }
                else
                {
                    x = fluido.Vorticidad();
                }

                escala = Math.Max(Math.Max(MaxAbsSinAnillo(x), MemoriaEscala * escala), EscalaMinima);
                for (int j = 1; j <= ny; j++)
                {
                    for (int i = 1; i <= nx; i++)
                    {
                        pixeles[n++] = ColorDivergente(x[fluido.IX(i, j)], escala).ToArgb();
                    }
                }
            }

            // El obstáculo (Fase 3) se pinta encima en el color de los bordes,
            // en cualquier vista. La celda k = i + (nx + 2)·j es el píxel
            // (i − 1, j − 1).
            int colorSolido = Borde.ToArgb();
            foreach (int k in fluido.GetCeldasSolidas())
            {
                int i = k % (nx + 2);
                int j = k / (nx + 2);
                pixeles[(j - 1) * nx + (i - 1)] = colorSolido;
            }

            BitmapData datos = imagen.LockBits(new Rectangle(0, 0, nx, ny),
                                               ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            try
            {
                // Cada fila del bitmap puede tener relleno al final (Stride),
                // así que se copia fila por fila.
                for (int fila = 0; fila < ny; fila++)
                {
                    IntPtr destino = datos.Scan0 + fila * datos.Stride;
                    Marshal.Copy(pixeles, fila * nx, destino, nx);
                }
            }
            finally
            {
                imagen.UnlockBits(datos);
            }
        }

        //---------------------------------------------------------------------
        // Dibuja la imagen agrandada al rectángulo con letterbox. El fondo de
        // las franjas lo pinta el propio control (su BackColor).
        // PixelOffsetMode.Half evita que GDI+ desplace media celda al agrandar.
        //---------------------------------------------------------------------
        public void Dibujar(Graphics g, Size control, InterpolationMode interpolacion)
        {
            Rectangle destino = RectanguloDibujo(control, nx, ny);
            g.InterpolationMode = interpolacion;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            g.DrawImage(imagen, destino, 0, 0, nx, ny, GraphicsUnit.Pixel, atributos);

            if (presionCongelada)
            {
                DibujarAviso(g, destino);
            }
        }

        //---------------------------------------------------------------------
        // Chip "Sin proyección: presión congelada" en la esquina superior
        // izquierda del rectángulo de dibujo: píldora (diámetro = alto) con
        // fondo y borde de la paleta. Se dibuja con Graphics, no con Region,
        // así que el suavizado funciona y la curva no sale dentada.
        //---------------------------------------------------------------------
        private static void DibujarAviso(Graphics g, Rectangle destino)
        {
            g.PixelOffsetMode = PixelOffsetMode.Default;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            SizeF texto = g.MeasureString(TextoAviso, FuenteAviso);
            int alto = (int)Math.Ceiling(texto.Height) + 8;
            int ancho = (int)Math.Ceiling(texto.Width) + alto;   // Medio círculo a cada lado.
            Rectangle chip = new Rectangle(destino.X + 8, destino.Y + 8, ancho, alto);

            using (GraphicsPath forma = new GraphicsPath())
            using (SolidBrush relleno = new SolidBrush(Fondo))
            using (Pen borde = new Pen(Borde))
            using (SolidBrush letra = new SolidBrush(Oscuro))
            {
                forma.AddArc(chip.X, chip.Y, alto, alto, 90, 180);
                forma.AddArc(chip.Right - alto, chip.Y, alto, alto, 270, 180);
                forma.CloseFigure();

                g.FillPath(relleno, forma);
                g.DrawPath(borde, forma);
                g.DrawString(TextoAviso, FuenteAviso, letra,
                             chip.X + alto / 2f, chip.Y + (alto - texto.Height) / 2f);
            }
        }

        private double MaxAbsSinAnillo(double[] x)
        {
            int ancho = nx + 2;
            double maximo = 0;
            for (int j = 2; j <= ny - 1; j++)
            {
                for (int i = 2; i <= nx - 1; i++)
                {
                    maximo = Math.Max(maximo, Math.Abs(x[i + ancho * j]));
                }
            }
            return maximo;
        }

        private static Color Mezclar(Color a, Color b, double t)
        {
            if (t < 0) t = 0;
            if (t > 1) t = 1;
            return Color.FromArgb(
                (int)Math.Round(a.R + (b.R - a.R) * t),
                (int)Math.Round(a.G + (b.G - a.G) * t),
                (int)Math.Round(a.B + (b.B - a.B) * t));
        }
    }
}
