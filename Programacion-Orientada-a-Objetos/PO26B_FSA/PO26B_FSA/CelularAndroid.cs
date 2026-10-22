using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PO26B_FSA
{
    public class CelularAndroid : Celular
    {

        //---------------------------------------------------------------------
        // Atributos.
        //---------------------------------------------------------------------

        //---------------------------------------------------------------------
        // Métodos.
        //---------------------------------------------------------------------
        public CelularAndroid(string modelo, string marca) : base("Android", modelo, marca)
        {
        }
    }
}
