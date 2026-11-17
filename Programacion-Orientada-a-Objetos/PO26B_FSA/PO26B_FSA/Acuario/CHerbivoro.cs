using System.Drawing;

namespace PO26B_FSA.Acuario
{
    //---------------------------------------------------------------------
    // Pez que come plantas.
    //---------------------------------------------------------------------
    internal class CHerbivoro : CPez
    {
        //---------------------------------------------------------------------
        // Constructor. El ángulo va en radianes.
        //---------------------------------------------------------------------
        public CHerbivoro(float x, float y, double angulo)
            : base(x, y, CParametros.LadoHerbivoro, CParametros.VelocidadHerbivoro, angulo,
                   CParametros.EnergiaInicialHerbivoro, CParametros.EdadMaximaHerbivoro)
        {
        }

        //---------------------------------------------------------------------
        // Métodos.
        //---------------------------------------------------------------------
        public override bool PuedeComer(CEntidad otra)
        {
            return otra is CPlanta;
        }

        public override void Dibujar(Graphics g)
        {
            g.FillEllipse(Brushes.Gold, GetLimites());
        }
    }
}
