namespace PO26B_FSA.Acuario
{
    //---------------------------------------------------------------------
    // Valores ajustables de la simulación, reunidos en un solo lugar.
    //---------------------------------------------------------------------
    internal static class CParametros
    {
        //---------------------------------------------------------------------
        // Población inicial.
        //---------------------------------------------------------------------
        public const int PlantasIniciales = 20;
        public const int HerbivorosIniciales = 10;
        public const int DepredadoresIniciales = 3;

        //---------------------------------------------------------------------
        // Tamaño (lado del cuadrado que ocupa cada entidad, en píxeles).
        //---------------------------------------------------------------------
        public const float LadoPlanta = 12f;
        public const float LadoHerbivoro = 14f;
        public const float LadoDepredador = 20f;

        //---------------------------------------------------------------------
        // Movimiento. Velocidad en píxeles por tick; la probabilidad de giro
        // es por tick y por pez (0.02 = 2 %).
        //---------------------------------------------------------------------
        public const float VelocidadHerbivoro = 2f;
        public const float VelocidadDepredador = 2.5f;
        public const double ProbabilidadGiro = 0.02;

        //---------------------------------------------------------------------
        // Energía y edad. La energía se gasta por tick; la edad se mide en
        // ticks (con Interval = 33 ms, 30 ticks ≈ 1 s).
        //---------------------------------------------------------------------
        public const int EnergiaPlanta = 300;
        public const int EnergiaInicialHerbivoro = 600;
        public const int EnergiaInicialDepredador = 900;
        public const int GastoPorTick = 1;
        public const int EdadMaximaHerbivoro = 1800;
        public const int EdadMaximaDepredador = 2700;

        //---------------------------------------------------------------------
        // Nacimientos. UmbralReproduccion debe ser mayor que EnergiaCria para
        // que el padre nunca muera al reproducirse (hay un test que lo vigila).
        //---------------------------------------------------------------------
        public const int UmbralReproduccion = 1000;
        public const int EnergiaCria = 400;
        public const int EsperaReproduccion = 300;
        public const int PoblacionMaxima = 150;
        public const int IntervaloPlanta = 45;
    }
}
