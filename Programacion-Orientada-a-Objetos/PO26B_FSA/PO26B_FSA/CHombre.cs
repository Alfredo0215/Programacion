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
        Random numRandom = new Random();


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

        protected override string GetFactorReproductivo()
        {
            return $"mi cantidad de espermatozoides es: {tasaProduccionEspermatozoides}, por dia";
        }

        //---------------------------------------------------------------------
        // Implementa el método abstracto respirar para la clase.
        //---------------------------------------------------------------------
       
        /*
        protected override bool Respirar()
        {
            MessageBox.Show("Hombre respirando");
            return true;
        }
        */
    }
}
