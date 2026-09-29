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
        protected int altura;
        protected double peso;
        protected int sexo;


        //-------------------------------------------------------------------------
        // Atributos.
        //-------------------------------------------------------------------------

        public CAnimal(int altura, double peso, int sexo) : base()
        {
            this.altura = altura;
            this.peso = peso;
            this.sexo = sexo;
            numExtremidades = 0;
            tipoSangre = "";
            nivelInstinto = 0;
        }
    }
}
