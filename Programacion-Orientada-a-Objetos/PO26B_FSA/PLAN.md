# Plan: Acuario (proyecto final POO)

Formato: adaptado de disenar-agente · checklist v1.2 (sin los campos propios de agentes: autonomía, permisos, evals y consumo).

Estado global: Fases 0 a 4 terminadas · Fase 5 en curso (falta la prueba manual)
Leyenda: [ ] pendiente · [~] en curso · [x] terminada
Decisiones: [Firme] ya decidida · [Validar en Fase N] supuesto a comprobar

Regla de trabajo (desde 2026-11-17): Claude propone cada paso, lo explica y justifica el código; Alfredo aprueba; Claude implementa. Cada propuesta incluye "qué debes entender para defenderlo".

## Estado actual (2026-11-17)
- **Fase 2 [x]:** prueba manual en verde (ver "Resultado" de la Fase 2).
- **Nombres:** se quedan como están (`TimerAcuario`, `BtnPractica2_Click`); no hay renombres pendientes.
- **Fase 3 [x]:** prueba manual en verde (ver "Resultado" de la Fase 3).
- **Población inicial:** se queda en 20 / 10 / 3 (lo que tiene `CParametros`).
- **Fase 4 [x]:** tests 25/25 y prueba manual en verde (ver "Resultado" de la Fase 4).
- **Fase 5 [~]:** implementada tras la aprobación; compila sin advertencias; tests 32/32 en verde.
- **Siguiente paso:** prueba manual de la Fase 5 (Alfredo): aparecen plantas cada ~1.5 s; los peces que comen lo suficiente tienen crías a su lado; la población no pasa de 150 ni se traba la app. Después, Claude propone la Fase 6.

**Prompt para continuar:**
~~~
Lee CLAUDE.md y PLAN.md (sección "Estado actual"). Seguimos en la fase indicada.
Propón el paso, explícalo y justifícalo; implementa solo cuando lo apruebe.
~~~

## Resumen de la arquitectura

- **Decisión:** las entidades son objetos normales guardados en una `List<>` dentro del ecosistema; se dibujan en el `Paint` de un solo control y avanzan con un único `System.Windows.Forms.Timer`.
- **Alternativa descartada:** cada entidad como control (`PictureBox`, como `CDron`) con su propio hilo.
- **Qué haría revisarla:** que el dibujo parpadee o vaya lento con ~150 entidades aun con doble búfer.

## Mapa de fases

| Fase | Entregable demostrable | Depende de |
|---|---|---|
| 0 | Panel del acuario abre en `MesaDePracticas4`; un contador de ticks avanza y se detiene al cerrar | — |
| 1 | Diseño de clases escrito por Alfredo y revisado | 0 |
| 2 | Entidades dibujadas quietas en posiciones aleatorias | 1 |
| 3 | Peces que se mueven y rebotan en los bordes | 2 |
| 4 | Energía: comer y morir, con tests | 3 |
| 5 | Nacimientos: crías y plantas nuevas, con tope | 4 |
| 6 | Estadísticas y botones Iniciar/Pausar, Reiniciar, Agregar | 5 |
| 7 | Ecosistema equilibrado; criterio de terminado del MVP cumplido | 6 |

---

## Fase 0: Validación y esqueleto  [x]
**Objetivo:** confirmar supuestos y tener el panel con un reloj que se enciende y se apaga bien.
**Entregable demostrable:** al abrir el panel, una etiqueta cuenta ticks; al cerrar la mesa el conteo para y la app no queda viva.
**Depende de:** nada.
**Decisiones:** [Validar en Fase 0] el profesor no pide requisitos extra (hilos, archivos). [Validar en Fase 0] `PictureBox` trae doble búfer de fábrica.
**Tareas:** preguntar al profesor; en el diseñador, agregar al panel elegido el control del área y una etiqueta de prueba; conectar un `Timer` que se detenga al ocultar el panel y al cerrar el formulario.
**Criterio de aceptación:** compila con `/compilar`; el contador avanza, se detiene al cambiar de panel y al cerrar la mesa; el Administrador de tareas no muestra el proceso tras cerrar la app.
**Pruebas y evaluación:** manual (lo anterior). Sin MSTest: aún no hay lógica.
**Fuera de alcance:** cualquier entidad.
**Riesgos:** el profesor pide algo distinto → reajustar fases antes de seguir.
**Para que lo entiendas tú:** en qué hilo corre el `Tick` del `Timer` y por qué eso evita los problemas de `CDron`.
**Punto de revisión:** ¿algún supuesto resultó falso?
**Resultado (2026-11-17):** pruebas manuales en verde. El profesor no pide requisitos extra. El doble búfer de `PictureBox` sigue sin comprobar: se valida en la Fase 3, cuando haya movimiento.

---

## Fase 1: Diseño de clases  [x]
**Objetivo:** que Alfredo deduzca las clases, la herencia y quién conoce a quién.
**Entregable demostrable:** un texto o diagrama (en este archivo o aparte) con clases, atributos, métodos y relaciones, revisado críticamente.
**Depende de:** 0.
**Decisiones:** [Firme] plantas son entidades; la base solo guarda lo que comparten todas. Por resolver aquí: quién detecta choques y quién decide qué se come a qué; cuándo se quitan los muertos y se agregan los nacidos durante un paso; dónde viven los valores ajustables; qué pasa si una especie se extingue.
**Tareas:** responder las preguntas guía de Claude; escribir el diseño; anotar cada decisión en el registro.
**Criterio de aceptación:** el diseño cumple: (a) el ecosistema nunca pregunta el tipo concreto para hacer actuar a una entidad; (b) la energía no se modifica directo desde fuera; (c) ninguna clase hereda atributos que no le corresponden; (d) agregar un cuarto tipo no obliga a tocar el ecosistema.
**Pruebas y evaluación:** revisión del diseño contra (a)–(d).
**Fuera de alcance:** escribir código.
**Riesgos:** sobrediseño (interfaces o capas sin problema real) → quitar lo que no evite un coste hoy.
**Para que lo entiendas tú:** diferencia entre herencia ("es un") y composición ("tiene un"), con un ejemplo de tu diseño.
**Punto de revisión:** ¿el diseño cambia el orden o el contenido de las fases 2–6?
**Resultado (2026-11-17):** no cambia el orden. Diseño aprobado:

```
CEntidad (abstracta)        posición X,Y · tamaño · energía (privada) · viva
 ├─ CPlanta                 no se mueve; su energía es lo que nutre
 └─ CPez (abstracta)        dirección dx,dy · velocidad · edad · edad máxima
     ├─ CHerbivoro          PuedeComer(e): e es CPlanta
     └─ CDepredador         PuedeComer(e): e es CHerbivoro

CEcosistema                 List<CEntidad> · ancho, alto · Paso() · Dibujar(g)
CParametros (static)        constantes ajustables (velocidad, energías, topes)
MesaDePracticas4            tiene un CEcosistema; Tick → Paso() + Invalidate()
```

- `CEntidad`: `abstract Dibujar(Graphics)`, `virtual Actualizar()` (vacío en la planta), `virtual PuedeComer(CEntidad)` (por defecto `false`), `int SerComido()` (marca muerta y devuelve su energía), `protected GanarEnergia` / `GastarEnergia`.
- `CPez`: `Actualizar()` mueve, rebota, gasta energía y envejece; `Comer(presa)` hace `GanarEnergia(presa.SerComido())`.
- Cumple (a) el ecosistema solo llama métodos de `CEntidad`; (b) energía `private`, cambia solo por `Comer`/`SerComido`; (c) la planta no hereda movimiento ni edad; (d) un tipo nuevo solo define su `PuedeComer`.

---

## Fase 2: Entidades en pantalla  [x]
**Objetivo:** crear el ecosistema inicial y dibujarlo.
**Entregable demostrable:** al abrir el panel se ven plantas, herbívoros y depredadores quietos, en posiciones aleatorias dentro del área.
**Depende de:** 1.
**Decisiones:** [Firme] un solo `Random` `static`, en `CEcosistema`. Figuras de color provisionales; sprites pixel-art después. [Validar en Fase 2] `PbxAcuario.ClientSize` ya tiene su tamaño real en la primera llamada a `PnlProyectoF_VisibleChanged`.
**Tareas:** las clases de la Fase 1 (cada `.cs` nuevo agregado a mano a `POO26B_FSA.csproj`); creación inicial; dibujo en el `Paint`.
**Criterio de aceptación:** compila; ninguna entidad sale del área; reiniciar el formulario da posiciones distintas.
**Pruebas y evaluación:** MSTest: la posición inicial siempre queda dentro de los límites.
**Fuera de alcance:** movimiento.
**Riesgos:** olvidar el `.csproj` → error de compilación "no se encuentra el tipo".
**Para que lo entiendas tú:** por qué el `Paint` puede dibujar todo sin saber el tipo de cada entidad.
**Punto de revisión:** ¿dibujar quedó dentro de cada entidad o en el formulario? ¿por qué?
**Resultado (2026-11-17):** compila; tests 16/16 en verde; prueba manual en verde (nada cortado ni tapado por `LblTicks`; posiciones distintas al reabrir la mesa). `PbxAcuario` está al frente. `BtnPrueba` ya no existe. Supuesto validado: `PbxAcuario.ClientSize` ya tiene su tamaño real en la primera llamada a `PnlProyectoF_VisibleChanged`. El dibujo quedó dentro de cada entidad (ver registro).

---

## Fase 3: Movimiento  [x]
**Objetivo:** que los peces naden.
**Entregable demostrable:** con el reloj corriendo, los peces se mueven, rebotan en los bordes y a veces cambian de dirección; las plantas no se mueven.
**Depende de:** 2.
**Decisiones:** [Firme] movimiento aleatorio con rebote; buscar comida es extensión. [Validar en Fase 3] doble búfer de `PictureBox` (heredado de la Fase 0). Por decidir: cómo usan los peces el `Random` de `CEcosistema`.
**Tareas:** paso de simulación por tick; redibujo.
**Criterio de aceptación:** fluido con ~150 entidades; ningún pez sale del área.
**Pruebas y evaluación:** MSTest: un pez en el borde invierte su dirección.
**Fuera de alcance:** energía y choques.
**Riesgos:** parpadeo → revisar el doble búfer (Fase 0).
**Para que lo entiendas tú:** posición nueva = posición + dirección × velocidad, y cómo el rebote invierte una componente.
**Punto de revisión:** ¿el intervalo del Timer da buena fluidez?
**Resultado (2026-11-17):** compila; tests 19/19 en verde (incluye rebote en el borde, 1000 pasos dentro del área y planta quieta). Prueba manual en verde: sin parpadeo, los peces rebotan en los cuatro bordes sin salir ni temblar, las plantas no se mueven, fluido con ~150 entidades (80 / 50 / 20). Supuesto validado (pendiente desde la Fase 0): el doble búfer de `PictureBox` basta, sin código extra. Punto de revisión: `Interval` = 33 ms da buena fluidez.

---

## Fase 4: Energía, comer y morir  [x]
**Objetivo:** aplicar las reglas de energía y de choques.
**Entregable demostrable:** herbívoros que comen plantas, depredadores que comen herbívoros, peces que mueren de hambre o de viejos y desaparecen.
**Depende de:** 3.
**Decisiones:** las de choques y retiro de muertos tomadas en la Fase 1.
**Tareas:** energía y edad encapsuladas; detección de choques; retiro de muertos sin modificar la lista mientras se recorre.
**Criterio de aceptación:** no hay `InvalidOperationException`; la energía nunca queda negativa ni se cambia desde fuera.
**Pruebas y evaluación:** MSTest: gastar energía, comer suma energía, energía 0 → muere, edad máxima → muere.
**Fuera de alcance:** reproducción.
**Riesgos:** todo muere en segundos → normal aquí; el equilibrio es la Fase 7.
**Para que lo entiendas tú:** por qué `Remove` dentro de un `foreach` sobre la misma lista falla.
**Punto de revisión:** ¿quién decide "esto se puede comer" respeta el criterio (a) de la Fase 1?
**Resultado (2026-11-17):** compila; tests 25/25 en verde (gastar energía, comer suma, no comer lo prohibido, presa ya comida, energía 0 sin negativos, muerte por edad con un pez de prueba). Punto de revisión: sí; `PuedeComer` vive en cada subclase y el ecosistema solo llama a `Comer` de `CEntidad`. Prueba manual en verde: se ve comer y desaparecer, sin excepciones. Nota: `MilPasos_NingunPezSaleDelArea` ahora cubre ~600 pasos de movimiento de los herbívoros (mueren de hambre y se quedan quietos).

---

## Fase 5: Nacimientos  [~]
**Objetivo:** crías y plantas nuevas.
**Entregable demostrable:** peces con energía suficiente tienen crías cerca; aparecen plantas cada cierto tiempo hasta un máximo; la población nunca supera el tope.
**Depende de:** 4.
**Decisiones:** [Validar en Fase 5] reproducción asexual basta para el MVP.
**Tareas:** umbral, costo y espera de reproducción; aparición de plantas; agregar nacidos sin romper el recorrido.
**Criterio de aceptación:** ninguna cría nace fuera del área; la cría recibe energía del padre (no se crea de la nada).
**Pruebas y evaluación:** MSTest: bajo el umbral no se reproduce; al reproducirse el padre pierde energía; en espera no se reproduce.
**Fuera de alcance:** macho/hembra (extensión E3).
**Riesgos:** explosión de población → revisar costo y tope.
**Para que lo entiendas tú:** por qué transferir energía al hijo frena el crecimiento sin control.
**Punto de revisión:** ¿los valores ajustables siguen en un solo lugar?
**Resultado parcial (2026-11-17):** compila; tests 32/32 en verde (bajo el umbral no hay cría, la energía se conserva al reproducirse, en espera no hay cría, la cría es del mismo tipo y nace dentro, la planta no se reproduce, umbral > energía de cría, 3000 pasos sin pasar el tope). Punto de revisión: sí, todo en `CParametros`. Falta la prueba manual.

---

## Fase 6: Interfaz y estadísticas  [ ]
**Objetivo:** controlar y observar la simulación.
**Entregable demostrable:** botones Iniciar/Pausar, Reiniciar y Agregar (planta, herbívoro, depredador); panel con conteos por tipo y tiempo.
**Depende de:** 5.
**Decisiones:** [Firme] estilo pixel-art Terraria; controles desde el diseñador.
**Tareas:** pasos en el diseñador; handlers `Control_Evento`; conteos leídos del ecosistema.
**Criterio de aceptación:** pausar congela sin perder estado; reiniciar vuelve al estado inicial; los conteos coinciden con lo que se ve.
**Pruebas y evaluación:** manual; MSTest del conteo por tipo si vive en la lógica.
**Fuera de alcance:** gráfica de población (E2).
**Riesgos:** el formulario empieza a contener reglas de la simulación → moverlas a la lógica.
**Para que lo entiendas tú:** por qué el formulario pregunta al ecosistema y no cuenta por su cuenta.
**Punto de revisión:** ¿el formulario solo muestra y reenvía acciones?

---

## Fase 7: Equilibrio y cierre del MVP  [ ]
**Objetivo:** que el ecosistema no colapse y el MVP quede verificado.
**Entregable demostrable:** las tres especies conviven al menos 2 minutos seguidos.
**Depende de:** 6.
**Decisiones:** valores finales de los parámetros, anotados en el registro.
**Tareas:** ajustar parámetros probando; repasar el criterio de terminado.
**Criterio de aceptación (terminado del MVP):**
- [ ] Abre desde `MesaDePracticas4` con la navegación existente.
- [ ] Botones funcionan; nada sale del área.
- [ ] Se ve comer, morir de hambre y de viejo, nacer crías y aparecer plantas.
- [ ] Conteos coinciden con la pantalla.
- [ ] Tres especies sobreviven ≥ 2 minutos.
- [ ] Cerrar con la simulación corriendo no deja procesos ni lanza excepciones.
- [ ] El ecosistema no pregunta tipos concretos; la energía no se modifica desde fuera.
- [ ] Tests de gastar, comer, morir y reproducirse en verde (`/test`).
- [ ] Todo `.cs` nuevo en `POO26B_FSA.csproj`; compila sin errores.
**Pruebas y evaluación:** suite completa de MSTest + tres corridas manuales de 2 minutos.
**Fuera de alcance:** extensiones.
**Riesgos:** no se logra el equilibrio → revisar la razón energía por comida / gasto y la densidad de plantas.
**Para que lo entiendas tú:** por qué las poblaciones oscilan (presa-depredador) y qué distingue oscilar de colapsar.
**Punto de revisión:** ¿qué extensión aporta más a demostrar dominio?

---

## Extensiones (después del MVP)
E1 buscar comida · E2 gráfica de población (`DataVisualization.Charting`) · E3 macho y hembra · E4 parámetros en vivo · E5 clic para ver datos de un pez · E6 guardar/cargar en archivo · E7 cuarto tipo de entidad (prueba del polimorfismo).

## Registro de decisiones
| Fecha | Fase | Decisión | Alternativa descartada | Motivo |
|---|---|---|---|---|
| 2026-11-10 | — | Proyecto: acuario (ecosistema) | Drones (B), combate por turnos (C) | Cubre listas, composición y ciclo de vida; no depende de `CDron` |
| 2026-11-10 | — | Vive en un panel de `MesaDePracticas4` | Proyecto aparte | Debe estar en la solución |
| 2026-11-10 | — | Un solo `Timer` | Un hilo por objeto | Evita hilos de primer plano y llamadas entre hilos |
| 2026-11-10 | — | Entidades como objetos dibujados en `Paint` | Un `PictureBox` por entidad | Transparencia entre controles y rendimiento; suma polimorfismo en el dibujo |
| 2026-11-10 | — | Plantas son entidades | Plantas aparte | Una sola colección y trato uniforme; la base guarda solo lo común |
| 2026-11-10 | — | [Validar en Fase 5] reproducción asexual; movimiento aleatorio | Macho/hembra; buscar comida | Menor alcance para el MVP |
| 2026-11-17 | 0 | Sin requisitos extra del profesor ("la implementación demuestra el dominio") | Reajustar fases | Respuesta del profesor |
| 2026-11-17 | 0 | El `Timer` arranca y para en `PnlProyectoF_VisibleChanged` | Hacerlo en `AlternarPractica` | La navegación no conoce al acuario; también cubre el cierre de la mesa |
| 2026-11-17 | — | Claude propone y escribe el código tras la aprobación de Alfredo | Alfredo escribe con guía | Decisión de Alfredo |
| 2026-11-17 | 1 | `CEcosistema` detecta choques; cada entidad decide con `PuedeComer` | El ecosistema decide por tipos | Criterio (a) |
| 2026-11-17 | 1 | Energía en `CEntidad`; comer = transferir energía con `SerComido()` | Energía solo en peces; `SetEnergia` | Comer igual para todos; criterio (b) |
| 2026-11-17 | 1 | Muertos con `RemoveAll` y nacidos en lista aparte, ambos al final de `Paso()` | Modificar la lista al recorrerla | Evita `InvalidOperationException` |
| 2026-11-17 | 1 | Valores ajustables en `CParametros` (static) | Constantes repartidas | Ajuste en un solo lugar (Fase 7) |
| 2026-11-17 | 1 | Extinción: sin regla especial | Reintroducir automáticamente | El botón Agregar basta para el MVP |
| 2026-11-17 | 1 | `is` dentro de `PuedeComer` de cada subclase | Doble despacho | Más simple; el ecosistema sigue sin preguntar tipos |
| 2026-11-17 | 1 | Cada entidad se dibuja a sí misma | Clase dibujadora aparte | Capa sin un problema real hoy |
| 2026-11-17 | 2 | Población inicial: 20 plantas, 10 herbívoros, 3 depredadores (en `CParametros`) | — | Punto de partida; se ajusta en la Fase 7 |
| 2026-11-17 | 2 | `Random` `static` en `CEcosistema` | Uno por entidad; clase aparte | Hoy solo el ecosistema lo usa; se revisa en la Fase 3 |
| 2026-11-17 | 2 | Figuras de color, provisionales | Sprites pixel-art ya | Validar el dibujo antes de invertir en arte |
| 2026-11-17 | 2 | Encimado al aparecer: se pospone; si hace falta, muestreo con rechazo solo para plantas (Fase 5 o 6) | Evitarlo ya | No rompe criterios hoy; los choques de la Fase 4 no lo resuelven |
| 2026-11-17 | 2 | Se quedan los nombres actuales (`TimerAcuario`, `BtnPractica2_Click`) | Renombrar a `TmrAcuario` / `BtnProyectoF_Click` | Decisión de Alfredo |
| 2026-11-17 | 3 | Bordes y `Random` pasados como parámetros: `Actualizar(ancho, alto, azar)` | El pez guarda una referencia al ecosistema; otro `Random` `static` en `CPez` | El pez no depende de `CEcosistema`; una sola semilla; tests con semilla fija |
| 2026-11-17 | 3 | Rebote con `Math.Abs` (signo siempre hacia adentro), en orden girar → avanzar → corregir | `dx = -dx` | `-dx` puede dejar al pez temblando en el borde si gira hacia afuera |
| 2026-11-17 | 3 | Dirección como vector unitario `(cos θ, sin θ)` + `velocidad` aparte (píxeles por tick) | `dx, dy` al azar en [-1, 1]; píxeles por segundo | Misma rapidez en cualquier dirección; el intervalo es fijo |
| 2026-11-17 | 3 | Valores iniciales: velocidad 2 / 2.5, giro 2 % por tick, `Interval` 33 ms | — | Punto de partida; se ajusta en la Fase 7 |
| 2026-11-17 | 4 | `Comer` sube a `CEntidad` (público, con las reglas dentro); `SerComido` queda `private` | `Comer` en `CPez` (diseño de la Fase 1) | Con `Comer` en `CPez` el ecosistema tendría que convertir a `CPez` (rompe a); `private` es por clase, así nadie de fuera quita energía (b) |
| 2026-11-17 | 4 | `Paso()` en tres etapas: actualizar → choques (todos contra todos) → `RemoveAll` | Quitar al detectar la muerte | Solo se marca durante los recorridos; ~22 500 pares por tick bastan |
| 2026-11-17 | 4 | Energía 600 / 900 / planta 300, gasto 1 por tick, edad máxima 1800 / 2700 ticks | — | Punto de partida; se ajusta en la Fase 7 |
| 2026-11-17 | 4 | Sin tope de energía ni edad máxima con azar | Ponerlos ya | No rompen criterios de la Fase 4; se revisan en las Fases 5 y 7 |
| 2026-11-17 | 5 | Cada pez crea a su cría (`Reproducirse` virtual en `CEntidad`, `CrearCria` abstracto en `CPez`) | El ecosistema crea la cría según el tipo | Criterios (a) y (d) |
| 2026-11-17 | 5 | La cría recibe `EnergiaCria` del padre (transferencia) | Energía nueva para la cría | Sin transferencia la energía total crece sin freno |
| 2026-11-17 | 5 | Tope revisado antes de `Reproducirse`; nacidos en lista aparte + `AddRange` | Revisar después | El padre no paga por una cría rechazada |
| 2026-11-17 | 5 | Un solo tope global (150) y una planta cada 45 ticks | Tope aparte de plantas | Contar plantas exigiría preguntar el tipo; se revisa en la Fase 7 si las plantas llenan el acuario |
| 2026-11-17 | 5 | Cría en la posición del padre; umbral 1000, cría 400, espera 300 ticks, comunes a las dos especies | Separarla; valores por especie | Nace dentro por construcción; se ajusta en la Fase 7 |

## Riesgos globales
- Requisitos del profesor no confirmados → Fase 0.
- Colapso del ecosistema → parámetros en un solo lugar y Fase 7.
- Modificar la lista mientras se recorre → decidirlo en Fase 1.
- Sobrediseño → cada clase o capa debe evitar un coste real hoy.
