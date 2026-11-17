using System.Drawing;

namespace PO26B_FSA.Acuario
{
    //---------------------------------------------------------------------
    // Pez que come herbívoros.
    //---------------------------------------------------------------------
    internal class CDepredador : CPez
    {
        //---------------------------------------------------------------------
        // Constructor. El ángulo va en radianes.
        //---------------------------------------------------------------------
        public CDepredador(float x, float y, double angulo)
            : base(x, y, CParametros.LadoDepredador, CParametros.VelocidadDepredador, angulo,
                   CParametros.EnergiaInicialDepredador, CParametros.EdadMaximaDepredador)
        {
        }

        //---------------------------------------------------------------------
        // Métodos.
        //---------------------------------------------------------------------
        public override bool PuedeComer(CEntidad otra)
        {
            return otra is CHerbivoro;
        }

        public override void Dibujar(Graphics g)
        {
            g.FillEllipse(Brushes.IndianRed, GetLimites());
        }
    }
}
