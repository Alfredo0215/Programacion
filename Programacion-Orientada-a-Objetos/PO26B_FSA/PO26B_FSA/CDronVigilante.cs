using System.Drawing;
using System.Windows.Forms;

namespace PO26B_FSA
{
    //-------------------------------------------------------------------------
    // Dron que no rebota en diagonal: baja hasta el borde y recorre el
    // perímetro del área (este → norte → oeste → sur).
    //-------------------------------------------------------------------------
    internal class CDronVigilante : CDron
    {
        //---------------------------------------------------------------------
        // Métodos.
        //---------------------------------------------------------------------

        public CDronVigilante(Control areaDesplazamiento) : base(areaDesplazamiento)
        {
            // En movimiento: texto #2B2A27 (el dron normal usa terracota).
            colorEnMovimiento = Color.FromArgb(43, 42, 39);

            // Empieza bajando en línea recta.
            norte = false;
            sur = true;
            este = false;
            oeste = false;
        }

        //---------------------------------------------------------------------
        // Al tocar un borde gira para seguir el perímetro. El orden de los
        // "else if" resuelve las esquinas: en cada una gana el giro que
        // continúa la vuelta.
        //---------------------------------------------------------------------
        protected override void CambiarDireccion(int x, int y)
        {
            int limiteX = areaDesplazamiento.Width - TAMAÑO;
            int limiteY = areaDesplazamiento.Height - TAMAÑO;

            if (x <= 0 && y <= 0)
            {
                // Esquina superior izquierda: baja.
                norte = false;
                sur = true;
                este = false;
                oeste = false;
            }
            else if (y <= 0)
            {
                // Borde superior: va al oeste.
                norte = false;
                sur = false;
                este = false;
                oeste = true;
            }
            else if (x >= limiteX)
            {
                // Borde derecho: sube.
                norte = true;
                sur = false;
                este = false;
                oeste = false;
            }
            else if (y >= limiteY)
            {
                // Borde inferior: va al este.
                norte = false;
                sur = false;
                este = true;
                oeste = false;
            }
        }
    }
}
