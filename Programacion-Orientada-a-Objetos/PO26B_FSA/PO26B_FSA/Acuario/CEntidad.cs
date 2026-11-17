using System;
using System.Drawing;

namespace PO26B_FSA.Acuario
{
    //---------------------------------------------------------------------
    // Base de todo lo que vive en el acuario. Solo guarda lo que comparten
    // todas las entidades, incluidas las plantas.
    //---------------------------------------------------------------------
    internal abstract class CEntidad
    {
        //---------------------------------------------------------------------
        // Atributos.
        //---------------------------------------------------------------------
        protected float x;
        protected float y;
        protected readonly float lado;
        // Privada: solo cambia por Comer, SerComido, GanarEnergia y
        // GastarEnergia. Nadie de fuera puede asignarla.
        private int energia;
        private bool viva = true;

        //---------------------------------------------------------------------
        // Constructor.
        //---------------------------------------------------------------------
        protected CEntidad(float x, float y, float lado, int energiaInicial)
        {
            this.x = x;
            this.y = y;
            this.lado = lado;
            energia = energiaInicial;
        }

        //---------------------------------------------------------------------
        // Solo lectura.
        //---------------------------------------------------------------------
        public int GetEnergia()
        {
            return energia;
        }

        public bool EstaViva()
        {
            return viva;
        }

        //---------------------------------------------------------------------
        // Cada tipo decide qué puede comer. Por defecto, nada (la planta).
        //---------------------------------------------------------------------
        public virtual bool PuedeComer(CEntidad otra)
        {
            return false;
        }

        //---------------------------------------------------------------------
        // Única puerta pública para transferir energía; las reglas van
        // dentro. Devuelve true si de verdad comió.
        //---------------------------------------------------------------------
        public bool Comer(CEntidad presa)
        {
            if (!viva || !presa.viva || !PuedeComer(presa))
            {
                return false;
            }

            GanarEnergia(presa.SerComido());
            return true;
        }

        //---------------------------------------------------------------------
        // Private funciona aunque se llame sobre la presa: en C# el acceso
        // es por clase, no por objeto. Así nadie de fuera puede matar a una
        // entidad ni quitarle energía.
        //---------------------------------------------------------------------
        private int SerComido()
        {
            int nutre = energia;
            energia = 0;
            viva = false;
            return nutre;
        }

        protected void GanarEnergia(int cantidad)
        {
            energia += cantidad;
        }

        //---------------------------------------------------------------------
        // El único lugar donde se resta energía: aquí se garantiza que nunca
        // quede negativa.
        //---------------------------------------------------------------------
        protected void GastarEnergia(int cantidad)
        {
            energia -= cantidad;
            if (energia <= 0)
            {
                energia = 0;
                Morir();
            }
        }

        protected void Morir()
        {
            viva = false;
        }

        //---------------------------------------------------------------------
        // Rectángulo que ocupa la entidad. Sirve para dibujar y, más
        // adelante, para detectar choques.
        //---------------------------------------------------------------------
        public RectangleF GetLimites()
        {
            return new RectangleF(x, y, lado, lado);
        }

        //---------------------------------------------------------------------
        // Un paso de vida. Por defecto no hace nada (la planta lo hereda así).
        // Recibe los límites del área y el Random del ecosistema para no
        // depender de CEcosistema.
        //---------------------------------------------------------------------
        public virtual void Actualizar(float ancho, float alto, Random azar)
        {
        }

        //---------------------------------------------------------------------
        // Cada tipo decide cómo se ve.
        //---------------------------------------------------------------------
        public abstract void Dibujar(Graphics g);
    }
}
