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

        protected bool inhabilitado;
        protected bool encendido;

        protected bool regresarACasa;

        //Atributos de ejecución

        protected Thread procesoNavegacion;

        //Atributos de configuración

        protected const int TAMAÑO = 50;
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
            BackColor = Color.FromArgb(122, 119, 110);
            BringToFront();
           
            Click += new EventHandler(Accion);

            CheckForIllegalCrossThreadCalls = false;
        }

        public void Encender(int tiempoArranque)
        {
            Thread procesoEncendido;

            procesoEncendido = new Thread(() => {
                Thread.Sleep(tiempoArranque);

                Visible = false;
                Thread.Sleep(100);
                Visible = true;
                Thread.Sleep(100);
                Visible = true;


                // Estado encendido: acento terracota #C96442.
                BackColor = Color.FromArgb(201, 100, 66);
                encendido = true;
                Desplazar(numRandom.Next(1, 50
                    ));
            });

            procesoEncendido.Start();
        }

        public void Desplazar(int velocidad)
        {

            int x;
            int y;

            procesoNavegacion = new Thread(() =>
            {

                while(encendido)
                {
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

                    if(x <= 0)
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

                    Location = new Point(x, y);
                    Thread.Sleep(velocidad);
                }
            });

            procesoNavegacion.Start();
        }

        private void Accion(object sender, EventArgs e)
        {
            // Estado tras el clic: texto #2B2A27.
            BackColor = Color.FromArgb(43, 42, 39);
        }
    }
}
