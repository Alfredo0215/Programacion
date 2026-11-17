using System;
using System.Drawing;

namespace PO26B_FSA.Acuario
{
    //---------------------------------------------------------------------
    // Base de los peces: dirección, velocidad y edad.
    // Las plantas no heredan nada de esto.
    //---------------------------------------------------------------------
    internal abstract class CPez : CEntidad
    {
        //---------------------------------------------------------------------
        // Atributos.
        //---------------------------------------------------------------------
        // Dirección como vector unitario (largo 1): solo dice hacia dónde va.
        protected float dx;
        protected float dy;
        // Rapidez en píxeles por tick.
        protected readonly float velocidad;
        // Edad en ticks.
        private int edad;
        private readonly int edadMaxima;

        //---------------------------------------------------------------------
        // Constructor. El ángulo va en radianes (0 = hacia la derecha).
        //---------------------------------------------------------------------
        protected CPez(float x, float y, float lado, float velocidad, double angulo,
                       int energiaInicial, int edadMaxima)
            : base(x, y, lado, energiaInicial)
        {
            this.velocidad = velocidad;
            this.edadMaxima = edadMaxima;
            CambiarDireccion(angulo);
        }

        //---------------------------------------------------------------------
        // (cos, sin) siempre mide 1, así que la rapidez real es "velocidad"
        // en cualquier dirección.
        //---------------------------------------------------------------------
        private void CambiarDireccion(double angulo)
        {
            dx = (float)Math.Cos(angulo);
            dy = (float)Math.Sin(angulo);
        }

        //---------------------------------------------------------------------
        // Primero envejecer y gastar energía; si muere, ya no se mueve.
        // Después: girar -> avanzar -> corregir. La corrección va al final para
        // que, pase lo que pase en el giro, el pez termine dentro del área.
        // Math.Abs fija el signo hacia adentro; "-dx" podría dejarlo
        // apuntando afuera y el pez temblaría pegado al borde.
        //---------------------------------------------------------------------
        public override void Actualizar(float ancho, float alto, Random azar)
        {
            if (!EstaViva())
            {
                return;
            }

            edad++;
            if (edad >= edadMaxima)
            {
                Morir();
                return;
            }

            GastarEnergia(CParametros.GastoPorTick);
            if (!EstaViva())
            {
                return;
            }

            if (azar.NextDouble() < CParametros.ProbabilidadGiro)
            {
                CambiarDireccion(azar.NextDouble() * 2 * Math.PI);
            }

            x += dx * velocidad;
            y += dy * velocidad;

            if (x < 0)
            {
                x = 0;
                dx = Math.Abs(dx);
            }
            else if (x + lado > ancho)
            {
                x = ancho - lado;
                dx = -Math.Abs(dx);
            }

            if (y < 0)
            {
                y = 0;
                dy = Math.Abs(dy);
            }
            else if (y + lado > alto)
            {
                y = alto - lado;
                dy = -Math.Abs(dy);
            }
        }

        //---------------------------------------------------------------------
        // Solo lectura: X = dx, Y = dy.
        //---------------------------------------------------------------------
        public PointF GetDireccion()
        {
            return new PointF(dx, dy);
        }

        public int GetEdad()
        {
            return edad;
        }
    }
}
