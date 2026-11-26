# Plan: Simulador de fluidos (Stable Fluids) en `PnlProyectoFIA`

Formato: planificar-fases v1 · módulos: software

## Estado actual
- Fase siguiente: 4, Guion de la demo y ensayo [ ] · Fase 3 [x] (2026-11-25) · Fase 2 [x] (2026-11-25) · Fase 1 [x] (2026-11-25) · Fase 0 [x] (2026-11-25) · Última actualización: 2026-11-25
- Combinación vigente: 160×59, k = 20, SOR ω = 1.7, `NearestNeighbor` (constantes en `MesaDePracticas4.cs` y en `CFluidoTests.cs`).
- Historial de las fases cerradas (decisiones completas, mediciones, sondas y filas archivadas del Registro): `PLAN-FLUIDOS-historial.md`. Solo hace falta leerlo para entender por qué se decidió algo; lo vigente está en este plan.
- **Siguiente acción:**
  1. Replanificar la Fase 4 con planificar-fases: la demo será un vídeo grabado en el PC de Alfredo y proyectado (ver Registro). El objetivo, la lista previa y el minuto 4:15 del guion actuales suponen una demo en vivo.
  2. Alfredo decide si el guion incorpora lo observado en las fases 2 y 3 (ver Preguntas abiertas).

Leyenda: [ ] pendiente · [~] en curso · [h] hecho sin verificar · [x] verificado (fecha)
Decisiones: [Firme] ya decidida · [Validar en Fase N] supuesto a comprobar
Hechos externos: [volátil] dato que caduca: se anota con fuente y fecha
Modo de trabajo: `CLAUDE.md` de usuario y del proyecto. Excepción de este plan, autorizada por Alfredo: **Claude escribe todo el código**, sin la estructura de 5 partes; Alfredo hace el diseñador y las pruebas manuales. Sin commit ni push.

**Hecho siempre incluye:** el `.csproj` principal compila con el MSBuild de Visual Studio, el proyecto de tests compila y `dotnet test ... --no-build` pasa (comandos en el `CLAUDE.md` del proyecto). Todo `.cs` nuevo está en `POO26B_FSA.csproj`.

**Presupuesto:** ~10 h con Alfredo. Estimación por fase en el mapa; si una fase pasa de 1.5× su estimación, se para y se decide en el punto de revisión.

## Resumen de la arquitectura
- **Decisión:** solucionador "Stable Fluids" de Stam (1999/2003) con valores en el centro de las celdas, cuadrícula rectangular Nx×Ny con celdas cuadradas (el panel mide 910×379, proporción ~2.4:1), `dt` constante por tick. Advección semi-lagrangiana con interpolación bilineal; proyección con k iteraciones de Gauss-Seidel; paredes de la caja como en Stam; sin paso de difusión (viscosidad). Un solo hilo. La cuadrícula es fija y se dibuja centrada con su proporción (franjas de fondo a los lados si el panel cambia de forma).
  - `Fluidos\CFluido.cs` (namespace `PO26B_FSA.Fluidos`): estado (velocidad, tinta, presión, divergencia, máscara de sólidos) y `Paso()`. Sin nada de WinForms: se prueba sin pantalla.
  - `Fluidos\CVistaFluido.cs`: funciones puras de campo → color y pantalla → celda, y el relleno de un `Bitmap` reutilizado con `LockBits`.
  - `MesaDePracticas4.cs`: solo conecta Timer, ratón y botones con `CFluido` y `CVistaFluido`, como hace con el acuario.
- **Alternativa descartada:** cuadrícula escalonada (MAC), que da una proyección exacta pero un indexado más complejo; Lattice Boltzmann, que tiene límites de estabilidad y puede explotar con un tirón del ratón en vivo; partículas (SPH), más caras.
- **Qué observación la haría revisar:** que la proyección reduzca la divergencia menos del 80 % (r > 0.2) con k = 40 en todas las N que cumplen el tiempo, **incluso con SOR**, o que el **piso** de r (r con k = 2000, ver la Fase 0) ya pase de 0.2, o que el patrón de tablero de ajedrez en la vista de presión impida explicarla (→ considerar MAC en "Después del MVP").

## Mapa de fases
| Fase | Entregable demostrable | Escribe | Estimación | Estado |
|---|---|---|---|---|
| 0 | Chorro de tinta que se mueve solo + mediciones registradas | Claude (Alfredo: diseñador) | 2.5 h | [x] 2026-11-25 |
| 1 | Tinta y fuerza con el ratón, pausa y reinicio: **demo mínima** | Claude (Alfredo: diseñador) | 1.5 h | [x] 2026-11-25 |
| 2 | Vistas de presión y vorticidad + interruptor de proyección | Claude (Alfredo: diseñador) | 1.5 h | [x] 2026-11-25 |
| 3 | Obstáculo circular fijo que se activa y desactiva | Claude (Alfredo: diseñador) | 2 h | [x] 2026-11-25 |
| 4 | Guion de la demo ensayado + lista previa | Alfredo (guion: Claude) | 1 h | [ ] |
| 5 | *Vorticity confinement* (esqueleto) | Claude | 1 h | [ ] |
| — | Margen para imprevistos | — | 0.5 h | — |

Las fases 0 a 2 bastan para una demo completa. La 3 añade lo vistoso. La 5 es mejora, salvo que la Fase 0 la convierta en requisito (ver su decisión).

---

## Fase 0: Validación  [x] 2026-11-25
Detalle completo (decisiones, tareas, tablas de medición, sondas de la tinta): `PLAN-FLUIDOS-historial.md`, sección "Fase 0".
**Resultado:** un chorro de tinta que se mueve solo; el solucionador, la vista y los tests en verde.
**Vigente para las fases siguientes:**
- Combinación: 160×59, k = 20, SOR ω = 1.7, `NearestNeighbor`, `Interval = 15` (~63 ticks/s). La demo va en Release + Ctrl+F5.
- `Paso()`: fuentes → advección de la velocidad → proyección → advección de la tinta → disipación de la tinta. Solo se disipa la tinta; no hay difusión. `Proyectar` borra `p` al empezar (arranque en frío).
- Escena de prueba (la usa la Fase 5): vórtice suave v_θ(ρ) = V·s·e^((1 − s²)/2), con s = ρ/R y tinta 1 en la mitad superior del disco; fuente radial (u, v) = A·(x − xc, y − yc)·e^(−ρ²/R²), con A = V·√2·e^(1/2)/R, solo para medir r. R = máx(3, redondeo(Ny/8)), V·`dt` = 1.5.
- `Diagnostico_PorCombinacion` (`[TestCategory("Diagnostico")]`, escribe `diagnostico.txt`) se queda; las ejecuciones normales lo excluyen con `--filter "TestCategory!=Diagnostico"`. r, la tinta y el pulso se miden en las celdas interiores.
- Mediciones de la combinación vigente: r = 0.0502 (umbral del test 2·r = 0.100); variación de la tinta 67.87 % (límite del test 68 %: propiedad del método, ver Registro); pulso hasta el 10 % = 424 ticks (≥ 120: la Fase 5 es mejora); velocidad máxima con chorro 0.70 celdas/tick (estable, sin amortiguar).
- Tiempo con la ventana maximizada en el monitor de Alfredo (2560×1440): 61-64 ticks/s con carga; máximo de ms total 10-15. El tope de ≤ 10 ms se comprueba en la pantalla de la demo (Fase 4); plan B: no maximizar.
- Horas: aproximadamente las 2.5 h estimadas.

---

## Fase 1: Interacción con el ratón  [x] 2026-11-25
Detalle completo: `PLAN-FLUIDOS-historial.md`, sección "Fase 1".
**Resultado:** arrastrar deja tinta y empuja el fluido; Pausar y Reiniciar funcionan; al abrir el panel arranca el chorro. Alfredo lo verificó a mano el 2026-11-25: esquinas, centro, ventana normal y maximizada, pausa, reinicio en pausa y el clic quieto con `AsegurarTinta` (la mancha queda ocre, no oscura). 61 tests en verde.
**Vigente para las fases siguientes:**
- El ratón se acumula entre ticks y se gasta en el siguiente `Paso()`; un arrastre rápido se pinta a lo largo del segmento.
- Celda = 1 + ⌊(x − r.X)·Nx / r.Width⌋, recortada a [1, Nx] (igual en vertical), con r el rectángulo de *letterbox*.
- Pincel: disco de radio 2 celdas con `AsegurarTinta` (máx(tinta, 1)). Fuerza = desplazamiento del tick en celdas, con tope 1.5, aplicada una vez por celda y tick.
- En pausa el ratón no hace nada; pausar, ocultar el panel o reiniciar descarta el trazo pendiente.
- `LblFluido` muestra "Ticks: N"; los ticks/s y los ms solo aparecen con la constante `bool` de medición (la usa la lista previa de la Fase 4).
- `DisipacionFluido` = 0.003 y `FuerzaChorro` = 0.1.
- Horas: no registradas.

---

## Fase 2: Ver lo que calcula el solucionador  [x] 2026-11-25
**Objetivo:** mostrar la presión y el giro que calcula el método, y qué pasa si se apaga la proyección.
**Entregable demostrable:** un botón recorre las vistas Tinta → Presión → Vorticidad; otro enciende y apaga la proyección. Sin proyección, un empujón en línea recta sigue recto, la tinta se aprieta en un frente estrecho y ya no se enrolla en remolinos (inferencia, se confirma a la vista); los que ya había siguen siendo transportados. La tinta **no** se ve más oscura donde el fluido converge: la advección solo interpola valores que ya existen (`Advectar_NoCreaMaximosNuevos`), así que el "amontonamiento" de la velocidad no se ve en la vista Tinta; solo lo mostraría una vista de divergencia ("Después del MVP"). En la vista de vorticidad sí aparecen dos bandas a los lados del trazo: la fuerza misma tiene giro en sus bordes (inferencia, se confirma a la vista). Lo que falta es que esas bandas se enrollen.
**Escribe:** Claude. Alfredo hace los pasos del diseñador.
**Decisiones:**
- [Firme] Campos con signo: positivo en ocre `#8C6420`, negativo en ciruela `#6B4E7A`, 0 en el fondo `#F5F4EE`. Se evita el salvia: verde frente a ocre es la pareja que confunde el daltonismo rojo-verde. Fórmula: s = x / e, recortado a [−1, 1]; color = fondo + (extremo − fondo)·|s| (lineal; la curva raíz se probó y se revirtió, ver Registro).
- [Firme] **La presión se centra antes de colorearla.** En la caja cerrada, las paredes fijan cuánto cambia la presión, no su nivel: sumarle una constante a toda la presión da la misma velocidad. Así, el signo (ocre o ciruela) dependería de dónde quede esa constante tras k iteraciones. Antes de dibujar se resta la media de la presión en las celdas interiores, solo en `CVistaFluido`; el solucionador no cambia. Con el obstáculo activado (Fase 3), la salida fija p = 0 y la resta deja de ser necesaria, pero se mantiene por simplicidad. La vorticidad no lo necesita: su signo sí tiene significado (sentido del giro).
- [Firme] **Signo de la presión:** ocre = alta, ciruela = baja, comprobado por `Presion_FuenteRadial_CentroEsBaja`. En la fuente radial la divergencia es positiva en el centro, y ∇²p = div > 0 indica un mínimo de presión: el centro debe salir ciruela. Si sale ocre, el signo está invertido en el código, no en la física.
- [Firme] **Sentido del giro:** horario en pantalla = ocre, antihorario = ciruela. La fila j crece hacia abajo, así que ∂v/∂x − ∂u/∂y positivo ya es giro horario en pantalla y `ColorDivergente` se usa sin cambiar el signo. Lo comprueba `Vorticidad_GiroHorarioEnPantalla_EsPositiva`.
- [Firme] **Vista Presión con la proyección apagada:** `p` solo cambia dentro de `Proyectar()`, así que queda congelada. Se sigue dibujando ese campo y encima, en la esquina superior izquierda del rectángulo de dibujo, un aviso tipo chip: fondo `245; 244; 238`, borde de 1 px `221; 217; 206`, texto `43; 42; 39` en Segoe UI 10 pt, "Sin proyección: presión congelada". Solo en la vista Presión: la vorticidad sale de la velocidad, que sigue cambiando.
- [Validar en Fase 2] **Velocidad sin proyección.** El 0.70 celdas/tick de la Fase 0 se midió con la proyección encendida. Se añade a `Diagnostico_PorCombinacion` una fila con el chorro sin proyección durante 1000 ticks, anotando máx\|u\|·`dt` cada 100 ticks. Si pasa de 3 celdas/tick, el guion limita la proyección apagada a ~10 s de arrastre. No se añade código: la advección sigue siendo estable con velocidades grandes; el riesgo es solo visual.
- [Firme] La escala e se suaviza para que los colores no parpadeen cuando cambia el máximo: e ← máx(máx|x| del tick, 0.95·e, ε), con ε = 1e-6. El ε evita dividir 0 entre 0 (NaN) al arrancar, cuando la presión vale 0 en todas partes y e aún es 0.
- [Firme] Vorticidad = ∂v/∂x − ∂u/∂y con diferencias centrales (la función existe desde la Fase 0); se calcula solo cuando se muestra.
- [Firme] El interruptor de proyección es un `bool` de `CFluido`; las paredes se siguen aplicando.
- [Validar en Fase 2] Hipótesis sin comprobar: con el Laplaciano compacto de Stam la presión suele salir suave, y el modo par-impar queda más bien en la velocidad; por eso es probable que esto no haga falta. Si la vista de presión muestra un tablero de ajedrez que distrae → mostrar la presión suavizada (promedio con las 4 vecinas solo al dibujar) y anotarlo; no se cambia el solucionador.
**Tareas:**
- *Alfredo, en el diseñador:* en `FlpControlesFluido`, `BtnFluidoVista` (texto "Vista: Tinta") y `BtnFluidoProyeccion` (texto "Proyección: sí").
- *Claude:* `ColorDivergente`, el modo de vista y el aviso "Sin proyección: presión congelada" en `CVistaFluido`; interruptor de proyección en `CFluido` (la vorticidad ya existe desde la Fase 0); los handlers; la fila sin proyección en `Diagnostico_PorCombinacion`.
**Hecho cuando:** las tres vistas se ven y el botón muestra la actual; con la proyección apagada, la diferencia es visible en menos de 5 segundos de arrastre; los tests pasan.
**Verificación:**
- Automática: `PresionCentrada_MediaCero` (una presión con una constante sumada da el mismo campo centrado), `ColorDivergente_CeroEsElFondo`, `ColorDivergente_SignosVanACadaExtremo`, `ColorDivergente_CampoTodoCero_NoDaNaN`, `Presion_FuenteRadial_CentroEsBaja` (tras proyectar la fuente radial y centrar `p`, la celda central es negativa), `Vorticidad_GiroHorarioEnPantalla_EsPositiva` (con u = −ω·(y − yc), v = ω·(x − xc) y j hacia abajo, la vorticidad en el centro vale 2ω > 0), `SinProyeccion_PasoEsSoloAdveccion`: con la proyección apagada, `u` y `v` tras `Paso()` son exactamente los de aplicar a una copia del mismo estado las fuentes y luego `Advectar(1, …)` y `Advectar(2, …)` con `u0` y `v0`. Solo se comparan `u` y `v`, para no depender de la tinta ni de la disipación. Repite el orden de `Paso()`; se acepta porque es lo que distingue un interruptor que funciona de uno que no hace nada (si no hiciera nada, la proyección cambiaría la velocidad y el test falla). No se mide "la divergencia no se reduce": la advección bilineal suaviza y también baja el máximo de |div|, así que fallaría con código correcto. `Vorticidad_RotacionRigida_EsConstante` ya está desde la Fase 0.
- Manual: legibilidad de los colores en tu pantalla y, si puedes, en un proyector.
**Fuera de alcance:** vista de la divergencia antes y después de proyectar (ver "Después del MVP").
**Riesgos:** ocre y ciruela se ven parecidos en el proyector, porque tienen una luminosidad parecida → oscurecer el ciruela antes de cambiar de color.
**Qué debes entender:** qué es la presión en este método. Es el valor que hace que, en cada celda, entre tanto fluido como sale, y su gradiente empuja de alta a baja. Por eso, al apagarla, la velocidad deja de cumplir "entra tanto como sale" y un empujón ya no se enrolla en remolinos nuevos: sigue recto y la tinta se aprieta en un frente. La tinta no se oscurece donde el fluido converge, porque la advección solo interpola valores que ya existen. Los que ya existen no desaparecen: la advección los sigue transportando. Y la vista de vorticidad sigue mostrando el giro que mete la propia fuerza en los bordes del trazo. Además: por qué un mínimo de presión tiene Laplaciano positivo (como la segunda derivada positiva en el fondo de una parábola), y por qué invertir el eje vertical invierte el sentido del giro (la pantalla cuenta y hacia abajo, al revés que en matemáticas).
**Punto de revisión:** ¿qué vista lleva el peso en el guion?
**Notas:**
- 2026-11-25: código escrito por Claude. Los dos `.csproj` compilan; 68 de 68 tests en verde (`--filter "TestCategory!=Diagnostico"`).
- Velocidad sin proyección: la fila va en un test aparte, `Diagnostico_SinProyeccion` (`[TestCategory("Diagnostico")]`, escribe `diagnostico-sin-proyeccion.txt`), para no repetir las 20 combinaciones de `Diagnostico_PorCombinacion`. Resultado (160×59, k = 20, ω = 1.7, 1000 ticks): **0.53 celdas/tick, estable** desde el tick 100. Cumple ≤ 3: el guion no necesita limitar la proyección apagada. Es menor que con proyección (0.70), probablemente porque sin la presión el chorro no arrastra al fluido de alrededor (inferencia).
- El diseñador nombró el botón `BtnFluidoProyección` (con tilde) y su texto es "Proyección: Sí". El código usa ese nombre y "Sí"/"No".
- La escala e es una sola y vuelve a 0 al cambiar de vista; su máximo se toma sin el anillo de borde. Reiniciar conserva la vista y el estado de la proyección.
- Prueba manual con la proyección encendida (capturas de Alfredo, 2026-11-25; lectura de Claude):
  - Signos coherentes en las tres vistas. En el chorro: ciruela en el borde de arriba y ocre en el de abajo, como se predijo. En un arrastre hacia la derecha (tick 4095): ciruela arriba del trazo, ocre abajo y la punta empieza a enrollarse.
  - Tick 2591, en pausa, las tres vistas: la tinta gira en sentido antihorario alrededor de (x ≈ 550, y ≈ 500); en ese mismo punto la vorticidad es ciruela (antihorario) y la presión tiene un mínimo ciruela. Un remolino tiene presión baja en su centro: las tres vistas cuentan lo mismo.
  - Tick 420: dos mínimos de presión (núcleos del par de remolinos del frente del chorro) y ocre delante del frente.
  - Cambiar de vista en pausa funciona. No se ve tablero de ajedrez ni marco en las paredes; los bloques visibles son las celdas (`NearestNeighbor`).
  - Legibilidad: ocre y ciruela se distinguen en el monitor de Alfredo, pero el campo sale pálido: la entrada del chorro tiene los valores más altos y fija la escala.
  - Proyección apagada (ticks 12378 y 14588): el chorro cruza la caja en línea recta, en la tinta y en la vorticidad. Las dos bandas (ciruela arriba, ocre abajo) llegan a la pared derecha sin enrollarse. Coincide con la predicción. El aviso "presión congelada" aparece en la vista Presión (Alfredo).
  - Observación de Alfredo: al agitar mucho el ratón, la presión queda blanca salvo alrededor del cursor, y la vorticidad se vuelve más blanca cuanto más se agita. No es un error de cálculo: la escala es relativa (e = máximo del tick) y el cursor produce los valores más grandes de la caja, así que el resto queda cerca de 0 en color. Es la misma causa que la palidez por la entrada del chorro. Alfredo eligió la curva raíz; tras verla se revirtió a lineal (ver Registro).
  - Con la curva raíz (capturas de los ticks 448, 638 y 1140, sin agitar el ratón): toda la caja sale coloreada, sin fondo neutro, y aparecen líneas blancas finas donde el campo cruza 0. La raíz sube muy deprisa cerca de 0 (pendiente infinita), así que la transición por el fondo se comprime en una línea que parece un borde físico y no lo es. La física sigue cuadrando: en el tick 638, los dos núcleos de remolino de la vorticidad (uno ciruela y otro ocre) son los dos mínimos de presión. Un remolino baja la presión en su centro gire hacia donde gire.
  - Se vuelve a la escala lineal (Alfredo). Es la misma versión que se vio en todas las capturas anteriores, así que la verificación manual vale. Fase cerrada el 2026-11-25.

---

## Fase 3: Obstáculo circular fijo  [x] 2026-11-25
**Objetivo:** que el fluido rodee un círculo fijo y suelte remolinos detrás.
**Entregable demostrable:** un botón activa y desactiva un círculo en el primer tercio del área, una celda por encima del centro vertical; el fluido que entra por la izquierda lo rodea y se ven remolinos en su estela.
**Escribe:** Claude. Alfredo hace los pasos del diseñador.
**Decisiones:**
- [Firme] Máscara `bool[]` de celdas sólidas. La misma regla vale para cualquier forma, pero el círculo fijo evita la interfaz de dibujo y el vaciado del fluido atrapado al dibujar.
- [Firme] Reglas: la velocidad y la tinta valen 0 dentro del sólido; en la divergencia, la vecina sólida aporta velocidad 0; en Gauss-Seidel, la presión de la vecina sólida se toma igual a la de la celda central (no empuja a través de la pared). **La misma regla vale al restar el gradiente** (`u -= 0.5·(p[i+1] − p[i−1])`): si la vecina es sólida, se usa la presión de la celda central en su lugar. Si no, la resta leería la `p` del sólido, que nadie actualiza, y empujaría la celda de fluido hacia la pared. La advección no se modifica: si muestrea dentro del sólido lee 0, y se acepta esa pequeña fuga.
- [Firme] **Entrada y salida con el obstáculo activado.** En una caja cerrada el chorro solo recircula y no hay estela. Con el obstáculo activado, el borde izquierdo es entrada (velocidad fija u = U, v = 0, tinta en una franja) y el derecho es salida (velocidad y tinta copian a su vecina interior, presión 0 en ese borde); arriba y abajo siguen las paredes de Stam. La presión 0 hace falta: con paredes en todos los lados, la ecuación de presión solo tiene solución si entra tanto fluido como sale, y una entrada sin salida no lo cumple. Sin obstáculo, la caja vuelve a ser cerrada y las fases 0 a 2 no cambian.
- [Firme] El sólido se dibuja en `#DDD9CE` (bordes de la paleta).
- [Validar en Fase 3] **Tamaño del círculo y velocidad de entrada.** Diámetro D = 14 celdas (radio 7), centro en x = redondeo(Nx/3) ≈ 53 y una celda por encima del centro vertical; entrada con U·`dt` = 0.7 celdas/tick (lo que ya alcanza el chorro). Con el obstáculo activado no se llama a `AgregarChorro()`: la entrada lo sustituye. Motivo (estimación 1D, sin comprobar: ignora la proyección y las paredes): la interpolación lineal con desplazamiento fraccionario α actúa como una viscosidad ν ≈ α(1−α)·dx²/(2·`dt`), así que Re ≈ 2·C·D / α(1−α), con C = U·`dt`. Con C = 0.7 y D = 14, Re ≈ 2·0.7·14 / 0.21 ≈ 93, por encima del umbral de la calle de Kármán (~47) con margen para lo que la estimación ignora. D = 14 tapa el 24 % del alto (59 filas): las paredes estrecharán la estela. Si no hay remolinos, se sigue la lista de la decisión siguiente.
- [Firme] **El círculo va descentrado una celda en vertical.** La estela alterna (calle de Kármán) aparece al romperse la simetría. Con un chorro uniforme y un círculo centrado, una simulación determinista puede mantener mucho tiempo una estela simétrica: dos remolinos quietos detrás del círculo, que no cumplen el "Hecho cuando". Descentrarlo es lo más barato y no añade código nuevo.
- [Validar en Fase 3] ¿Se desprenden remolinos visibles? Aun con entrada y salida y el círculo descentrado puede no haberlos, porque la disipación numérica actúa como una viscosidad alta. Si no, en este orden: comprobar que el descentrado está aplicado; añadir una perturbación vertical pequeña a la entrada durante los primeros ticks; subir U; si no basta, adelantar la Fase 5.
**Tareas:**
- *Alfredo, en el diseñador:* `BtnFluidoObstaculo` (texto "Obstáculo: no") en `FlpControlesFluido`.
- *Claude:* máscara y reglas, y bordes de entrada y salida, en `CFluido`; dibujo del sólido en `CVistaFluido`; handler.
**Hecho cuando:** con el obstáculo activado, la tinta no entra en el círculo y la estela muestra remolinos alternos durante al menos 1900 ticks de entrada continua (~30 s a 63 ticks/s); desactivarlo quita el círculo y vuelve a la caja cerrada de las fases 0 a 2 (sin entrada ni salida); los tests pasan.
**Verificación:**
- Automática: `Solido_SinTintaNiVelocidadTrasPasos`, `Solido_FlujoUniforme_SeDesvia` (la velocidad vertical junto al círculo deja de ser 0; distingue una máscara que se aplica de una que se ignora), `EntradaYSalida_FlujoUniforme_SeMantiene` (con los bordes abiertos y sin máscara, un flujo uniforme de izquierda a derecha sigue uniforme tras 100 pasos).
- Manual: la estela, porque se juzga a la vista.
**Fuera de alcance:** obstáculos dibujados con el botón derecho (ver "Después del MVP").
**Riesgos:** no hay estela con remolinos → ver la decisión [Validar en Fase 3].
**Qué debes entender:** por qué una pared necesita dos reglas, una para la velocidad (no atravesarla) y otra para la presión (no empujar a través de ella); y por qué la salida necesita presión 0 para que la proyección tenga solución.
**Punto de revisión:** ¿queda tiempo para la Fase 5, o se dedica al ensayo?
**Notas:**
- 2026-11-25: código escrito por Claude. Los dos `.csproj` compilan; 72 de 72 tests en verde (4 nuevos: `Solido_SinTintaNiVelocidadTrasPasos`, `Solido_FlujoUniforme_SeDesvia`, `EntradaYSalida_FlujoUniforme_SeMantiene` y `SinObstaculo_VuelveALaCajaCerrada`, que cubre el "desactivarlo vuelve a la caja cerrada").
- Gauss-Seidel tiene un solo camino de código con la regla del sólido: sin sólidos son 4 vecinas y la fórmula es la de siempre (dividir entre 4 es exacto, igual que × 0.25). Por eso r (0.0502) y la tinta (68 %) no cambian. Coste medido sin pantalla (Release, 160×59, 1000 pasos): `Paso()` 1.93 ms de media y 2.64 ms de máximo sin obstáculo; 1.92 / 2.68 ms con obstáculo.
- `AplicarParedes` tiene dos códigos nuevos: b = 3 (presión) y b = 4 (tinta). En la caja cerrada se comportan como b = 0; con los bordes abiertos aplican la entrada y la salida. La tinta de la entrada usa la franja del chorro.
- Reiniciar conserva el estado del obstáculo, como la vista y la proyección. El texto inicial del botón en el diseñador es "Obstaculo: No" (sin tilde); el código escribe "Obstáculo: Sí/No". Para que coincida desde el arranque, corrígelo en el diseñador.
- Prueba manual, primera tanda (capturas de Alfredo hasta el tick 590; lectura de Claude):
  - La tinta de la entrada choca con el círculo, lo rodea y no entra (tick 204). Detrás se enrollan dos lóbulos todavía simétricos.
  - Vorticidad: desde el tick 7 hay ocre en el borde de arriba del círculo y ciruela en el de abajo. Es el signo correcto: arriba, el fluido de fuera va más rápido que el pegado al círculo, y eso es giro horario. En el tick 67 esas capas se despegan hacia atrás.
  - Presión, ticks 41-75: ocre fuerte delante del círculo (punto de remanso: ahí el fluido choca y se frena) y ciruela a sus lados, arriba y abajo (el fluido se acelera al rodearlo y su presión baja). Al arrancar, toda la caja baja de presión de izquierda a derecha: es el gradiente que acelera el canal entero.
  - Presión, tick 590: los mínimos de la estela están escalonados (arriba en x ≈ 1040, abajo en x ≈ 1280, otra vez arriba en x ≈ 1600). Es la disposición de una calle de Kármán: el desprendimiento alterno parece haber empezado. Falta confirmarlo durante 1900 ticks.
  - Desactivar el obstáculo vuelve al chorro en caja cerrada (ticks 321-358), con los signos de siempre. La estela que quedaba sigue siendo transportada.
  - Segunda tanda (ticks 1999 y 2049, con el obstáculo activado desde el tick 0): calle de Kármán confirmada. En la vorticidad, la estela alterna manchas ocre y ciruela. El remolino que se forma justo detrás del círculo es ciruela (abajo) en el tick 1999 y ocre (arriba) en el 2049: cambia de lado en ~50 ticks, como predecía la estimación de Strouhal (St ≈ 0.2, ciclo de ~107 ticks). La tinta (tick 1999) forma la estela ondulada en "S" típica.
  - La estela alterna aparece entre los ticks 200 (aún simétrica) y 590 (ya escalonada), es decir, a los 3-10 s. Cabe en el minuto 3:00-4:15 del guion.
  - En la vorticidad, la estela se ve más pálida que el borde del círculo, que fija la escala. En el monitor de Alfredo se lee; se revisa en el proyector con la lista previa de la Fase 4.
  - La decisión [Validar en Fase 3] (D = 14, U·`dt` = 0.7) queda confirmada sin usar la lista de recursos de reserva. La estimación del Reynolds acertó en este caso, pero un solo dato no valida la fórmula.
  - **Punto de revisión:** la estela sale sin *vorticity confinement*, así que la Fase 5 sigue siendo opcional. Lo siguiente es la Fase 4 (guion y ensayo).

---

## Fase 4: Guion de la demo y ensayo  [ ]
**Objetivo:** que la demo de 5 minutos salga igual en vivo que en el ensayo.
**Entregable demostrable:** dos ensayos cronometrados de principio a fin sin tocar código, y la lista previa marcada.
**Depende de:** Fase 2 (la Fase 3 es opcional; si no está, se salta el minuto 3).
**Escribe:** Alfredo ensaya; Claude escribió el guion (abajo) y lo ajusta a lo que exista.
**Decisiones:** [Firme] la demo se ejecuta en Release + Ctrl+F5, como decidió la Fase 0 (F5 no se mide).
**Tareas:**

*Guion (5 min)*

| Minuto | Qué se hace | Qué se dice |
|---|---|---|
| 0:00–0:45 | Abrir Mesa 4 → "Proyecto Final IA". El chorro ya se mueve. | "Es una simulación de fluido 2D con el método Stable Fluids de Jos Stam. En cada tick hace tres cosas: empujar, transportar y corregir la presión." |
| 0:45–1:45 | Arrastrar tinta en varias direcciones; dejar que se arremoline y se disipe. | "La tinta no tiene animación propia: es un número por celda que el fluido transporta. Lo que ven son los datos pintados." |
| 1:45–3:00 | Vista Presión → Vorticidad. Volver a Tinta, esperar a que se calme y apagar la proyección; arrastrar en línea recta; encenderla y repetir. | "Ocre es presión alta, ciruela baja. En la vorticidad, ocre gira como las agujas del reloj y ciruela al revés. Sin este paso, el empujón sigue recto y no se enrolla en remolinos: la presión es lo que obliga a que entre tanto fluido como sale, y eso es lo que lo hace comportarse como un líquido." (Esta parte se hace en la vista Tinta.) |
| 3:00–4:15 | Activar el obstáculo; esperar la estela. | "El fluido rodea el círculo y detrás se sueltan remolinos alternos, como el humo detrás de un poste." |
| 4:15–5:00 | Enseñar el Explorador de pruebas en verde, con los resultados de la ejecución hecha antes de la demo (no se pulsa "Ejecutar todas" en vivo: también lanzaría el diagnóstico, que es lento). Cierre. | "Las pruebas comprueban propiedades del método: que la proyección reduce la divergencia y que el transporte no inventa valores mayores que los que ya había." |

*Respuestas de una frase*
- **¿Qué es la presión aquí?** El número que el método calcula en cada celda para que entre tanto fluido como sale; su gradiente empuja de alta a baja presión.
- **¿Por qué no explota aunque arrastre fuerte?** El transporte mira hacia atrás e interpola entre valores que ya existen, así que no inventa valores mayores con ningún paso de tiempo; y la fuerza del ratón tiene un tope, porque las fuerzas sí suben la velocidad en cada tick. Además, una prueba aplica 1000 pasos con fuerzas grandes y no aparece ningún NaN. (No se dice que la presión "no añade energía": eso vale para una proyección exacta, y esta es aproximada.)
- **¿Por qué se apagan los remolinos?** Interpolar promedia vecinas en cada tick y eso suaviza el detalle; es una disipación numérica conocida del método.
- **¿Dónde está la programación orientada a objetos?** `CFluido` encapsula el estado y las reglas, `CVistaFluido` traduce datos a color y el formulario solo conecta, así que la física se prueba sin pantalla.
- **¿Es física exacta?** No: conserva la incompresibilidad de forma aproximada y puede perder o ganar mucha tinta (en la prueba, casi un 68 % en 200 ticks); sirve para gráficos, no para ingeniería.

*Lista previa (el día de la demo)*
- [ ] Compilado en la configuración decidida en la Fase 0, antes de llegar.
- [ ] Tests ejecutados antes de la demo, para que el Explorador de pruebas ya muestre el verde. En el Explorador: agrupar por rasgo (*trait*) y ejecutar solo el grupo sin `Diagnostico`, o ejecutar `dotnet test ... --no-build --filter "TestCategory!=Diagnostico"` y abrir el Explorador después.
- [ ] Portátil conectado a la corriente.
- [ ] Ratón, no touchpad.
- [ ] Probado con la resolución del proyector o de la pantalla duplicada.
- [ ] En esa pantalla, Presión y Vorticidad se leen: ocre y ciruela se distinguen y los remolinos destacan sobre el fondo. Si se ven pálidas, se cambia la constante `ExponenteColor` de `CVistaFluido` de 1.0 a 0.7 (cambiar una constante no cuenta como cambiar código; ver Registro), se compila y se vuelve a mirar.
- [ ] En esa pantalla, en Release + Ctrl+F5 y maximizado, el máximo de ms total es ≤ 10 (se activa la constante `bool` de medición de la Fase 1, se compila y se vuelve a apagar; cambiar una constante no cuenta como cambiar código). Si no lo es, la demo se hace con la ventana sin maximizar.
- [ ] Comprobación del ritmo: abrir el panel 10 s y ver que "Ticks" sube unas 10 × los ticks/s medidos en la Fase 0 (~60).

**Hecho cuando:** dos ensayos seguidos entran en 5:00 ± 0:30 sin errores y la lista está marcada.
**Verificación:** manual, cronometrada.
**Fuera de alcance:** cambiar código. Si el ensayo revela un fallo, se anota y se corrige en la fase que corresponde.
**Riesgos:** te pasas de tiempo → recortar el minuto 3 (obstáculo) antes que el 2 (presión).
**Qué debes entender:** las cinco respuestas de arriba, con tus palabras.
**Punto de revisión:** ¿queda tiempo para la Fase 5?
**Notas:**

---

## Fase 5: *Vorticity confinement*  [ ]  (esqueleto: depende de [Validar en Fase 0] y [Validar en Fase 3])
**Objetivo:** devolver a los remolinos la fuerza que les quita la disipación numérica. · **Entregable demostrable:** un interruptor que muestra los remolinos más vivos con la mejora que sin ella. · **Hecho cuando:** con la mejora, el pulso de la Fase 0 (escena de prueba, mismo N y k) tarda al menos el doble de ticks en bajar al 10 % de su pico, y `MilPasosConFuerzasGrandes_SinNaN` sigue pasando.

---

## Después del MVP
- Obstáculos dibujados con el botón derecho (decidido como mejora, no como requisito).
- Vista de la divergencia antes y después de proyectar.
- Cuadrícula escalonada (MAC) si la presión en el centro de las celdas no convence ni con SOR.
- Advección MacCormack/BFECC con limitador.
- Flechas de velocidad (idea de Alfredo, 2026-11-25): una flecha cada ~8 celdas (~140 flechas), dibujada encima de la vista actual en `CVistaFluido.Dibujar` (`AdjustableArrowCap`) y activada con un botón propio. Aporta lo único que ninguna vista muestra: la velocidad misma; sobre Presión se vería el fluido frenar al llegar al ocre. Por decidir antes de hacerla: la escala de la longitud (con escala relativa, el cursor encoge las demás flechas) y su coste de dibujo, estimado en < 1 ms y sin medir. Se decide tras la Fase 3 o la 4, según el presupuesto.

## Preguntas abiertas
| Pregunta | Supuesto mientras tanto | Cuándo se resuelve |
|---|---|---|
| ¿El guion incorpora lo observado en las fases 2 y 3 (el centro de un remolino tiene presión baja; remanso delante del círculo y presión baja a los lados; ocre = giro horario)? | No: el guion queda como está | Al replanificar la Fase 4 |
| ¿Con qué programa y a qué resolución se graba el vídeo? | En el monitor de Alfredo (2560×1440), maximizado | Al replanificar la Fase 4 (la compresión del vídeo puede borrar los degradados pálidos) |

## Registro de decisiones
Las filas reemplazadas, o que solo describen cómo se midió en las fases 0 y 1, están en `PLAN-FLUIDOS-historial.md`.

| Fecha | Fase | Decisión | Alternativa descartada | Motivo |
|---|---|---|---|---|
| 2026-11-24 | — | "IA" en "Proyecto Final IA" significa que lo escribe Claude | Proyecto de inteligencia artificial | Confirmado por Alfredo |
| 2026-11-24 | — | Stam con valores en el centro de las celdas | MAC, Lattice Boltzmann, SPH | Estable con cualquier `dt` (fiable en vivo) y con una referencia clara; cabe en ~10 h |
| 2026-11-24 | — | Cuadrícula rectangular con celdas cuadradas | 128×128 estirada | El panel es 2.4:1; estirar deforma el fluido |
| 2026-11-24 | 3 | Círculo fijo primero; dibujar con el botón derecho es mejora | Forma libre desde el inicio | La máscara es la misma, pero el círculo evita la interfaz de dibujo y el fluido atrapado, y es predecible en la demo |
| 2026-11-24 | 0 | Demo en Release + Ctrl+F5, sin medir F5 | Optimizar para Debug; medir F5 por si cumplía | Sin coste y quita el riesgo del depurador; medir F5 no cambiaba ninguna decisión |
| 2026-11-24 | 2 | Ciruela frente a ocre, con 0 en el fondo | Salvia | Verde frente a ocre se confunde con daltonismo rojo-verde |
| 2026-11-24 | 0 | Un Timer propio (`TmrFluido`) | Compartir `TimerAcuario` | Los paneles se excluyen; cada simulación tiene su único reloj |
| 2026-11-24 | 0 | Sin hilos salvo que la Fase 0 lo exija | `Parallel.For` desde el inicio | Gauss-Seidel actualiza en su sitio; paralelizarlo cambia el algoritmo |
| 2026-11-24 | 0 | Sin paso de difusión (viscosidad) | Difusión implícita de Stam | La advección semi-lagrangiana ya suaviza (disipación numérica); difundir apagaría aún más los remolinos y añade otro solucionador de Gauss-Seidel por tick. No es un olvido: no añadirla sin revisar esta fila |
| 2026-11-24 | 0 | Cuadrícula fija dibujada con *letterbox* | Estirar a `ClientSize`; rehacer la cuadrícula al cambiar de tamaño | El panel tiene `Anchor` en los cuatro lados y cambia de proporción al maximizar; estirar deforma, rehacer reinicia la simulación |
| 2026-11-24 | 0 | Escena de prueba fija: vórtice suave para tinta, pulso y Fase 5; fuente radial para r; R = máx(3, redondeo(Ny / 8)), V = 1.5 | Un campo distinto por test, sin fijar; disco con rotación rígida y R = 6 | Sin escena fija, los umbrales dependen del campo y no del código; el salto del disco rígido falsea r y el pulso; una R fija roza las paredes con pocas filas |
| 2026-11-24 | 0 | `Refresh()` en el `Tick`, en la medición y en la demo | `Invalidate()` en la demo | Se mide lo mismo que se ejecuta; `WM_TIMER` no se acumula, así que repintar en el tick no retrasa la entrada |
| 2026-11-24 | 3 | Entrada a la izquierda y salida a la derecha (presión 0) con el obstáculo activado | Caja cerrada; bajar el criterio de la estela | En una caja cerrada el chorro recircula y no hay estela; sin salida, la ecuación de presión no tiene solución |
| 2026-11-24 | 0 | Disipación solo de la tinta; `Vorticidad()` se adelanta a la Fase 0 | Disipar también la velocidad; vorticidad en la Fase 2 | Amortiguar la velocidad (multiplicarla por un factor) no es difusión, pero también apaga los remolinos y mezclaría su efecto con la medición del pulso, que necesita la vorticidad. Si el chorro la necesita, se decide en la Fase 0 (velocidad máxima) |
| 2026-11-24 | 0 | `Advectar`, `Proyectar` y `Divergencia` `internal`; los getters devuelven el arreglo sin copia | `private`; copias de solo lectura | Los tests los llaman por separado; copiar en cada tick cuesta memoria sin evitar un error real hoy |
| 2026-11-24 | 2 | ε = 1e-6 en la escala de `ColorDivergente` | Sin mínimo | Al arrancar e = 0 y s = 0/0 da NaN |
| 2026-11-24 | 3 | Círculo descentrado una celda en vertical | Círculo centrado | Un montaje simétrico y determinista puede no soltar la estela alterna |
| 2026-11-24 | 4 | Demo en Release + Ctrl+F5 sin excepción | "Salvo que F5 haya cumplido" | Contradecía la Fase 0, que no mide F5 |
| 2026-11-24 | 3 | Sin obstáculo = caja cerrada | "Flujo libre" con entrada y salida | Quitar la contradicción entre la decisión y el "Hecho cuando" |
| 2026-11-24 | 0 | Tinta, pulso y velocidad máxima con chorro se miden en `Diagnostico_PorCombinacion` (sin pantalla), que se queda tras la Fase 1; chorro en `CFluido.AgregarChorro()` | Medirlos en la interfaz tras el barrido | No dependen del equipo; la Fase 5 necesita volver a medir el pulso; el formulario y el test usan el mismo chorro |
| 2026-11-24 | 3 | La regla del sólido para la presión vale también al restar el gradiente | Solo en Gauss-Seidel | La resta leería la `p` del sólido, que nadie actualiza, y empujaría el fluido hacia la pared |
| 2026-11-24 | 2 | "No se enrolla en remolinos" en lugar de "no se forman remolinos" | La frase anterior | La fuerza del arrastre tiene giro en sus bordes, y la vista de vorticidad lo mostraría aunque no haya proyección |
| 2026-11-24 | 0 | El diagnóstico se excluye con `--filter "TestCategory!=Diagnostico"` en las ejecuciones normales | Ejecutarlo siempre | Es lento (16 + 4 combinaciones, hasta 2000 ticks) y "Hecho siempre incluye" ejecuta los tests a menudo |
| 2026-11-24 | 2 | Sin proyección: "sigue recto y no se enrolla", sin "se amontona" | "El fluido se amontona donde se empuja" | La advección no crea máximos nuevos: la tinta no se oscurece donde converge la velocidad, así que la frase prometía algo que la vista Tinta no muestra |
| 2026-11-24 | 2 | Restar la media de la presión antes de colorearla (solo en la vista) | Colorear la presión tal cual | En la caja cerrada el nivel de la presión es arbitrario; sin centrarla, el signo del color no significaría "alta" o "baja" |
| 2026-11-24 | 4 | Respuesta "¿por qué no explota?" con transporte, presión y tope de fuerza | Solo "la advección no inventa valores mayores" | Las fuerzas sí suben la velocidad en cada tick; la respuesta anterior era cierta solo para la advección |
| 2026-11-24 | 4 | Tests ejecutados antes de la demo, sin "Ejecutar todas" en vivo | Ejecutarlos en la demo | "Ejecutar todas" en Visual Studio lanza también el diagnóstico, que es lento; el `--filter` solo existe en la línea de comandos |
| 2026-11-24 | 0 | 160×59, k = 20, SOR ω = 1.7 | 160×59, k = 40, sin SOR (lo que daba el criterio fijado) | Aprobado por Alfredo. El piso (0.030) muestra que el fallo era de convergencia; SOR ya estaba escrito (parámetro `omega`), hace la mitad de pasadas (el paso pesaba ~6-7 ms maximizado) y sube el pulso de 105 a 424 ticks, así que la Fase 5 no pasa a requisito |
| 2026-11-24 | 0 | Tope de 10 ms en la pantalla de la demo; en la Fase 0 bastan ticks/s ≥ 90 % y sin tirones; plan B: no maximizar | Seguir optimizando el dibujo (búfer propio, GDI) hasta ≤ 10 ms en el monitor de Alfredo; subir el tope | Aprobado por Alfredo. Se midió en un monitor 2560×1440 (área 2354×1111, ~2 M píxeles dibujados), más exigente que un proyector típico de 1920×1080; aun así, 61-64 ticks/s con carga, igual que sin carga. El tope es un margen apoyado en un modelo sin comprobar (salto a ~32 ticks/s): optimizar para él no evitaba ningún coste observado. No es un umbral sacado del resultado: el dato nuevo es la pantalla, y el tope se mantiene donde importa |
| 2026-11-24 | 0 | Tinta: se acepta la variación medida como propiedad del método; `TintaTotal_SinDisipacion_DentroDelMargen` pasa a ≤ 68 % (67.87 % medido con 160×59, k = 20, ω = 1.7) y queda como detector de regresiones | RK2 en el retroceso; conservar el 5 %; borrar el test | Aprobado por Alfredo. Sin error de código tras investigar (dos sondas, ver Notas). RK2 arregla el caso con la velocidad congelada (−100 % → −6.4 %) pero en el paso completo solo cambia el signo (−68 % → +63 %): manda la divergencia que la proyección en el centro de las celdas no ve. Arreglarlo exige advección conservativa o MAC ("Después del MVP"). En la demo la tinta se disipa a propósito, así que no se nota. Los errores que buscaba el 5 % (i/j, advección nula, paredes) los cubren otros tests |
| 2026-11-24 | 0 | Pulso: con la combinación vigente (SOR) es 424 ticks ≥ 120, así que la Fase 5 sigue siendo mejora | Fase 5 como requisito antes de la 3 (lo que daba k = 40 sin SOR: 105 ticks) | Consecuencia de elegir SOR |
| 2026-11-25 | 0 | El límite del 68 % de la tinta se mantiene aunque su margen sea pequeño (0.13 puntos); si un cambio legítimo del solucionador lo rompe (p. ej. la Fase 3), se vuelve a medir y se registra el nuevo límite | Banda alrededor de lo medido; borrar el test | Decidido por Alfredo: el producto ya funciona, y rediseñar el test sería sobre-ingeniería. Los errores que buscaba los cubren otros tests |
| 2026-11-25 | 1 | Los ticks/s y los ms de `LblFluido` quedan detrás de una constante `bool` | Borrarlos en la Fase 1 | La lista previa de la Fase 4 los necesita en la pantalla de la demo, y la Fase 4 no cambia código |
| 2026-11-25 | 0 | `NearestNeighbor` para el dibujo | `Bilinear` | Elegido por Alfredo a la vista. `Bilinear` cumplía el tiempo, pero cuesta ~3 ms más de media en el dibujo y oculta la cuadrícula que explica el guion |
| 2026-11-25 | 1 | Botones del fluido: Pausar relleno ocre, Reiniciar con contorno (como el acuario) | Los dos rellenos | Aprobado por Alfredo al confirmar la edición del `.Designer.cs` |
| 2026-11-25 | 1 | Fuerza del ratón = desplazamiento del tick en celdas, tope 1.5, aplicada una vez por celda y tick (`HashSet<Point>`) | Sumar la fuerza en cada disco del trazo | Con discos solapados, una celda recibía la fuerza varias veces y se pasaba el tope |
| 2026-11-25 | 1 | En pausa el ratón no hace nada; pausar, ocultar el panel o reiniciar descarta el trazo pendiente | Acumular el trazo y aplicarlo al reanudar | Un trazo acumulado en pausa saltaría de golpe al reanudar |
| 2026-11-25 | 1 | Pincel: disco de radio 2 celdas | Una sola celda | Con ~5.7 px por celda, una celda deja un trazo casi invisible |
| 2026-11-25 | 1 | El pincel fija la tinta a máx(tinta, 1) (`AsegurarTinta`), como el chorro | Sumar 1 por tick (`AgregarTinta`) | Aprobado por Alfredo. Sumando, el clic quieto llevaba la celda al color oscuro y tapaba el flujo |
| 2026-11-25 | 1 | `DisipacionFluido` se mantiene en 0.003 | Subirla (p. ej. 0.01, ~70 ticks de vida media) | Decidido por Alfredo a la vista: la acumulación de tinta tras ~3500 ticks no molesta en la demo |
| 2026-11-24 | 2 | `SinProyeccion_PasoEsSoloAdveccion` (igualdad exacta con fuentes + `Advectar`) | `SinProyeccion_LaDivergenciaNoSeReduce` | La advección bilineal también baja el máximo de \|div\|: el test anterior podía fallar con código correcto |
| 2026-11-25 | — | Fases 0 y 1 archivadas en `PLAN-FLUIDOS-historial.md`; el plan conserva un resumen de lo vigente | Dejar todo en el plan | El plan ocupaba ~34 000 tokens y cada sesión de fluidos lo lee entero; el detalle cerrado solo hace falta para entender una decisión |
| 2026-11-25 | 2 | Ocre = presión alta (test `Presion_FuenteRadial_CentroEsBaja`); en la vorticidad, ocre = giro horario en pantalla | Fijar los colores sin test; ocre = antihorario | Un signo invertido en el código haría falso el guion; con j hacia abajo, la vorticidad positiva ya es horaria y no hay que invertir nada |
| 2026-11-25 | 2 | Presión congelada con aviso "Sin proyección: presión congelada" al apagar la proyección | Pintar el fondo; dejar el campo sin aviso | Elegido por Alfredo. `p` solo cambia en `Proyectar()`: sin aviso, el campo congelado parecería actual |
| 2026-11-25 | 2 | `SinProyeccion_PasoEsSoloAdveccion` compara solo `u` y `v` | Comparar todo el estado | La tinta y la disipación obligarían a copiar todo `Paso()` en el test |
| 2026-11-25 | 2 | Medir la velocidad sin proyección en el diagnóstico (1000 ticks, tope 3 celdas/tick) | Suponer que sigue en 0.70 | El 0.70 se midió con proyección; sin ella nada reparte el impulso del chorro |
| 2026-11-25 | 2 | Intensidad de `ColorDivergente` lineal; la curva raíz se revierte. Si en el proyector (Fase 4) se ve pálido, se prueba \|s\|^0.7 | Raíz; exponente 0.7 ya | Elegido por Alfredo tras verla. La raíz coloreaba toda la caja (sin fondo neutro) y comprimía el paso por 0 en líneas finas que parecían bordes. Arreglaba un caso pasajero (agitar el ratón) y un riesgo sin observar (el proyector), a costa del caso normal, que con la escala lineal se leía bien |
| 2026-11-25 | 2 | *(Revertida)* Intensidad de `ColorDivergente` = √\|s\| (curva raíz) | Excluir la entrada del chorro del máximo; escala por percentil; dejarlo lineal | Elegida por Alfredo. El máximo lo fija una zona pequeña (entrada o cursor) y el resto quedaba pálido. Excluir la entrada no arregla el cursor. El percentil necesita más código y pinta el ruido saturado con la caja en calma. La escala ya era relativa, así que la proporción no se podía leer de todos modos; el guion solo usa el signo y la posición |
| 2026-11-25 | 3 | Círculo D = 14 celdas, entrada U·`dt` = 0.7, sin chorro con el obstáculo; "Hecho cuando" en 1900 ticks | Tamaño y velocidad sin fijar; 30 s | El Reynolds numérico estimado (~93 frente a ~47) depende de D y U; el tiempo de la simulación se mide en ticks |
| 2026-11-25 | 2 | La velocidad sin proyección se mide en un test aparte (`Diagnostico_SinProyeccion`) | Una fila más en `Diagnostico_PorCombinacion` | No repite las 20 combinaciones lentas. Resultado: 0.53 celdas/tick, ≤ 3 |
| 2026-11-25 | 2 | Reiniciar conserva la vista, la proyección y el obstáculo | Volver a los valores por defecto | Igual que respeta la pausa; los textos de los botones no se desincronizan |
| 2026-11-25 | 2 | `ExponenteColor` como constante (1.0) en `CVistaFluido` | Cambiar la línea el día de la demo | La Fase 4 no cambia código; cambiar una constante sí está permitido |
| 2026-11-25 | 3 | Gauss-Seidel con un solo camino de código que incluye la regla del sólido | Un camino rápido aparte para la caja cerrada | Sin sólidos da el mismo resultado exacto (dividir entre 4 es exacto); coste medido 1.9 ms/paso. Duplicar código no evitaba ningún coste observado |
| 2026-11-25 | 3 | Supuesto resuelto: D = 14 y U·`dt` = 0.7 bastan para la estela alterna | Recursos de reserva (perturbar la entrada, subir U, adelantar la Fase 5) | La estela alterna aparece a los 3-10 s y sigue en los ticks 1999-2049. La Fase 5 sigue siendo opcional |
| 2026-11-25 | 4 | Demo como vídeo grabado con antelación en el PC de Alfredo y proyectado | Demo en vivo en el portátil | Decidido por Alfredo. Quita los riesgos en vivo (ms, ritmo, ratón) y añade uno nuevo: la legibilidad del vídeo comprimido. La Fase 4 está pendiente de replanificar |

## Riesgos globales
- **Lentitud en vivo** → la Fase 0 mide en la configuración de la demo; lista previa de la Fase 4.
- **Saturación o pantalla vacía** → disipación, tope de fuerza, escena inicial y botón Reiniciar.
- **NaN que estropea la imagen** → test de 1000 pasos con fuerzas grandes y tope de fuerza.
- **Remolinos que se apagan** → medición en la Fase 0; la Fase 5 puede pasar a ser requisito.
- **Tiempo** → las fases 0 a 2 ya forman la demo; 3 y 5 se recortan sin romperla.
