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

        public CHombre() :base ( ) 
        {
            tasaProduccionEspermatozoides = 0;
        }
    }
}
