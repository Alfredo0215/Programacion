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

        private int tasaProduccionOvulos;

        public CMujer(int altura, double peso) : base(altura, peso, 0)
        {
            tasaProduccionOvulos = IniciaProduccionOvulos();
        }

        //---------------------------------------------------------------------
        //  Inicia la Producción de Óvulos
        //---------------------------------------------------------------------

        public int IniciaProduccionOvulos()
        {
            Random numRandom = new Random();
            return tasaProduccionOvulos = numRandom.Next(1000000, 2000000);
            
        }

        protected override string GetFactorReproductivo()
        {
            return $"mi cantidad de óvulos es: {IniciaProduccionOvulos()}";
        }

        //---------------------------------------------------------------------
        // Implementa el método abstracto respirar para la clase.
        //---------------------------------------------------------------------

        /*
        protected override bool Respirar()
        {
            MessageBox.Show("Mujer respirando");
            return true;
        }
        */
    }
}
