using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;               // Color (fondo de las filas de DgvTabla3).
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;         // Form, MessageBox, DataGridView, etc.
using Bogus;                        // Librería NuGet que genera datos falsos (nombres, apellidos...).

namespace PO26B_FSA.Mesas_De_Practicas
{
    // Formulario de la mesa de prácticas 1. Tiene cuatro paneles (uno por
    // práctica) y botones que muestran uno y ocultan los demás.
    // "partial" porque la otra mitad de la clase (los controles) está en
    // MesaDePracticas1.Designer.cs, que genera el diseñador.
    public partial class MesaDePracticas1 : Form
    {
        //---------------------------------------------------------------------
        // Atributos.
        //---------------------------------------------------------------------

        // Un solo Random para todo el formulario. Si se creara uno nuevo en
        // cada llamada, varios creados muy seguidos podrían dar la misma
        // secuencia de números.
        Random numRandom = new Random();

        // Generador de datos falsos de Bogus, configurado en español ("es")
        // para que los nombres y apellidos suenen hispanos.
        Faker faker = new Faker("es");

        //---------------------------------------------------------------------
        // Método constructor de la clase.
        //---------------------------------------------------------------------

        public MesaDePracticas1()
        {
            // Crea y coloca todos los controles definidos en el diseñador.
            // Debe ir primero: antes de esta línea los controles son null.
            InitializeComponent();

            // Al abrir la mesa no se muestra ninguna práctica.
            PnlPracticas1.Visible = false;
            PnlPracticas2.Visible = false;
            PnlPracticas3.Visible = false;
            PnlPracticas4.Visible = false;

            // Quita la fila vacía que el DataGridView agrega al final para
            // que el usuario escriba. Así Rows.Count cuenta solo personas.
            DgvTabla3.AllowUserToAddRows = false;
        }

        //---------------------------------------------------------------------
        // Navegación entre paneles.
        // Cada botón alterna (muestra/oculta) su panel con "!" y oculta los
        // otros tres. Si se pulsa dos veces el mismo botón, su panel se cierra.
        //---------------------------------------------------------------------

        //---------------------------------------------------------------------
        // Botón que activa el panel de la práctica 1 y desactiva los demás.
        //---------------------------------------------------------------------
        private void BtnPractica1_Click(object sender, EventArgs e)
        {
            PnlPracticas1.Visible = !PnlPracticas1.Visible;
            PnlPracticas2.Visible = false;
            PnlPracticas3.Visible = false;
            PnlPracticas4.Visible = false;
        }

        //---------------------------------------------------------------------
        // Botón que activa el panel de la práctica 2 y desactiva los demás.
        //---------------------------------------------------------------------
        private void BtnPractica2_Click(object sender, EventArgs e)
        {
            PnlPracticas2.Visible = !PnlPracticas2.Visible;
            PnlPracticas1.Visible = false;
            PnlPracticas3.Visible = false;
            PnlPracticas4.Visible = false;
        }

        //---------------------------------------------------------------------
        // Botón que activa el panel de la práctica 3 y desactiva los demás.
        //---------------------------------------------------------------------
        private void BtnPractica3_Click(object sender, EventArgs e)
        {
            PnlPracticas3.Visible = !PnlPracticas3.Visible;
            PnlPracticas1.Visible = false;
            PnlPracticas2.Visible = false;
            PnlPracticas4.Visible = false;

        }

        //---------------------------------------------------------------------
        // Botón que activa el panel de la práctica 4 y desactiva los demás.
        //---------------------------------------------------------------------
        private void BtnPractica4_Click(object sender, EventArgs e)
        {
            PnlPracticas4.Visible = !PnlPracticas4.Visible;
            PnlPracticas1.Visible = false;
            PnlPracticas2.Visible = false;
            PnlPracticas3.Visible = false;
        }

        //---------------------------------------------------------------------
        // Práctica 1: celulares.
        //---------------------------------------------------------------------

        //---------------------------------------------------------------------
        // Botón 1 practica 1, crea el objeto Celular.
        //---------------------------------------------------------------------
        private void BtnP1Pnl1_Click(object sender, EventArgs e)
        {
            // Lee el texto de las dos cajas de captura del panel superior.
            string Captura1 = TbxCaptura1.Text;
            string Captura2 = TbxCaptura2.Text;

            // Crea un CelularAndroid con lo capturado y lo enciende.
            // El objeto es local: deja de existir al terminar el método.
            CelularAndroid Celular1 = new CelularAndroid(Captura1, Captura2);
            Celular1.Encender();
            MessageBox.Show("Celular encendido");

        }

        //---------------------------------------------------------------------
        // Práctica 2: herencia CAnimal -> CPersona -> CHombre / CMujer.
        //---------------------------------------------------------------------

        //---------------------------------------------------------------------
        // Botón 1 práctica 2, crea los objetos hombre y mujer y muestra sus atributos iniciales.
        //---------------------------------------------------------------------

        private void BtnP1Pnl2_Click(object sender, EventArgs e)
        {
            // Constructor: (altura, peso).
            CMujer mujer1 = new CMujer(25, 2.5);
            CHombre hombre1 = new CHombre(32, 3.2);

            // Completa los datos personales y los muestra con ToString().
            hombre1.RegistrarPersona("Alfredo", "Fletes", "Sánchez", new DateTime(2007, 02, 15));
            MessageBox.Show(hombre1.ToString());


            mujer1.RegistrarPersona("Valentina", "Velázquez", "López", new DateTime(2007, 12, 14));
            MessageBox.Show(mujer1.ToString());
        }

        //---------------------------------------------------------------------
        // Botón 2 práctica 2, crea un objeto de cada clase de la jerarquía y
        // compara qué devuelve Respirar() en cada una.
        //---------------------------------------------------------------------
        private void BtnP2Pnl2_Click(object sender, EventArgs e)
        {

            // Un objeto de cada clase de la jerarquía.
            CAnimal animal1 = new CAnimal(30, 30, 1);
            CPersona persona1 = new CPersona(30, 30, 1);
            CHombre hombre1 = new CHombre(30, 30);      // OJO: peso 30, en los otros botones el peso va de 0 a 4.
            CMujer mujer1 = new CMujer(30, 30);

            // Llama a Respirar() en cada uno para ver qué versión del método
            // se ejecuta según la clase del objeto.
            MessageBox.Show(animal1.Respirar().ToString());
            MessageBox.Show(persona1.Respirar().ToString());

            MessageBox.Show(hombre1.Respirar().ToString());

            MessageBox.Show(mujer1.Respirar().ToString());

        }

        //---------------------------------------------------------------------
        // Botón 3 práctica 2, crea un hombre y una mujer y agrega sus datos a DgvTabla2.
        //---------------------------------------------------------------------
        private void BtnP3Pnl2_Click(object sender, EventArgs e)
        {
            // Vacía la tabla para no duplicar filas si se pulsa varias veces.
            DgvTabla2.Rows.Clear();

            // GetDatosBasicos() devuelve los valores de una fila; Rows.Add
            // los reparte en las columnas en el mismo orden.
            CHombre hombre1 = new CHombre(32, 3.2);
            hombre1.RegistrarPersona("Alfredo", "Fletes", "Sánchez", new DateTime(2007, 02, 15));
            DgvTabla2.Rows.Add(hombre1.GetDatosBasicos());

            CMujer mujer1 = new CMujer(25, 2.5);
            mujer1.RegistrarPersona("Valentina", "Velázquez", "López", new DateTime(2007, 12, 14));
            DgvTabla2.Rows.Add(mujer1.GetDatosBasicos());
        }

        //---------------------------------------------------------------------
        // Práctica 3: población de personas aleatorias en DgvTabla3.
        //---------------------------------------------------------------------

        //---------------------------------------------------------------------
        // Botón 1 práctica 3, agrega UNA persona aleatoria a la tabla
        // (sin borrar las que ya había).
        //---------------------------------------------------------------------
        private void BtnP1Pnl3_Click(object sender, EventArgs e)
        {
            AgregarPersonaRandom();
            ActualizarLabelPersonas();
        }

        //---------------------------------------------------------------------
        // Botón 2 práctica 3, borra la tabla y crea tantas personas
        // aleatorias como indique TbxCaptura1 (caja del panel superior).
        //---------------------------------------------------------------------
        private void BtnP2Pnl3_Click(object sender, EventArgs e)
        {
            //Obtiene el número de personas
            // TryParse no lanza excepción: devuelve false si el texto no es
            // un entero. Si falla, avisa y sale del método con return.
            // OJO: un número negativo sí pasa; el for simplemente no se ejecuta.
            if (!int.TryParse(TbxCaptura1.Text, out int personasIniciales))
            {
                MessageBox.Show("Introduce un número de personas válido");
                return;
            }

            // Empieza desde cero en cada pulsación.
            DgvTabla3.Rows.Clear();

            //Crea un numero de personas aleatorias.
            for (int i = 0; i < personasIniciales; i++)
            {
                AgregarPersonaRandom();

            }

            // Se actualiza una sola vez, al final, no en cada vuelta.
            ActualizarLabelPersonas();
        }

        //---------------------------------------------------------------------
        // Crea un CHombre o una CMujer con datos aleatorios, lo agrega como
        // fila de DgvTabla3 y pinta la fila según el sexo.
        //---------------------------------------------------------------------
        private void AgregarPersonaRandom()
        {
            // Next(min, max): min entra, max NO entra.
            int sexo = numRandom.Next(0, 2);                // 0 o 1.
            int altura = numRandom.Next(15, 31);            // 15 a 30.
            double peso = numRandom.NextDouble() * 4;       // NextDouble da [0, 1), así que peso queda en [0, 4).

            // Año 1980-2020, mes 1-12, día 1-28. El día llega solo a 28 para
            // que la fecha siempre exista en cualquier mes (febrero puede
            // tener solo 28). Con un día inexistente, como 30 de febrero,
            // new DateTime lanzaría una excepción.
            DateTime fechaNacimiento = new DateTime(numRandom.Next(1980, 2021),
                                                    numRandom.Next(1, 13),
                                                    numRandom.Next(1, 29));

            // Variable del tipo base: puede guardar un CHombre o una CMujer
            // (polimorfismo). Así RegistrarPersona y GetDatosBasicos se
            // escriben una sola vez después del if.
            CPersona persona;
            Color color;

            if (sexo == 1)
            {
                persona = new CHombre(altura, peso);
                // Se escribe Bogus.DataSets.Name completo porque, dentro de un
                // Form, "Name" a secas es la propiedad Name del formulario.
                persona.RegistrarPersona(faker.Name.FirstName(Bogus.DataSets.Name.Gender.Male), faker.Name.LastName(), faker.Name.LastName(), fechaNacimiento);
                color = Color.LightBlue;

            }

            else
            {
                persona = new CMujer(altura, peso);
                persona.RegistrarPersona(faker.Name.FirstName(Bogus.DataSets.Name.Gender.Female), faker.Name.LastName(), faker.Name.LastName(), fechaNacimiento);
                color = Color.LightPink;
            }

            // Rows.Add devuelve el índice de la fila nueva; con él se cambia
            // el color de fondo solo de esa fila.
            int fila = DgvTabla3.Rows.Add(persona.GetDatosBasicos());
            DgvTabla3.Rows[fila].DefaultCellStyle.BackColor = color;
        }

        //---------------------------------------------------------------------
        // Muestra en LblNumPersonas cuántas filas tiene DgvTabla3.
        // Es correcto porque AllowUserToAddRows = false (ver constructor);
        // si no, contaría también la fila vacía del final.
        //---------------------------------------------------------------------
        private void ActualizarLabelPersonas()
        {
            LblNumPersonas.Text = $"Personas: {DgvTabla3.Rows.Count}";
        }
    }
}
