using System;
using System.Collections.Generic;

namespace PO26B_FSA.Fluidos
{
    //-------------------------------------------------------------------------
    // Solucionador "Stable Fluids" de Stam con valores en el centro de las
    // celdas. La cuadrícula tiene Nx×Ny celdas interiores (de 1 a Nx y de 1 a
    // Ny) y un anillo de celdas de borde (0 y Nx+1, 0 y Ny+1) que guardan la
    // regla de paredes. La fila j crece hacia abajo, como en la pantalla.
    // No usa nada de WinForms: se prueba sin pantalla.
    //-------------------------------------------------------------------------
    internal class CFluido
    {
        //---------------------------------------------------------------------
        // Atributos.
        //---------------------------------------------------------------------

        // Fuerza que el chorro suma a la velocidad en cada tick (celdas/tick
        // por tick, multiplicada por dt).
        private const double FuerzaChorro = 0.1;

        // Fase 3 [Validar en Fase 3]: velocidad de la entrada (U·dt, en
        // celdas/tick) y radio del círculo en celdas.
        private const double CourantEntrada = 0.7;
        private const int RadioObstaculo = 7;

        // Fase 5: intensidad ε del vorticity confinement (con h = 1 celda).
        // Barrido de Diagnostico_Confinamiento (2026-11-26): 0.01 es el
        // menor que cumple (pulso > 2000 ticks, Courant con chorro 0.73).
        public const double EpsilonConfinamiento = 0.01;

        // Evita dividir entre 0 al normalizar ∇|ω| donde no hay giro.
        private const double MinimoGradiente = 1e-6;

        private readonly int nx;
        private readonly int ny;
        private readonly int ancho;          // nx + 2: celdas por fila con el borde.
        private readonly int iteraciones;    // k: pasadas de Gauss-Seidel.
        private readonly double omega;       // 1 = Gauss-Seidel; > 1 = SOR.
        private readonly double dt;
        private readonly double disipacion;  // Fracción de tinta que se pierde por tick.

        // Arreglos reservados una sola vez. Los "0" guardan la copia previa
        // que lee la advección.
        private readonly double[] u, v, u0, v0;
        private readonly double[] tinta, tinta0;
        private readonly double[] p, div, vort;

        private long ticks;

        // Interruptor de la Fase 2. Apagado, Paso() no proyecta: p se queda
        // como estaba y las paredes se siguen aplicando (las aplica Advectar).
        private bool proyeccionActiva = true;

        // Interruptor de la Fase 5 y su ε (los tests prueban otros valores).
        private bool confinamientoActivo;
        private double epsilonConfinamiento = EpsilonConfinamiento;

        // Fase 3. Máscara de celdas sólidas y la lista de sus índices (para
        // recorrer solo esas ~150 celdas). Con bordes abiertos, la izquierda
        // es entrada y la derecha salida; arriba y abajo siguen las paredes.
        private readonly bool[] solido;
        private int[] celdasSolidas = new int[0];
        private bool bordesAbiertos;

        //---------------------------------------------------------------------
        // Constructor.
        //---------------------------------------------------------------------
        public CFluido(int nx, int ny, int iteraciones = 40, double omega = 1.0,
                       double dt = 1.0, double disipacion = 0.0)
        {
            this.nx = nx;
            this.ny = ny;
            this.ancho = nx + 2;
            this.iteraciones = iteraciones;
            this.omega = omega;
            this.dt = dt;
            this.disipacion = disipacion;

            int total = (nx + 2) * (ny + 2);
            u = new double[total];
            v = new double[total];
            u0 = new double[total];
            v0 = new double[total];
            tinta = new double[total];
            tinta0 = new double[total];
            p = new double[total];
            div = new double[total];
            vort = new double[total];
            solido = new bool[total];
        }

        //---------------------------------------------------------------------
        // Getters. Devuelven el arreglo sin copia: solo lo leen la vista y los
        // tests (decisión registrada en PLAN-FLUIDOS.md).
        //---------------------------------------------------------------------
        public int GetNx() { return nx; }
        public int GetNy() { return ny; }
        public int GetIteraciones() { return iteraciones; }
        public long GetTicks() { return ticks; }
        public double GetDt() { return dt; }
        public double[] GetU() { return u; }
        public double[] GetV() { return v; }
        public double[] GetTinta() { return tinta; }
        public double[] GetPresion() { return p; }
        public bool GetProyeccion() { return proyeccionActiva; }

        public void SetProyeccion(bool activa)
        {
            proyeccionActiva = activa;
        }

        public bool GetConfinamiento() { return confinamientoActivo; }

        public void SetConfinamiento(bool activo)
        {
            confinamientoActivo = activo;
        }

        internal void SetEpsilonConfinamiento(double epsilon)
        {
            epsilonConfinamiento = epsilon;
        }

        public bool GetObstaculo() { return celdasSolidas.Length > 0; }
        public int[] GetCeldasSolidas() { return celdasSolidas; }

        //---------------------------------------------------------------------
        // Fase 3. Activar el obstáculo pone el círculo y abre los bordes
        // (entrada y salida); desactivarlo vuelve a la caja cerrada de las
        // fases 0 a 2. Los tests usan las dos mitades por separado.
        //---------------------------------------------------------------------
        public void SetObstaculo(bool activo)
        {
            SetBordesAbiertos(activo);
            SetCirculo(activo);
        }

        internal void SetBordesAbiertos(bool abiertos)
        {
            bordesAbiertos = abiertos;
        }

        // Círculo de radio RadioObstaculo, centrado en x = redondeo(Nx/3) y
        // una celda por encima del centro vertical (j crece hacia abajo).
        // El descentrado rompe la simetría para que la estela alterne.
        internal void SetCirculo(bool activo)
        {
            Array.Clear(solido, 0, solido.Length);
            List<int> lista = new List<int>();
            if (activo)
            {
                int xc = (int)Math.Round(nx / 3.0, MidpointRounding.AwayFromZero);
                int yc = (ny + 1) / 2 - 1;
                for (int j = 1; j <= ny; j++)
                {
                    for (int i = 1; i <= nx; i++)
                    {
                        int dx = i - xc, dy = j - yc;
                        if (dx * dx + dy * dy <= RadioObstaculo * RadioObstaculo)
                        {
                            solido[IX(i, j)] = true;
                            lista.Add(IX(i, j));
                        }
                    }
                }
            }
            celdasSolidas = lista.ToArray();
            AnularSolido(u);
            AnularSolido(v);
            AnularSolido(tinta);
        }

        // Posición en el arreglo de la celda (columna i, fila j).
        public int IX(int i, int j)
        {
            return i + ancho * j;
        }

        //---------------------------------------------------------------------
        // Fuentes. Modifican el campo al instante; quien las llama antes de
        // Paso() cumple el orden "fuentes → advección → ...". Fuera del
        // interior no hacen nada.
        //---------------------------------------------------------------------
        // Las tres fuentes ignoran las celdas del sólido: si no, la advección
        // leería esa tinta o esa fuerza desde dentro del círculo.
        public void AgregarTinta(int i, int j, double cantidad)
        {
            if (EsFluido(i, j))
            {
                tinta[IX(i, j)] += cantidad;
            }
        }

        // Sube la tinta de la celda hasta "minimo" sin pasarse: llamarla en
        // cada tick sobre la misma celda no la hace crecer sin límite (la
        // misma regla que el chorro).
        public void AsegurarTinta(int i, int j, double minimo)
        {
            if (EsFluido(i, j))
            {
                int k = IX(i, j);
                tinta[k] = Math.Max(tinta[k], minimo);
            }
        }

        public void AgregarFuerza(int i, int j, double fu, double fv)
        {
            if (EsFluido(i, j))
            {
                u[IX(i, j)] += fu * dt;
                v[IX(i, j)] += fv * dt;
            }
        }

        //---------------------------------------------------------------------
        // Chorro fijo desde la izquierda: una franja centrada en vertical,
        // en las columnas 1 a 3. La tinta se fija a 1 (no se suma) para que
        // no crezca sin límite; la fuerza sí se suma en cada tick.
        //---------------------------------------------------------------------
        public void AgregarChorro()
        {
            int centro = (ny + 1) / 2;
            int mitad = Math.Max(1, ny / 12);

            for (int j = centro - mitad; j <= centro + mitad; j++)
            {
                for (int i = 1; i <= Math.Min(3, nx); i++)
                {
                    if (EsInterior(i, j))
                    {
                        int k = IX(i, j);
                        tinta[k] = Math.Max(tinta[k], 1.0);
                        u[k] += FuerzaChorro * dt;
                    }
                }
            }
        }

        //---------------------------------------------------------------------
        // Un tick: confinamiento (si está activo, es una fuente más) →
        // advección de velocidad → proyección → advección de tinta →
        // disipación de la tinta. Sin paso de difusión (ver Registro). Con la
        // proyección apagada se salta solo ese paso.
        //---------------------------------------------------------------------
        public void Paso()
        {
            if (confinamientoActivo)
            {
                AplicarConfinamiento();
            }

            Array.Copy(u, u0, u.Length);
            Array.Copy(v, v0, v.Length);
            Advectar(1, u, u0, u0, v0);
            Advectar(2, v, v0, u0, v0);

            if (proyeccionActiva)
            {
                Proyectar();
            }

            Array.Copy(tinta, tinta0, tinta.Length);
            Advectar(4, tinta, tinta0, u, v);

            if (disipacion > 0)
            {
                double factor = 1.0 - disipacion;
                for (int k = 0; k < tinta.Length; k++)
                {
                    tinta[k] *= factor;
                }
            }

            ticks++;
        }

        //---------------------------------------------------------------------
        // Advección semi-lagrangiana: cada celda sigue la velocidad hacia
        // atrás un tick e interpola (bilineal) entre las 4 celdas de
        // alrededor del punto de origen. El retroceso se recorta a
        // [0.5, N + 0.5] para no leer fuera del arreglo.
        // b dice qué regla de pared aplicar al final (ver AplicarParedes).
        //---------------------------------------------------------------------
        internal void Advectar(int b, double[] destino, double[] origen, double[] uu, double[] vv)
        {
            for (int j = 1; j <= ny; j++)
            {
                for (int i = 1; i <= nx; i++)
                {
                    int k = IX(i, j);
                    double x = i - dt * uu[k];
                    double y = j - dt * vv[k];

                    if (x < 0.5) x = 0.5;
                    if (x > nx + 0.5) x = nx + 0.5;
                    if (y < 0.5) y = 0.5;
                    if (y > ny + 0.5) y = ny + 0.5;

                    int i0 = (int)x;
                    int j0 = (int)y;
                    double s1 = x - i0;
                    double s0 = 1.0 - s1;
                    double t1 = y - j0;
                    double t0 = 1.0 - t1;

                    destino[k] =
                        s0 * (t0 * origen[IX(i0, j0)] + t1 * origen[IX(i0, j0 + 1)]) +
                        s1 * (t0 * origen[IX(i0 + 1, j0)] + t1 * origen[IX(i0 + 1, j0 + 1)]);
                }
            }
            AplicarParedes(b, destino);
        }

        //---------------------------------------------------------------------
        // Divergencia en cada celda interior con diferencias centrales:
        // div = 0.5 · (u[i+1] − u[i−1] + v[j+1] − v[j−1]). Positiva: la
        // celda se vacía. Escribe en el arreglo interno y lo devuelve.
        //---------------------------------------------------------------------
        internal double[] Divergencia()
        {
            for (int j = 1; j <= ny; j++)
            {
                for (int i = 1; i <= nx; i++)
                {
                    div[IX(i, j)] = 0.5 * (u[IX(i + 1, j)] - u[IX(i - 1, j)] +
                                           v[IX(i, j + 1)] - v[IX(i, j - 1)]);
                }
            }
            AplicarParedes(0, div);
            return div;
        }

        //---------------------------------------------------------------------
        // Proyección: busca p con Laplaciano(p) = div (Laplaciano compacto de
        // 5 puntos) con k pasadas de Gauss-Seidel (SOR si omega > 1) y resta
        // su gradiente a la velocidad. p arranca en 0 en cada llamada.
        // Sólidos (Fase 3): una vecina sólida "tiene la presión de la celda
        // central", así que no empuja a través de la pared. En Gauss-Seidel
        // eso deja p = (suma de las vecinas fluidas − div) / (nº de fluidas);
        // sin sólidos son 4 vecinas y es la fórmula de siempre (dividir entre
        // 4 da el mismo resultado exacto que multiplicar por 0.25).
        // La divergencia no necesita regla: la velocidad del sólido ya es 0.
        //---------------------------------------------------------------------
        internal void Proyectar()
        {
            Divergencia();
            Array.Clear(p, 0, p.Length);

            for (int pasada = 0; pasada < iteraciones; pasada++)
            {
                for (int j = 1; j <= ny; j++)
                {
                    for (int i = 1; i <= nx; i++)
                    {
                        int k = IX(i, j);
                        if (solido[k])
                        {
                            continue;
                        }

                        double suma = 0;
                        int fluidas = 0;
                        if (!solido[k - 1]) { suma += p[k - 1]; fluidas++; }
                        if (!solido[k + 1]) { suma += p[k + 1]; fluidas++; }
                        if (!solido[k - ancho]) { suma += p[k - ancho]; fluidas++; }
                        if (!solido[k + ancho]) { suma += p[k + ancho]; fluidas++; }
                        if (fluidas == 0)
                        {
                            continue;
                        }

                        double gs = (suma - div[k]) / fluidas;
                        p[k] += omega * (gs - p[k]);
                    }
                }
                AplicarParedes(3, p);
            }

            // Resta del gradiente: la misma regla. Si la vecina es sólida se
            // usa la presión central; si no, se leería la p del sólido, que
            // nadie actualiza, y empujaría el fluido hacia la pared.
            for (int j = 1; j <= ny; j++)
            {
                for (int i = 1; i <= nx; i++)
                {
                    int k = IX(i, j);
                    if (solido[k])
                    {
                        continue;
                    }
                    double centro = p[k];
                    double este = solido[k + 1] ? centro : p[k + 1];
                    double oeste = solido[k - 1] ? centro : p[k - 1];
                    double sur = solido[k + ancho] ? centro : p[k + ancho];
                    double norte = solido[k - ancho] ? centro : p[k - ancho];
                    u[k] -= 0.5 * (este - oeste);
                    v[k] -= 0.5 * (sur - norte);
                }
            }
            AplicarParedes(1, u);
            AplicarParedes(2, v);
        }

        //---------------------------------------------------------------------
        // Vorticidad = ∂v/∂x − ∂u/∂y con diferencias centrales, en las celdas
        // interiores. En el anillo pegado al borde usa celdas de pared, cuyos
        // valores son artificiales: quien la mida debe saltarse ese anillo.
        //---------------------------------------------------------------------
        public double[] Vorticidad()
        {
            for (int j = 1; j <= ny; j++)
            {
                for (int i = 1; i <= nx; i++)
                {
                    vort[IX(i, j)] = 0.5 * (v[IX(i + 1, j)] - v[IX(i - 1, j)]) -
                                     0.5 * (u[IX(i, j + 1)] - u[IX(i, j - 1)]);
                }
            }
            return vort;
        }

        //---------------------------------------------------------------------
        // Fase 5. Vorticity confinement (Fedkiw, Stam y Jensen, 2001).
        // η = ∇|ω| apunta hacia el núcleo del remolino; N = η / |η|; la
        // fuerza f = ε·(N_y·ω, −N_x·ω) es N × ω en 2D: tangente al giro y en
        // su mismo sentido, así que devuelve el giro que quita la
        // interpolación. Se usa en índices (i, j) tal cual: ω y f se definen
        // respecto a los mismos ejes, así que la j hacia abajo no cambia
        // ningún signo (lo comprueba Confinamiento_RefuerzaElGiro).
        // Se salta el anillo pegado al borde (∇|ω| leería celdas de pared)
        // y las celdas del sólido. Lee vort y escribe u y v: no se pisan.
        //---------------------------------------------------------------------
        private void AplicarConfinamiento()
        {
            Vorticidad();
            for (int j = 2; j < ny; j++)
            {
                for (int i = 2; i < nx; i++)
                {
                    int k = IX(i, j);
                    if (solido[k])
                    {
                        continue;
                    }
                    double ex = 0.5 * (Math.Abs(vort[IX(i + 1, j)]) - Math.Abs(vort[IX(i - 1, j)]));
                    double ey = 0.5 * (Math.Abs(vort[IX(i, j + 1)]) - Math.Abs(vort[IX(i, j - 1)]));
                    double largo = Math.Sqrt(ex * ex + ey * ey) + MinimoGradiente;
                    double factor = dt * epsilonConfinamiento * vort[k] / largo;
                    u[k] += factor * ey;
                    v[k] -= factor * ex;
                }
            }
        }

        //---------------------------------------------------------------------
        // Paredes de la caja, como en Stam. b = 1: u (horizontal), su celda
        // de borde izquierdo/derecho es el negativo de su vecina, así que en
        // la pared (el promedio) vale 0. b = 2: lo mismo para v arriba y
        // abajo. b = 0: la celda de borde copia a su vecina. b = 3 (presión)
        // y b = 4 (tinta) se comportan como 0 en la caja cerrada; solo
        // cambian con los bordes abiertos. Las esquinas son el promedio de
        // sus dos vecinas de borde. Con obstáculo, u, v y la tinta valen 0
        // dentro del sólido.
        //---------------------------------------------------------------------
        internal void AplicarParedes(int b, double[] x)
        {
            for (int j = 1; j <= ny; j++)
            {
                x[IX(0, j)] = b == 1 ? -x[IX(1, j)] : x[IX(1, j)];
                x[IX(nx + 1, j)] = b == 1 ? -x[IX(nx, j)] : x[IX(nx, j)];
            }
            for (int i = 1; i <= nx; i++)
            {
                x[IX(i, 0)] = b == 2 ? -x[IX(i, 1)] : x[IX(i, 1)];
                x[IX(i, ny + 1)] = b == 2 ? -x[IX(i, ny)] : x[IX(i, ny)];
            }
            if (bordesAbiertos)
            {
                AplicarEntradaYSalida(b, x);
            }
            if (b == 1 || b == 2 || b == 4)
            {
                AnularSolido(x);
            }
            x[IX(0, 0)] = 0.5 * (x[IX(1, 0)] + x[IX(0, 1)]);
            x[IX(0, ny + 1)] = 0.5 * (x[IX(1, ny + 1)] + x[IX(0, ny)]);
            x[IX(nx + 1, 0)] = 0.5 * (x[IX(nx, 0)] + x[IX(nx + 1, 1)]);
            x[IX(nx + 1, ny + 1)] = 0.5 * (x[IX(nx, ny + 1)] + x[IX(nx + 1, ny)]);
        }

        //---------------------------------------------------------------------
        // Bordes abiertos (Fase 3), sobre lo que ya dejó la regla de paredes
        // en las columnas 0 y nx + 1:
        // - Izquierda (entrada): u = U, v = 0, tinta 1 en la franja del chorro;
        //   la presión copia a su vecina (la velocidad ya está fijada).
        // - Derecha (salida): u, v y tinta copian a su vecina; presión 0.
        //   Ese 0 hace que la ecuación de presión tenga solución: con paredes
        //   en todos los lados haría falta que entrara tanto como sale.
        // b = 0 (divergencia) copia en los dos lados: ya lo hizo la pared.
        //---------------------------------------------------------------------
        private void AplicarEntradaYSalida(int b, double[] x)
        {
            int centro = (ny + 1) / 2;
            int mitad = Math.Max(1, ny / 12);

            for (int j = 1; j <= ny; j++)
            {
                int izquierda = IX(0, j);
                int derecha = IX(nx + 1, j);
                switch (b)
                {
                    case 1:
                        x[izquierda] = CourantEntrada / dt;
                        x[derecha] = x[IX(nx, j)];
                        break;
                    case 2:
                        x[izquierda] = 0;
                        x[derecha] = x[IX(nx, j)];
                        break;
                    case 3:
                        x[derecha] = 0;
                        break;
                    case 4:
                        x[izquierda] = Math.Abs(j - centro) <= mitad ? 1.0 : 0.0;
                        x[derecha] = x[IX(nx, j)];
                        break;
                }
            }
        }

        private void AnularSolido(double[] x)
        {
            foreach (int k in celdasSolidas)
            {
                x[k] = 0;
            }
        }

        private bool EsInterior(int i, int j)
        {
            return i >= 1 && i <= nx && j >= 1 && j <= ny;
        }

        private bool EsFluido(int i, int j)
        {
            return EsInterior(i, j) && !solido[IX(i, j)];
        }
    }
}
