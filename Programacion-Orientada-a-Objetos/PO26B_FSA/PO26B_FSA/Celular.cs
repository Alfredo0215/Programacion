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
        public string Marca;
        public string Modelo;
        public bool Encendido;

        //---------------------------------------------------------------------
        // Métodos.
        //---------------------------------------------------------------------
        public Celular(string Marca, string Modelo)
        {
            this.Marca = Marca;
            this.Modelo = Modelo;

        }

        public void Encender()
        {
            Encendido = true;
            MessageBox.Show("Celular encendido");
        }

       
    }
}
