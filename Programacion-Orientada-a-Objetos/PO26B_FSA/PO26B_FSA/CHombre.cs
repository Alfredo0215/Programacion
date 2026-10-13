using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PO26B_FSA
{
    internal class CHombre : CPersona
    {

        //-------------------------------------------------------------------------
        // Atributos.
        //-------------------------------------------------------------------------
        // static: una sola instancia compartida, para que dos objetos creados
        // en el mismo instante no reciban la misma semilla.
        private static Random numRandom = new Random();


        private int tasaProduccionEspermatozoides;
        //-------------------------------------------------------------------------
        // Constructor.
        //-------------------------------------------------------------------------

        public CHombre(int altura, double peso) :base (altura, peso, 1) 
        {
            IniciaProduccionEspermatozoides();
        }
        //---------------------------------------------------------------------
        // Inicia la Producción de Espermatozoides
        //---------------------------------------------------------------------
        public int IniciaProduccionEspermatozoides()
        {


            return tasaProduccionEspermatozoides = numRandom.Next(100, 170) * 1000000;
        }

        //---------------------------------------------------------------------
        // Factor reproductivo en millones de espermatozoides por día.
        //---------------------------------------------------------------------
        protected override string GetFactorReproductivo()
        {
            return $"{tasaProduccionEspermatozoides / 1000000} millones de espermatozoides/día";
        }
    }
}
