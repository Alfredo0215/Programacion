using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO26B_FSA
{
    internal abstract class CSerVivo
    {

        //-------------------------------------------------------------------------
        // Atributos.
        //-------------------------------------------------------------------------

        protected DateTime fechaNacimiento;
        protected string nombre;
        protected int nivelRazonamiento;

        //-------------------------------------------------------------------------
        // Constructor.
        //-------------------------------------------------------------------------

        public CSerVivo()
        {
            fechaNacimiento = new DateTime();
            nombre = "";
            nivelRazonamiento = 0;
        }

        //-------------------------------------------------------------------------
        // Devuelve una descripción de cómo respira el ser vivo. Cada subclase
        // decide el texto; el formulario es quien lo muestra.
        //-------------------------------------------------------------------------
        public abstract string Respirar();

    }
}
