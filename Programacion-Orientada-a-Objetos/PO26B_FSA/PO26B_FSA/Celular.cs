using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PO26B_FSA
{
    public class Celular
    {


        //---------------------------------------------------------------------
        // Atributos. 
        //---------------------------------------------------------------------

        private bool encendido;
        private string sistemaOperativo;
        private string marca;
        private string modelo;

        //---------------------------------------------------------------------
        // Constructor. 
        //---------------------------------------------------------------------

        public Celular(string sistemaOperativo, string modelo, string marca) 
        {
            this.sistemaOperativo = sistemaOperativo;
            this.modelo = modelo;
            this.marca = marca;

            this.encendido = false;
        }

        //---------------------------------------------------------------------
        // Métodos.
        //---------------------------------------------------------------------

        public void Encender()
        {
            encendido = true;
        }

        public void Apagar()
        {
            encendido = false;
        }

        public bool GetEncendido()
        {
            return encendido;
        }

        public string GetSistemaOperativo()
        {
            return sistemaOperativo;
        }

        public string GetMarca()
        {
            return marca;
        }

        public string GetModelo()
        {
            return modelo;
        }
    }
}
