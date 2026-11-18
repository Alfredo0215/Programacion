using System.Drawing;

namespace PO26B_FSA.Acuario
{
    //---------------------------------------------------------------------
    // Pez que come plantas.
    //---------------------------------------------------------------------
    internal class CHerbivoro : CPez
    {
        //---------------------------------------------------------------------
        // Constructor. El ángulo va en radianes. La energía es opcional: los
        // peces iniciales usan la de CParametros; las crías, la del padre.
        //---------------------------------------------------------------------
        public CHerbivoro(float x, float y, double angulo, int energia = CParametros.EnergiaInicialHerbivoro)
            : base(x, y, CParametros.LadoHerbivoro, CParametros.VelocidadHerbivoro, angulo,
                   energia, CParametros.EdadMaximaHerbivoro)
        {
        }

        //---------------------------------------------------------------------
        // Métodos.
        //---------------------------------------------------------------------
        public override bool PuedeComer(CEntidad otra)
        {
            return otra is CPlanta;
        }

        //---------------------------------------------------------------------
        // La cría nace donde está el padre, que siempre está dentro del área.
        //---------------------------------------------------------------------
        protected override CPez CrearCria(double angulo, int energia)
        {
            return new CHerbivoro(x, y, angulo, energia);
        }

        public override void Dibujar(Graphics g)
        {
            g.FillEllipse(Brushes.Gold, GetLimites());
        }
    }
}
