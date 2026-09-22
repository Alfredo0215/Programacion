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

        public CPersona() 
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
            return $"Hola, soy {nombre} {apellidoP} {apellidoM} y nací el {fechaNacimiento}.";
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
