using System.Drawing;

namespace PO26B_FSA.Acuario
{
    //---------------------------------------------------------------------
    // Pez que come herbívoros.
    //---------------------------------------------------------------------
    internal class CDepredador : CPez
    {
        //---------------------------------------------------------------------
        // Constructor. El ángulo va en radianes. La energía es opcional: los
        // peces iniciales usan la de CParametros; las crías, la del padre.
        //---------------------------------------------------------------------
        public CDepredador(float x, float y, double angulo, int energia = CParametros.EnergiaInicialDepredador)
            : base(x, y, CParametros.LadoDepredador, CParametros.VelocidadDepredador, angulo,
                   energia, CParametros.EdadMaximaDepredador)
        {
        }

        //---------------------------------------------------------------------
        // Métodos.
        //---------------------------------------------------------------------
        public override bool PuedeComer(CEntidad otra)
        {
            return otra is CHerbivoro;
        }

        //---------------------------------------------------------------------
        // La cría nace donde está el padre, que siempre está dentro del área.
        //---------------------------------------------------------------------
        protected override CPez CrearCria(double angulo, int energia)
        {
            return new CDepredador(x, y, angulo, energia);
        }

        public override void Dibujar(Graphics g)
        {
            g.FillEllipse(Brushes.IndianRed, GetLimites());
        }
    }
}
