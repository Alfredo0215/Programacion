using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO26B_FSA
{

    //-------------------------------------------------------------------------
    // Atributos.
    //-------------------------------------------------------------------------

    
    internal class CSerVivo
    {

       

        //-------------------------------------------------------------------------
        // Atributos.
        //-------------------------------------------------------------------------

        protected DateTime fechaNacimiento;
        protected string nombre;
        protected int nivelRazonamiento;

        protected int sexo;
        public CSerVivo()
        {
            fechaNacimiento = new DateTime();
            nombre = "";
        }
    }
}
