using System;
using System.Collections.Generic;
using System.Drawing;

namespace PO26B_FSA.Acuario
{
    //---------------------------------------------------------------------
    // Contiene a todas las entidades y conoce los límites del área.
    // Las trata a todas como CEntidad, sin preguntar su tipo concreto.
    //---------------------------------------------------------------------
    internal class CEcosistema
    {
        //---------------------------------------------------------------------
        // Atributos.
        //---------------------------------------------------------------------
        // Un solo Random para toda la app: varios "new Random()" creados casi
        // al mismo tiempo reciben la misma semilla y repiten los números.
        private static readonly Random azar = new Random();
        private readonly List<CEntidad> entidades = new List<CEntidad>();
        private readonly float ancho;
        private readonly float alto;

        //---------------------------------------------------------------------
        // Constructor: crea la población inicial en posiciones aleatorias.
        //---------------------------------------------------------------------
        public CEcosistema(float ancho, float alto)
        {
            this.ancho = ancho;
            this.alto = alto;

            for (int i = 0; i < CParametros.PlantasIniciales; i++)
            {
                PointF p = PosicionAleatoria(CParametros.LadoPlanta);
                entidades.Add(new CPlanta(p.X, p.Y));
            }

            for (int i = 0; i < CParametros.HerbivorosIniciales; i++)
            {
                PointF p = PosicionAleatoria(CParametros.LadoHerbivoro);
                entidades.Add(new CHerbivoro(p.X, p.Y, AnguloAleatorio()));
            }

            for (int i = 0; i < CParametros.DepredadoresIniciales; i++)
            {
                PointF p = PosicionAleatoria(CParametros.LadoDepredador);
                entidades.Add(new CDepredador(p.X, p.Y, AnguloAleatorio()));
            }
        }

        //---------------------------------------------------------------------
        // Ángulo al azar entre 0 y 2π radianes (una vuelta completa).
        //---------------------------------------------------------------------
        private double AnguloAleatorio()
        {
            return azar.NextDouble() * 2 * Math.PI;
        }

        //---------------------------------------------------------------------
        // Un paso de simulación en tres etapas. Las dos primeras solo marcan
        // muertos; la lista cambia únicamente en la tercera, cuando ya nadie
        // la recorre (evita InvalidOperationException).
        //---------------------------------------------------------------------
        public void Paso()
        {
            // 1. Moverse, envejecer y gastar energía.
            foreach (CEntidad entidad in entidades)
            {
                entidad.Actualizar(ancho, alto, azar);
            }

            // 2. Choques: el ecosistema detecta; cada entidad decide si come
            //    (Comer revisa PuedeComer y que ambas sigan vivas).
            foreach (CEntidad a in entidades)
            {
                foreach (CEntidad b in entidades)
                {
                    if (a != b && a.GetLimites().IntersectsWith(b.GetLimites()))
                    {
                        a.Comer(b);
                    }
                }
            }

            // 3. Retirar a los muertos.
            entidades.RemoveAll(e => !e.EstaViva());
        }

        //---------------------------------------------------------------------
        // Esquina superior izquierda al azar, de 0 a (ancho - lado), para
        // que la entidad completa quepa dentro del área.
        //---------------------------------------------------------------------
        private PointF PosicionAleatoria(float lado)
        {
            float x = (float)(azar.NextDouble() * (ancho - lado));
            float y = (float)(azar.NextDouble() * (alto - lado));
            return new PointF(x, y);
        }

        //---------------------------------------------------------------------
        // Solo lectura: desde fuera no se puede agregar ni quitar entidades.
        //---------------------------------------------------------------------
        public IReadOnlyList<CEntidad> GetEntidades()
        {
            return entidades.AsReadOnly();
        }

        //---------------------------------------------------------------------
        // Cada entidad se dibuja a sí misma (polimorfismo).
        //---------------------------------------------------------------------
        public void Dibujar(Graphics g)
        {
            foreach (CEntidad entidad in entidades)
            {
                entidad.Dibujar(g);
            }
        }
    }
}
