using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PO26B_FSA
{
    internal class CMujer : CPersona
    {


        //-------------------------------------------------------------------------
        // Atributos.
        //-------------------------------------------------------------------------

        private int cantidadOvulos;
        // static: una sola instancia compartida, para que dos objetos creados
        // en el mismo instante no reciban la misma semilla.
        private static Random numRandom = new Random();


        public CMujer(int altura, double peso) : base(altura, peso, 0)
        {
            IniciaProduccionOvulos();
        }

        //---------------------------------------------------------------------
        //  Inicia la Producción de Óvulos
        //---------------------------------------------------------------------

        public int IniciaProduccionOvulos()
        {
            return cantidadOvulos = numRandom.Next(1000000, 2000000);
            
        }

        //---------------------------------------------------------------------
        // Factor reproductivo en millones de óvulos (reserva total).
        // Se divide entre 1000000.0 para no perder los decimales.
        //---------------------------------------------------------------------
        protected override string GetFactorReproductivo()
        {
            return $"{cantidadOvulos / 1000000.0:0.##} millones de óvulos";
        }

    }
}
