using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PO26B_FSA
{
    internal class CPersona : CAnimal
    {
        //-------------------------------------------------------------------------
        // Atributos.
        //-------------------------------------------------------------------------

        protected string apellidoP;
        protected string apellidoM;
        protected int preferenciaSexual;

        //-------------------------------------------------------------------------
        // Constructor.
        //-------------------------------------------------------------------------

        public CPersona(int altura, double peso, int sexo) : base(altura, peso, sexo)
        {
            apellidoP = "";
            apellidoM = "";
            preferenciaSexual = 0;
        }

        //---------------------------------------------------------------------
        // Sobrescribe Respirar de CAnimal: una persona puede controlar su
        // respiración. CHombre y CMujer heredan esta versión.
        //---------------------------------------------------------------------
        public override string Respirar()
        {
            return "Persona respirando de forma consciente";
        }

        //---------------------------------------------------------------------
        // Sobreescribe la información tostring del objeto.
        //---------------------------------------------------------------------
        public override string ToString()
        {
            return $"Hola, soy {nombre} {apellidoP} {apellidoM} y nací el {fechaNacimiento:d}. Mi altura fue de {altura} y mi peso fue de {peso}, y mi factor reproductivo es: {GetFactorReproductivo()}";
        }

        //---------------------------------------------------------------------
        // Devuelve el factor reproductivo con su unidad. Cada subclase lo
        // sobrescribe; lo usan ToString y GetDatosBasicos.
        //---------------------------------------------------------------------
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

        //---------------------------------------------------------------------
        // Devuelve los datos para una fila de tabla. El orden debe coincidir
        // con el de las columnas de DgvTabla2.
        //---------------------------------------------------------------------
        public object[] GetDatosBasicos()
        {
            return new object[]
            {
                nombre, apellidoP, apellidoM, fechaNacimiento, GetFactorReproductivo()
            };
        }
    }
}
