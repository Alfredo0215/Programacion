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
        public string Marca;
        public string Modelo;

        //---------------------------------------------------------------------
        // Métodos.
        //---------------------------------------------------------------------
        public CelularAndroid(string Marca, string Modelo) : base()
        {
            //---------------------------------------------------------------------
            // Recibe parámetros
            //---------------------------------------------------------------------

            this.Marca = Marca;
            this.Modelo = Modelo;


        }
    }
}
