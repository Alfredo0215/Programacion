using System.Drawing;

namespace PO26B_FSA.Acuario
{
    //---------------------------------------------------------------------
    // Planta: no se mueve. Es el alimento de los herbívoros.
    //---------------------------------------------------------------------
    internal class CPlanta : CEntidad
    {
        //---------------------------------------------------------------------
        // Constructor.
        //---------------------------------------------------------------------
        public CPlanta(float x, float y) : base(x, y, CParametros.LadoPlanta, CParametros.EnergiaPlanta)
        {
        }

        //---------------------------------------------------------------------
        // Métodos.
        //---------------------------------------------------------------------
        public override void Dibujar(Graphics g)
        {
            g.FillRectangle(Brushes.ForestGreen, GetLimites());
        }
    }
}
