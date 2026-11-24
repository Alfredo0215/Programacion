using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PO26B_FSA
{
    internal class CDron : PictureBox
    {

        //---------------------------------------------------------------------
        //Atributos.
        //---------------------------------------------------------------------

        private static Random numRandom = new Random();
        //Atributos de nacimiento
        protected int posicionInicialX;
        protected int posicionInicialY;
        //Atributos del área de desplazamiento:

        protected Control areaDesplazamiento;
        protected bool norte;
        protected bool sur;
        protected bool este;
        protected bool oeste;

        //Atributos de control

        // true mientras el dron se destruye: ya no responde a clics.
        protected bool inhabilitado;
        protected volatile bool encendido;
        protected volatile bool destruido;

        protected bool regresarACasa;

        //Atributos de ejecución

        // volatile: lo asigna el hilo de arranque y lo lee el clic.
        protected volatile Thread procesoNavegacion;

        //Atributos de configuración

        protected const int TAMAÑO = 50;

        // Gris = quieto (apagado o pausado); el color de movimiento lo puede
        // cambiar una subclase.
        protected Color colorQuieto = Color.FromArgb(122, 119, 110);
        protected Color colorEnMovimiento = Color.FromArgb(201, 100, 66);
        //---------------------------------------------------------------------
        //Métodos.
        //---------------------------------------------------------------------


        public CDron(Control areaDesplazamiento)
        {

            //Recibe parámetros
            this.areaDesplazamiento = areaDesplazamiento;
            this.areaDesplazamiento.Controls.Add(this);

            //Calcula posición inicial
            posicionInicialX = numRandom.Next(0, this.areaDesplazamiento.Width - TAMAÑO);
            posicionInicialY = numRandom.Next(0, this.areaDesplazamiento.Height - TAMAÑO);

            //Inicia atributos de nagevación

            norte = false;
            sur = true;
            este = true;
            oeste = false;

            //Inicia atributos de control

            encendido = false;
            inhabilitado = false;
            regresarACasa = false;

            //Inicializa atributos de ejecución

            

            //Inicializa otras propiedades 
   
            Location = new System.Drawing.Point(this.posicionInicialX, this.posicionInicialY);
            Name = "DRON";
            Size = new System.Drawing.Size(50, 50);
            // Estado apagado: texto secundario #7A776E (Fase 6b, variante sonnet).
            BackColor = colorQuieto;
            BringToFront();
           
            Click += new EventHandler(Accion);

            CheckForIllegalCrossThreadCalls = false;
        }

        public void Encender(int tiempoArranque)
        {
            Thread procesoEncendido;

            procesoEncendido = new Thread(() => {
                Thread.Sleep(tiempoArranque);

                // La mesa pudo cerrarse mientras el hilo dormía.
                if (destruido)
                    return;

                // Parpadea 4 veces antes de moverse.
                for (int i = 0; i < 4; i++)
                {
                    Visible = false;
                    Thread.Sleep(200);
                    if (destruido)
                        return;

                    Visible = true;
                    Thread.Sleep(200);
                    if (destruido)
                        return;
                }

                // Estado encendido: acento terracota #C96442.
                BackColor = colorEnMovimiento;
                encendido = true;
                Desplazar(numRandom.Next(1, 50));
            });

            procesoEncendido.IsBackground = true;
            procesoEncendido.Start();
        }

        // virtual: una subclase (p. ej. el vigilante) puede reemplazar la
        // forma de moverse.
        public virtual void Desplazar(int velocidad)
        {

            int x;
            int y;

            procesoNavegacion = new Thread(() =>
            {

                // Un solo hilo por dron: en pausa no termina, solo espera.
                // Así reanudar no crea un segundo hilo.
                while (!destruido)
                {
                    if (!encendido)
                    {
                        Thread.Sleep(velocidad);
                        continue;
                    }

                    x = Location.X;
                    y = Location.Y;

                    if (norte)
                        y = y - 1;
                    if (sur)
                        y = y + 1;
                    if (este)
                        x = x +1;
                    if (oeste)
                        x = x - 1;

                    CambiarDireccion(x, y);

                    Location = new Point(x, y);
                    Thread.Sleep(velocidad);
                }
            });

            procesoNavegacion.IsBackground = true;
            procesoNavegacion.Start();
        }

        //---------------------------------------------------------------------
        // Decide la dirección según la posición (x, y) recién calculada.
        // Este dron rebota en diagonal; una subclase puede cambiar la regla.
        //---------------------------------------------------------------------
        protected virtual void CambiarDireccion(int x, int y)
        {
            if (x <= 0)
            {
                oeste = false;
                este = true;
            }

            if (x >= areaDesplazamiento.Width - TAMAÑO)
            {
                oeste = true;
                este = false;
            }

            if (y <= 0)
            {
                norte = false;
                sur = true;
            }

            if (y >= areaDesplazamiento.Height - TAMAÑO)
            {
                sur = false;
                norte = true;
            }
        }

        //---------------------------------------------------------------------
        // Clic: pausa el dron si se mueve y lo reanuda si está en pausa.
        // Shift+clic: lo destruye.
        //---------------------------------------------------------------------
        private void Accion(object sender, EventArgs e)
        {
            // Aún arrancando, o ya destruyéndose: el clic no hace nada.
            if (procesoNavegacion == null || inhabilitado)
                return;

            if (ModifierKeys.HasFlag(Keys.Shift))
            {
                Destruir();
                return;
            }

            encendido = !encendido;
            BackColor = encendido ? colorEnMovimiento : colorQuieto;
        }

        //---------------------------------------------------------------------
        // Detiene el dron, lo hace parpadear 3 veces y lo quita del área.
        //---------------------------------------------------------------------
        private void Destruir()
        {
            Thread procesoDestruccion;

            inhabilitado = true;
            encendido = false;
            BackColor = colorQuieto;

            procesoDestruccion = new Thread(() =>
            {
                for (int i = 0; i < 3; i++)
                {
                    Visible = false;
                    Thread.Sleep(200);
                    if (destruido)
                        return;

                    Visible = true;
                    Thread.Sleep(200);
                    if (destruido)
                        return;
                }

                // Windows solo deja destruir una ventana al hilo que la creó:
                // Remove y Dispose se piden al hilo de la interfaz.
                BeginInvoke(new Action(() =>
                {
                    areaDesplazamiento.Controls.Remove(this);
                    Dispose();
                }));
            });

            procesoDestruccion.IsBackground = true;
            procesoDestruccion.Start();
        }

        //---------------------------------------------------------------------
        // Al liberarse el dron (p. ej. al cerrar la mesa), avisa a sus hilos
        // para que terminen.
        //---------------------------------------------------------------------
        protected override void Dispose(bool disposing)
        {
            destruido = true;
            encendido = false;
            base.Dispose(disposing);
        }
    }
}
