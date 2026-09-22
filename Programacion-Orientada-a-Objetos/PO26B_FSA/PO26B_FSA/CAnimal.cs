using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO26B_FSA
{
    internal class CAnimal : CSerVivo
    {


        //-------------------------------------------------------------------------
        // Atributos.
        //-------------------------------------------------------------------------

        protected int numExtremidades;
        protected string tipoSangre;
        protected int nivelInstinto;

        //-------------------------------------------------------------------------
        // Atributos.
        //-------------------------------------------------------------------------

        public CAnimal() : base()
        {
            numExtremidades = 0;
            tipoSangre = "";
            nivelInstinto = 0;
        }
    }
}
