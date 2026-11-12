# Plan: Acuario (proyecto final POO)

Formato: adaptado de disenar-agente · checklist v1.2 (sin los campos propios de agentes: autonomía, permisos, evals y consumo).

Estado global: Fase 0 pendiente
Leyenda: [ ] pendiente · [~] en curso · [x] terminada
Decisiones: [Firme] ya decidida · [Validar en Fase N] supuesto a comprobar

Regla de trabajo: Alfredo deduce el diseño y escribe el código. Claude guía con preguntas, revisa y explica; solo edita si se le pide explícitamente ("aplícalo").

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

## Fase 0: Validación y esqueleto  [ ]
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

**Prompt para Claude Code:**
~~~
Lee CLAUDE.md y PLAN.md. Trabajamos solo la Fase 0.
1. Explícame los pasos en el diseñador de Visual Studio para los controles que pide la fase.
2. Guíame con preguntas para que yo escriba la lógica del Timer; no la escribas tú salvo que diga "aplícalo".
3. Revisa mi código contra el criterio de aceptación y actualiza el estado en PLAN.md cuando lo cumpla.
~~~

---

## Fase 1: Diseño de clases  [ ]
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

**Prompt para Claude Code:**
~~~
Lee CLAUDE.md y PLAN.md. Trabajamos solo la Fase 1.
Hazme preguntas en orden para que yo deduzca el diseño de clases; no me des la jerarquía resuelta.
Critica mi diseño contra el criterio de aceptación y registra las decisiones en PLAN.md.
~~~

---

## Fase 2: Entidades en pantalla  [ ]
**Objetivo:** crear el ecosistema inicial y dibujarlo.
**Entregable demostrable:** al abrir el panel se ven plantas, herbívoros y depredadores quietos, en posiciones aleatorias dentro del área.
**Depende de:** 1.
**Decisiones:** [Firme] un solo `Random` `static`. [Validar en Fase 2] sprites pixel-art o figuras de color.
**Tareas:** las clases de la Fase 1 (cada `.cs` nuevo agregado a mano a `POO26B_FSA.csproj`); creación inicial; dibujo en el `Paint`.
**Criterio de aceptación:** compila; ninguna entidad sale del área; reiniciar el formulario da posiciones distintas.
**Pruebas y evaluación:** MSTest: la posición inicial siempre queda dentro de los límites.
**Fuera de alcance:** movimiento.
**Riesgos:** olvidar el `.csproj` → error de compilación "no se encuentra el tipo".
**Para que lo entiendas tú:** por qué el `Paint` puede dibujar todo sin saber el tipo de cada entidad.
**Punto de revisión:** ¿dibujar quedó dentro de cada entidad o en el formulario? ¿por qué?

**Prompt para Claude Code:**
~~~
Lee CLAUDE.md y PLAN.md. Trabajamos solo la Fase 2.
Guíame para implementar lo que pide; revisa mi código contra el criterio de aceptación y actualiza PLAN.md.
~~~

---

## Fase 3: Movimiento  [ ]
**Objetivo:** que los peces naden.
**Entregable demostrable:** con el reloj corriendo, los peces se mueven, rebotan en los bordes y a veces cambian de dirección; las plantas no se mueven.
**Depende de:** 2.
**Decisiones:** [Firme] movimiento aleatorio con rebote; buscar comida es extensión.
**Tareas:** paso de simulación por tick; redibujo.
**Criterio de aceptación:** fluido con ~150 entidades; ningún pez sale del área.
**Pruebas y evaluación:** MSTest: un pez en el borde invierte su dirección.
**Fuera de alcance:** energía y choques.
**Riesgos:** parpadeo → revisar el doble búfer (Fase 0).
**Para que lo entiendas tú:** posición nueva = posición + dirección × velocidad, y cómo el rebote invierte una componente.
**Punto de revisión:** ¿el intervalo del Timer da buena fluidez?

**Prompt para Claude Code:**
~~~
Lee CLAUDE.md y PLAN.md. Trabajamos solo la Fase 3.
Guíame para implementar lo que pide; revisa mi código contra el criterio de aceptación y actualiza PLAN.md.
~~~

---

## Fase 4: Energía, comer y morir  [ ]
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

**Prompt para Claude Code:**
~~~
Lee CLAUDE.md y PLAN.md. Trabajamos solo la Fase 4.
Guíame para implementar lo que pide y sus tests; revisa contra el criterio de aceptación y actualiza PLAN.md.
~~~

---

## Fase 5: Nacimientos  [ ]
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

**Prompt para Claude Code:**
~~~
Lee CLAUDE.md y PLAN.md. Trabajamos solo la Fase 5.
Guíame para implementar lo que pide y sus tests; revisa contra el criterio de aceptación y actualiza PLAN.md.
~~~

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

**Prompt para Claude Code:**
~~~
Lee CLAUDE.md y PLAN.md. Trabajamos solo la Fase 6.
Explícame los pasos en el diseñador y guíame con la lógica; revisa contra el criterio de aceptación y actualiza PLAN.md.
~~~

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

**Prompt para Claude Code:**
~~~
Lee CLAUDE.md y PLAN.md. Trabajamos solo la Fase 7.
Ayúdame a razonar el ajuste de parámetros (no los ajustes tú); verifica conmigo cada punto del criterio y actualiza PLAN.md.
~~~

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

## Riesgos globales
- Requisitos del profesor no confirmados → Fase 0.
- Colapso del ecosistema → parámetros en un solo lugar y Fase 7.
- Modificar la lista mientras se recorre → decidirlo en Fase 1.
- Sobrediseño → cada clase o capa debe evitar un coste real hoy.
