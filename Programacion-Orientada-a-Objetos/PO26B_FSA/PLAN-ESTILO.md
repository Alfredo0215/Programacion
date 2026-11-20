# Plan: migración de PO26B_FSA al estilo chat

Formato: planificar-fases v1 · módulos: software

## Estado actual
- Fase en curso: 0, Validación e inventario · Última actualización: 2026-11-19
- **Siguiente acción:**
  1. Guardar la imagen de referencia como `docs/referencia-estilo-chat.png`.
  2. Terminar la Fase 0: ejecutar la app tal como está y comprobar las fuentes (ver "Hecho cuando").
  3. Responder las preguntas abiertas que bloquean la Fase 5 (caja de mensaje y chips).

Leyenda: [ ] pendiente · [~] en curso · [h] hecho sin verificar · [x] verificado (fecha)
Decisiones: [Firme] ya decidida · [Validar en Fase N] supuesto a comprobar
Hechos externos: [volátil] dato que caduca: se anota con fuente y fecha
Modo de trabajo: `CLAUDE.md` de usuario y `CLAUDE.md` del proyecto (secciones "Interfaz y diseñador" y "Estilo chat"). Este plan no los copia.

**Hecho siempre incluye** (toda fase):
- Compila con el MSBuild de Visual Studio (comando en `CLAUDE.md`, sección "Compilar") y `dotnet test PO26B_FSA.Tests\PO26B_FSA.Tests.csproj` pasa.
- Prueba de regresión a mano: desde el menú se abre cada mesa con `AbrirMesa`, Esc y "Atrás" vuelven, y el logo sigue animándose.

**Reparto en cada fase:** Alfredo agrega y configura los controles en el diseñador de Visual Studio, con los pasos que Claude le explica. Claude propone la lógica de los `.cs` y espera "aplícalo". Una fase solo cambia el aspecto; el comportamiento del acuario lo decide `PLAN.md`.

## Resumen del enfoque
- **Decisión:** primero se cambian colores y fuentes en las pantallas que ya existen, sin mover controles. Después se rehace la distribución del menú. Al final, el lienzo del acuario. Cada fase deja la app usable.
- **Alternativa descartada:** rehacer `DlgPrincipal` desde cero con la distribución de chat en un solo paso. Mezcla demasiados cambios a la vez: si se rompe la navegación, no se sabe qué lo causó.
- **Qué observación la haría revisar:** si en la Fase 5 resulta que la distribución de chat obliga a rehacer los controles que se recolorearon en la Fase 1, ese trabajo de color se pierde. En ese caso, conviene adelantar la Fase 5.

## Mapa de fases
| Fase | Entregable demostrable | Escribe | Estado |
|---|---|---|---|
| 0 | Inventario de restos y entorno comprobado | Claude (inventario) + Alfredo (ejecutar app) | [~] |
| 1 | Menú y filas de mesa con paleta crema y Segoe UI, sin efectos Terraria | Alfredo en diseñador + Claude propone `.cs` | [ ] |
| 2 | Dificultad con punto de color, texto en `#2B2A27` | Alfredo en diseñador + Claude propone `.cs` | [ ] |
| 3 | Retratos como avatares circulares | Alfredo en diseñador + Claude propone `.cs` | [ ] |
| 4 | Barra lateral del menú con las 4 mesas | Alfredo en diseñador + Claude propone `.cs` | [ ] |
| 5 | Área principal: logo, saludo, caja de mensaje, chips, "Atrás" (esqueleto) | Alfredo en diseñador + Claude propone `.cs` | [ ] |
| 6 | Mesas 1-3 con paleta y fuentes | Alfredo en diseñador | [ ] |
| 7 | Marco del acuario en `MesaDePracticas4` con paleta y fuentes | Alfredo en diseñador | [ ] |
| 8 | Lienzo del acuario: fondo y colores de entidades (esqueleto) | Claude propone `.cs` | [ ] |
| 9 | Limpieza: sin código Terraria y `CLAUDE.md` sin la nota de restos | Claude propone `.cs` | [ ] |

---

## Fase 0: Validación e inventario  [~]
**Objetivo:** saber exactamente qué hay que cambiar y que el entorno sirve, antes de tocar nada.
**Entregable demostrable:** la tabla de inventario de abajo y la app ejecutándose tal como está.
**Escribe:** Claude (inventario con Grep, hecho el 2026-11-19); Alfredo (ejecutar la app y comprobar las fuentes).
**Decisiones:**
- [Validar en Fase 0] Georgia y Segoe UI están instaladas → si alguna falta, se elige otra fuente y se anota en el registro y en `CLAUDE.md`.
- [Validar en Fase 0] El diseñador acepta el color como "R, G, B" en la propiedad `BackColor` → si no, se usa la pestaña "Personalizado" del selector de color. No cambia el plan, solo los pasos.
- [Validar en Fase 0] Los colores `LightBlue` / `LightPink` de `MesaDePracticas1.cs` y los de `CDron.cs` muestran un estado (significan algo), no son decoración → si significan algo, se conservan o se cambian por colores equivalentes de la paleta (decisión en la Fase 6). Si son decoración, se sustituyen.
**Tareas:**
- [h] Grep de `AplicarEstiloTerraria`, `Andy`, `Color.FromArgb`, `Color.<Nombre>`, `Brushes.` y `Pens.` en los `.cs` de `PO26B_FSA\`.
- [ ] Ejecutar la app y recorrer las 5 pantallas (menú, mesas 1-4). Opcional: guardar una captura de cada una como "antes".
- [ ] En Visual Studio, propiedad `Font` de cualquier control: comprobar que aparecen Georgia y Segoe UI.
**Hecho cuando:** la tabla está completa, la app corre y los tres supuestos tienen su resultado anotado en Notas.
**Verificación:** manual (ejecutar la app, mirar la lista de fuentes); el inventario es una búsqueda automática.
**Fuera de alcance:** cambiar cualquier color o fuente.
**Riesgos:** un color fijo que el Grep no encuentra, por ejemplo una imagen de fondo en un `.resx` → aparecerá al recorrer las pantallas; se añade a la tabla.
**Qué debes entender:** por qué un color definido en el `.Designer.cs` se cambia desde el diseñador y uno definido en un `.cs` se cambia en código. Son dos sitios distintos con dos formas de editarlos.
**Punto de revisión:** ¿algún color significa un estado? ¿Alguna pantalla tiene más restos de los esperados? Si es así, se reajustan las Fases 6-8.
**Notas:**

### Inventario (Grep del 2026-11-19)

Hallazgo general: las mesas 1-4 no son pixel-art. Usan colores con nombre de WinForms (`Aquamarine`, `AliceBlue`, `SteelBlue`…). En ellas la migración es "de colores sueltos a la paleta", no "quitar Terraria". Lo que es Terraria de verdad está en el menú: `DlgPrincipal`, `FilaMesa` y `UIHelper`.

| Archivo | Pantalla | Qué hay | Se cambia en | Fase |
|---|---|---|---|---|
| `DlgPrincipal.Designer.cs` | Menú | Fuente Andy en el formulario y en `LblSelectMesas`, `LblMesas` | Diseñador | 1 |
| `DlgPrincipal.Designer.cs` | Menú | Fondos azules de `PnlMesas` (28,36,71), `PnlLabelMesas` (73,94,171) y `UcMesa1-4` (52,68,126); texto `Gainsboro` | Diseñador | 1 |
| `DlgPrincipal.Designer.cs` | Menú | `BackColor = Transparent` en `PbxLogoMenu`, `PnlContenedor`, `PnlMenu` | Revisar en Fase 1: sobre un fondo liso no se ve como transparencia | 1 |
| `DlgPrincipal.cs` | Menú | `UIHelper.AplicarEstiloTerraria(LblMesas)` y `(LblAtras)` | `.cs` | 1 |
| `DlgPrincipal.cs` | Menú | Borde negro de `PnlLabelMesas` en `RoundControl` | `.cs` | 1 |
| `DlgPrincipal.cs` | Menú | Dificultad por color de texto en `Configurar` (`Gainsboro`, `Gold`, `Crimson`, `MediumPurple`) | `.cs` | 2 |
| `FilaMesa.Designer.cs` | Menú (filas) | Fuente Andy en `LblNombre`, `LblMana`, `LblVida`, `LblTiempo`, `LblDificultad`; texto `Gainsboro` | Diseñador | 1 |
| `FilaMesa.cs` | Menú (filas) | `colorNormal`, `colorHover`, `colorPaneles` y bordes azules de `RoundControl` | `.cs` | 1 |
| `UIHelper.cs` | Menú | `AplicarEstiloTerraria`: contorno, sombra con transparencia y relleno que va de `Gainsboro` a `Yellow` (brillo) | Se elimina | 9 |
| `UIHelper.cs` | Varias | `RoundControl`: bordes redondeados. Compatible con el diseño plano; solo cambian los colores que recibe | Se conserva | — |
| `MesaDePracticas1-3.Designer.cs` | Mesas 1-3 | `PanelSide` Aquamarine, `PnlFill` AliceBlue, `PnlPracticas1-4` SteelBlue/Lavender/Indigo/YellowGreen, `LblNumPersonas` Gainsboro | Diseñador | 6 |
| `MesaDePracticas1.cs` | Mesa 1 | `LightBlue` / `LightPink` (supuesto: color por tipo de persona) | `.cs` | 6 |
| `CDron.cs` | Mesa 2 | `BackColor` del dron: `FromArgb(0,G,B)`, `Red`, `Black` (supuesto: indica su estado) | `.cs` | 6 |
| `MesaDePracticas4.Designer.cs` | Mesa 4 / acuario | `PanelSide` Aquamarine, `PnlFill` AliceBlue, `PnlProyectoF` Lavender, `PnlProyectoFIA` YellowGreen | Diseñador | 7 |
| `MesaDePracticas4.Designer.cs` | Acuario | `PbxAcuario` LightSteelBlue (fondo del lienzo) | Diseñador | 8 |
| `Acuario\CHerbivoro.cs`, `CDepredador.cs`, `CPlanta.cs` | Acuario | `Brushes.Gold`, `Brushes.IndianRed`, `Brushes.ForestGreen` en el dibujo de cada entidad | `.cs` | 8 |

---

## Fase 1: Paleta y fuentes del menú  [ ]
**Objetivo:** que el menú actual se vea con la paleta crema y Segoe UI, sin mover ningún control.
**Entregable demostrable:** el menú y las filas de mesa con fondo `#F5F4EE`, paneles `#EDEBE3`, bordes `#DDD9CE`, texto `#2B2A27` / `#7A776E`, sin azules, sin Andy y sin contorno ni brillo en `LblMesas` y `LblAtras`.
**Escribe:** Alfredo en el diseñador (colores y fuentes del `.Designer.cs`); Claude propone los cambios de `DlgPrincipal.cs` y `FilaMesa.cs`.
**Decisiones:**
- [Firme] No se toca la distribución: eso es de las Fases 4-5.
- [Firme] El hover de `FilaMesa` se conserva como comportamiento; solo cambian sus colores.
- [Validar en Fase 1] ¿Dónde viven los colores de la paleta en código? → Si la misma constante se necesita en dos o más `.cs`, se ponen en un solo lugar (Claude propone dónde, con justificación). Si cada color se usa en un solo archivo, quedan como constantes locales, igual que hoy en `FilaMesa.cs`.
- [Validar en Fase 1] El color de hover de las filas → propuesta inicial: `#DDD9CE` sobre `#EDEBE3`. Si no se distingue al pasar el ratón, se prueba con un borde `#C96442`.
**Tareas:**
- Diseñador, `DlgPrincipal`: `BackColor` y `Font` del formulario, `PnlMesas`, `PnlLabelMesas`, `UcMesa1-4`, `LblSelectMesas`, `LblMesas`.
- Diseñador, `FilaMesa`: `Font` y `ForeColor` de sus etiquetas.
- `DlgPrincipal.cs`: quitar las dos llamadas a `AplicarEstiloTerraria` y cambiar el color del borde en `RoundControl`.
- `FilaMesa.cs`: `colorNormal`, `colorHover`, `colorPaneles` y los bordes de `RoundControl`.
**Hecho cuando:** al ejecutar la app no queda ningún azul ni ninguna letra pixelada en el menú, el hover de las filas se nota, y pasa la prueba de regresión de la cabecera.
**Verificación:** manual en la app; no hay prueba automática porque es aspecto visual. Compilación y tests como en la cabecera.
**Fuera de alcance:** la dificultad (Fase 2), los retratos (Fase 3), borrar `AplicarEstiloTerraria` de `UIHelper` (Fase 9).
**Riesgos:** quitar `AplicarEstiloTerraria` podría cambiar el tamaño o la posición del texto si el método también ajusta algo de eso → comparar con la captura "antes" y, si pasa, revisar el método antes de seguir.
**Qué debes entender:** la diferencia entre `BackColor`, `ForeColor` y la herencia de `Font`. Un control sin fuente propia usa la de su contenedor: por eso basta cambiar la fuente del formulario para que cambien muchos controles a la vez.
**Punto de revisión:** ¿hizo falta un lugar común para los colores? Si sí, las Fases 2, 6 y 8 lo usan.
**Notas:**

---

## Fase 2: Dificultad con punto de color  [ ]
**Objetivo:** que la dificultad se lea por un punto o una etiqueta de color y no por el color del texto.
**Entregable demostrable:** las 4 filas muestran el nombre de la dificultad en `#2B2A27` con un punto de color al lado.
**Escribe:** Alfredo en el diseñador (si hace falta un control para el punto); Claude propone el cambio en `Configurar` y en `FilaMesa.cs`.
**Decisiones:**
- [Firme] El texto de la dificultad siempre en `#2B2A27`.
- [Validar en Fase 2] Los 4 colores del punto (Clásico, Experto, Maestro, Legendario) → se proponen en esta fase. Criterio fijado de antemano: los 4 se distinguen entre sí sobre `#EDEBE3` a la distancia normal de la pantalla. Si no, se cambia el punto por una etiqueta con fondo de color y texto.
- [Validar en Fase 2] Punto dibujado en el `Paint` de un panel o control nuevo desde el diseñador → Claude propone uno con justificación.
**Tareas:** `DlgPrincipal.cs` (las llamadas a `Configurar`), `FilaMesa.cs` (cómo se aplica el color recibido) y, si hace falta, un control nuevo en el diseñador de `FilaMesa`.
**Hecho cuando:** ningún texto de dificultad cambia de color; cada fila muestra un punto con su color; pasa la prueba de regresión.
**Verificación:** manual en la app.
**Fuera de alcance:** cambiar los nombres de las dificultades o sus datos.
**Riesgos:** `Configurar` recibe el color como parámetro; cambiar su uso puede afectar a las 4 llamadas → se revisan juntas.
**Qué debes entender:** por qué el texto de color falla sobre fondo claro (poco contraste entre el color del texto y el fondo) y por qué un punto no tiene ese problema: no hace falta leerlo, solo distinguirlo.
**Punto de revisión:** ¿los 4 colores se distinguen? Si no, se pasa a etiquetas y se anota en el registro.
**Notas:**

---

## Fase 3: Avatares circulares  [ ]
**Objetivo:** cambiar los retratos pixel-art de las filas por avatares circulares simples.
**Entregable demostrable:** las 4 filas muestran un avatar circular en lugar del retrato.
**Escribe:** Alfredo en el diseñador; Claude propone el dibujo del círculo en `FilaMesa.cs`.
**Decisiones:**
- [Validar en Fase 3] Contenido del avatar: círculo de color con la inicial o el número de la mesa, o la imagen actual recortada en círculo → criterio: si la imagen pixel-art recortada sigue viéndose pixelada, se usa el círculo con inicial.
**Tareas:** `FilaMesa.cs` (dibujo del avatar), `DlgPrincipal.cs` (qué se pasa a `Configurar` en lugar de `retrato1-4`), `PnlRetrato` en el diseñador si cambia su tamaño.
**Hecho cuando:** ninguna fila muestra pixel-art y los 4 avatares son círculos del mismo tamaño; pasa la prueba de regresión.
**Verificación:** manual en la app.
**Fuera de alcance:** borrar las imágenes `retrato1-4` de los recursos (Fase 9, si ya no se usan).
**Riesgos:** un círculo dibujado sin suavizado se ve con bordes dentados → se activa el suavizado al dibujar.
**Qué debes entender:** cómo se dibuja una figura en el `Paint` de un control y qué cambia el suavizado (antialiasing) en los bordes.
**Punto de revisión:** ¿el avatar sirve igual en la barra lateral de la Fase 4?
**Notas:**

---

## Fase 4: Barra lateral del menú  [ ]
**Objetivo:** que las 4 mesas aparezcan en una barra lateral izquierda, como una lista de conversaciones.
**Entregable demostrable:** el menú tiene a la izquierda una barra `#EDEBE3` con las 4 mesas, y un clic en cada una la abre con `AbrirMesa`.
**Escribe:** Alfredo en el diseñador (barra y acoplamiento); Claude propone la lógica que haga falta.
**Decisiones:**
- [Firme] Se reutilizan las filas `FilaMesa` y `AbrirMesa`; abrir una mesa no cambia.
- [Validar en Fase 4] ¿La distribución de chat junta en una sola vista lo que hoy son el menú principal y el selector de mesas? → se responde al empezar la fase, leyendo `DlgPrincipal.cs`. Si las junta, se decide qué hace "Atrás" en la vista única y se reajusta la Fase 5.
**Tareas:** diseñador de `DlgPrincipal` (panel lateral con `Dock = Left`, filas dentro); `DlgPrincipal.cs` solo si cambia cómo se muestran las vistas.
**Hecho cuando:** las 4 mesas se ven a la izquierda y cada clic abre la mesa correcta; pasa la prueba de regresión.
**Verificación:** manual en la app.
**Fuera de alcance:** saludo, caja de mensaje y chips (Fase 5).
**Riesgos:** controles acoplados que se tapan → regla de `Dock = Fill` del `CLAUDE.md`.
**Qué debes entender:** cómo reparte el espacio `Dock` entre controles hermanos y por qué importa el orden de `Controls.Add`.
**Punto de revisión:** ¿cómo quedó la navegación? Eso fija el contenido de la Fase 5.
**Notas:**

---

## Fase 5: Área principal del menú  [ ]  (esqueleto: depende de [Validar en Fase 4] y de las preguntas abiertas)
**Objetivo:** logo "PO26B", saludo "¿Qué practicamos hoy?" en Georgia, caja de mensaje, chips de sugerencia y botón "Atrás". · **Entregable demostrable:** el menú se parece a la referencia en la distribución, con los valores de la paleta. · **Hecho cuando:** se ven los cinco elementos, el logo se anima con `Timer` + `Stopwatch`, "Atrás" y Esc vuelven, y pasa la prueba de regresión.

---

## Fase 6: Mesas 1-3  [ ]
**Objetivo:** aplicar el lenguaje visual (paleta, Segoe UI, diseño plano) a las mesas 1-3, sin cambiar su distribución.
**Entregable demostrable:** las tres mesas sin colores sueltos de WinForms (`Aquamarine`, `AliceBlue`, `SteelBlue`…).
**Escribe:** Alfredo en el diseñador; Claude propone los cambios en `MesaDePracticas1.cs` y `CDron.cs` si la Fase 0 confirma que esos colores son decoración.
**Decisiones:**
- [Firme] Las mesas no copian la distribución de chat (`CLAUDE.md`).
- [Validar en Fase 0] Colores que significan un estado (`LightBlue`/`LightPink`, colores del dron) → si significan algo, se conserva la distinción con colores que se lean sobre la paleta. No se quita información.
- [Validar en Fase 6] Qué color de la paleta lleva cada `PnlPracticasN` → hoy cada panel tiene un color distinto. Si el color solo decora, todos pasan a `#EDEBE3`; si sirve para distinguir prácticas, se usa el acento `#C96442` en el elemento seleccionado.
**Tareas:** diseñador de `MesaDePracticas1`, `2` y `3` (`PanelSide`, `PnlFill`, `PnlPracticas1-4`, `LblNumPersonas`, fuentes); `MesaDePracticas1.cs`, `CDron.cs`.
**Hecho cuando:** al abrir cada una de las tres mesas no queda ningún color fuera de la paleta (salvo los de estado decididos en la Fase 0) y sus prácticas funcionan igual que antes; pasa la prueba de regresión.
**Verificación:** manual en la app, mesa por mesa.
**Fuera de alcance:** cambiar cómo funciona cualquier práctica.
**Riesgos:** si una mesa da problemas, la fase se divide (6a, 6b, 6c) con un bloque de cambio de plan.
**Qué debes entender:** la diferencia entre un color que decora y uno que lleva información, y por qué el segundo no se puede quitar sin sustituirlo.
**Punto de revisión:** ¿los valores elegidos para los paneles sirven también para el marco del acuario?
**Notas:**

---

## Fase 7: Marco del acuario  [ ]
**Objetivo:** aplicar la paleta y las fuentes a `MesaDePracticas4`, sin tocar el lienzo.
**Entregable demostrable:** `MesaDePracticas4` con paneles, botones y etiquetas en la paleta; el acuario se ve y funciona igual por dentro.
**Escribe:** Alfredo en el diseñador.
**Decisiones:**
- [Firme] `PbxAcuario` y los colores de las entidades no se tocan aquí (Fase 8).
- [Firme] No se cambia nada del comportamiento del acuario; eso es de `PLAN.md`.
**Tareas:** diseñador de `MesaDePracticas4` (`PanelSide`, `PnlFill`, `PnlProyectoF`, `PnlProyectoFIA`, botones, etiquetas, fuentes).
**Hecho cuando:** el marco no tiene colores fuera de la paleta, `AlternarPractica` sigue abriendo el acuario, la simulación corre con `TimerAcuario` y los tests pasan.
**Verificación:** manual en la app; `dotnet test` como en la cabecera.
**Fuera de alcance:** `PbxAcuario`, `CHerbivoro`, `CDepredador`, `CPlanta`.
**Riesgos:** una fase de `PLAN.md` en curso edita `MesaDePracticas4` al mismo tiempo → se termina primero esa fase y luego esta.
**Qué debes entender:** por qué separar el marco del lienzo permite cambiar el aspecto sin arriesgar la simulación.
**Punto de revisión:** con el marco ya en crema, ¿el fondo actual del lienzo (`LightSteelBlue`) se ve fuera de lugar? Esa observación alimenta la Fase 8.
**Notas:**

---

## Fase 8: Lienzo del acuario  [ ]  (esqueleto: depende de [Validar en Fase 8])
**Objetivo:** decidir el fondo de `PbxAcuario` y los colores de peces y plantas. · **Entregable demostrable:** el acuario con colores coherentes con la paleta. · **Hecho cuando:** con la simulación corriendo, se distinguen a simple vista herbívoros, depredadores y plantas sobre el fondo elegido, y los tests pasan.

[Validar en Fase 8] Contraste sobre `#F5F4EE`. Criterio fijado de antemano: si los tres tipos de entidad no se distinguen a simple vista, la paleta recibe colores de entidad nuevos (se anotan en `CLAUDE.md`) o el lienzo usa un fondo propio. Los colores viven en `CHerbivoro.cs`, `CDepredador.cs` y `CPlanta.cs`: cambiar el pincel es aspecto, pero esos archivos son del proyecto final. Esta fase no coincide con una fase de `PLAN.md` que los edite.

---

## Fase 9: Limpieza  [ ]
**Objetivo:** quitar el código Terraria que ya no se usa y la nota de restos del `CLAUDE.md`.
**Entregable demostrable:** el proyecto sin `AplicarEstiloTerraria`, sin la fuente Andy y con el `CLAUDE.md` sin la viñeta "Restos de Terraria".
**Escribe:** Claude propone; Alfredo aplica o dice "aplícalo".
**Decisiones:** [Firme] Solo se borra lo que el Grep confirma sin uso.
**Tareas:** `UIHelper.cs` (borrar `AplicarEstiloTerraria` y `Interpolar` si nadie más lo usa); recursos `retrato1-4` si la Fase 3 los dejó sin uso; `CLAUDE.md`.
**Hecho cuando:** un Grep de `AplicarEstiloTerraria` y de `"Andy"` en los `.cs` no devuelve nada, y pasa la cabecera.
**Verificación:** Grep (automático) y la prueba de regresión (manual).
**Fuera de alcance:** refactorizar `UIHelper` más allá de borrar lo que no se usa.
**Riesgos:** borrar un recurso todavía referenciado rompe la compilación → la compilación de la cabecera lo detecta.
**Qué debes entender:** por qué se borra al final y no al principio: mientras quedaba un uso, borrar el método rompía la compilación.
**Punto de revisión:** fin de la migración.
**Notas:**

---

## Prompt para cada fase
~~~
Lee CLAUDE.md y PLAN-ESTILO.md. Trabajamos solo la Fase N.
1. Lista los controles que yo debo cambiar en el diseñador, con los pasos en Visual Studio.
2. Para la lógica en .cs, propón el cambio con la estructura de 5 partes y espera mi "aplícalo".
3. Comprueba "Hecho cuando" y la cabecera (compilar, tests, regresión). Al terminar, /cerrar-sesion.
No cambies comportamiento: si hace falta, detente y consulta.
~~~

## Después del MVP
- Tema oscuro con la misma estructura de paleta.
- Que la caja de mensaje haga algo (por ejemplo, buscar una mesa por nombre): es comportamiento nuevo y va en su propio plan.

## Preguntas abiertas
| Pregunta | Supuesto mientras tanto | Cuándo se resuelve |
|---|---|---|
| ¿La caja de mensaje acepta texto o es decorativa? | Decorativa: se ve, pero no hace nada (no hay comportamiento nuevo) | Antes de la Fase 5 |
| ¿Qué hacen los chips de sugerencia? | Cada chip abre una mesa con `AbrirMesa` (lógica que ya existe) | Antes de la Fase 5 |
| ¿El menú y el selector de mesas pasan a ser una sola vista? | No se sabe sin leer `DlgPrincipal.cs` | Inicio de la Fase 4 |

## Registro de decisiones
| Fecha | Fase | Decisión | Alternativa descartada | Motivo |
|---|---|---|---|---|
| 2026-11-19 | — | El estilo chat se aplica a todo el proyecto: menú, mesas 1-3 y `MesaDePracticas4`, acuario incluido | Solo el menú y las pantallas nuevas | Un proyecto con dos estilos se ve a medio hacer; el aspecto no toca las clases `C*` |
| 2026-11-19 | — | Lenguaje visual (paleta, fuentes, plano, dificultad, avatares) en todas las pantallas; distribución de chat solo en `DlgPrincipal` | Aplicar la distribución de chat también a las mesas | Una barra lateral y unos chips no tienen sentido dentro de una mesa de prácticas |
| 2026-11-19 | — | El plan se llama `PLAN-ESTILO.md`, separado de `PLAN.md` | `PLAN-MENU.md`, o una sección en `PLAN.md` | El alcance ya no es solo el menú; `PLAN.md` sigue siendo solo del comportamiento del acuario |
| 2026-11-19 | — | La imagen de referencia es inspiración, no especificación; si choca con los valores del `CLAUDE.md`, mandan los valores | Tomar medidas y colores de la imagen | La imagen generada no tiene valores exactos |
| 2026-11-19 | — | En el acuario, el marco (Fase 7) va antes que el lienzo (Fase 8) | Migrar todo el acuario en una fase | Hay riesgo de contraste sobre `#F5F4EE`, y separar protege la simulación |
| 2026-11-19 | — | Ninguna fase mezcla aspecto y comportamiento | Ajustar comportamiento cuando convenga durante la migración | Si algo se rompe, se sabe de qué plan vino |

## Riesgos globales
- **Dos planes sobre `MesaDePracticas4`:** no se ejecutan a la vez una fase de `PLAN.md` y la Fase 7 u 8 de este plan.
- **Romper la navegación al rehacer el menú:** la prueba de regresión de la cabecera se hace en cada fase, no solo al final.
- **Que el color se vuelva a escribir en muchos sitios:** se resuelve con la decisión de la Fase 1 sobre dónde viven las constantes.
