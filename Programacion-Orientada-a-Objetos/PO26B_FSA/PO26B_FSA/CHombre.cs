using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO26B_FSA
{
    internal class CHombre : CPersona
    {

        //-------------------------------------------------------------------------
        // Atributos.
        //-------------------------------------------------------------------------

        private int tasaProduccionEspermatozoides;
        //-------------------------------------------------------------------------
        // Constructor.
        //-------------------------------------------------------------------------

        public CHombre(int altura, double peso) :base (altura, peso, 1) 
        {
            tasaProduccionEspermatozoides = 0;
        }
        //---------------------------------------------------------------------
        // Inicia la Producción de Espermatozoides
        //---------------------------------------------------------------------
        public int IniciaProduccionEspermatozoides()
        {
            Random numRandom = new Random();


            return tasaProduccionEspermatozoides = numRandom.Next(100, 170) * 1000000; ;
        }

        protected override string GetFactorReproductivo()
        {
            return $"mi cantidad de espermatozoides es: {IniciaProduccionEspermatozoides()}, por dia";
        }
    }
}
