using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO26B_FSA
{
    internal class CPersona : CAnimal
    {
        //-------------------------------------------------------------------------
        // Atributos.
        //-------------------------------------------------------------------------

        protected string apellidoP;
        protected string apellidoM;
        protected int preferencisSexual;
        protected int genero;

        protected int religion;

        public CPersona(int altura, double peso, int sexo) : base(altura, peso, sexo)
        {
            apellidoM = "";
            apellidoM = "";
            nivelRazonamiento = 0;
            preferencisSexual = 0;
        }

        //---------------------------------------------------------------------
        // Sobreescribe la información tostring del objeto.
        //---------------------------------------------------------------------
        public override string ToString()
        {
            string factorReproductivo;
            if (sexo == 0)
            {
                factorReproductivo = "Tengo una cantidad de óvulos";
            }
            else
            {
                factorReproductivo = "Tengo una cantidad de espermatozoides";
            }
            return $"Hola, soy {nombre} {apellidoP} {apellidoM} y nací el {fechaNacimiento}. Mi altura fue de {altura} y mi peso fue de {peso}, además {GetFactorReproductivo()}";
        }

        protected virtual string GetFactorReproductivo()
        {
            return "{factor reproductivo}";
        }

        //---------------------------------------------------------------------
        // Registra a una persona.
        //---------------------------------------------------------------------
        public void RegistrarPersona(string nombre, string apellidoP, string apellidoM, DateTime fechaNacimiento)
        {
            this.nombre = nombre ;
            this.apellidoP = apellidoP ;
            this.apellidoM = apellidoM; 
            this.fechaNacimiento = fechaNacimiento ;
        }
    }
}
