# Plan: migración de PO26B_FSA al estilo chat

Formato: planificar-fases v1 · módulos: software

## Estado actual
- Fase en curso: 6, Mesas 1-3 · Última actualización: 2026-11-22
- **Siguiente acción:** hacer commit de la Fase 5 y empezar la Fase 6 (primero resolver la pregunta abierta de los colores de estado de Mesa 1 y del dron).
- Decidir si `docs/Screenshot_4.png` (aparece borrado en el árbol de trabajo; lo cita la Fase 0) se restaura con `git restore`.

Leyenda: [ ] pendiente · [~] en curso · [h] hecho sin verificar · [x] verificado (fecha)
Decisiones: [Firme] ya decidida · [Validar en Fase N] supuesto a comprobar
Hechos externos: [volátil] dato que caduca: se anota con fuente y fecha
Modo de trabajo: `CLAUDE.md` de usuario y `CLAUDE.md` del proyecto (secciones "Interfaz y diseñador" y "Estilo chat"). Este plan no los copia.

**Hecho siempre incluye** (toda fase):
- Compila con el MSBuild de Visual Studio (comando en `CLAUDE.md`, sección "Compilar") y `dotnet test PO26B_FSA.Tests\PO26B_FSA.Tests.csproj` pasa.
- Prueba de regresión a mano: desde el menú se abre cada mesa con `AbrirMesa`; al cerrar la mesa vuelve el menú; la X cierra la app; Esc no hace nada en el menú.

**Reparto en cada fase:** Alfredo agrega y configura los controles en el diseñador de Visual Studio, con los pasos que Claude le explica. Claude propone la lógica de los `.cs` y espera "aplícalo". Una fase solo cambia el aspecto, salvo la Fase 1 (navegación del menú) y la Fase 5 (logo sin animación); ver registro. El comportamiento del acuario lo decide `PLAN.md`.

## Resumen del enfoque
- **Decisión:** primero se junta el menú en una sola vista sin tocar su aspecto. Después se rehace la distribución del menú con la paleta. Luego las mesas y, al final, el lienzo del acuario. Cada fase deja la app usable.
- **Alternativa descartada:** rehacer `DlgPrincipal` desde cero con la distribución de chat en un solo paso. Mezcla demasiados cambios a la vez: si se rompe la navegación, no se sabe qué lo causó.
- **Qué observación la haría revisar:** si en la Fase 4 la barra lateral no deja espacio suficiente para el área principal de la Fase 5, se revisa el ancho de la barra antes de seguir.

> **Cambio de plan (2026-11-19):** el menú se simplifica a una sola vista estilo chat. Se descartan la dificultad, los retratos y las estadísticas (maná, vida, tiempo); sin ellos `FilaMesa` queda en nombre + hover + clic, que un `Button` plano ya trae, y se elimina. La Fase 1 pasa de "recolorear el menú" a "vista única": recolorear controles que la Fase 4 quita era trabajo perdido. Las Fases 2 y 3 se descartan. Las Fases 4 y 5 se rehacen. Todos los recursos de Terraria se borran en la Fase 9.

## Mapa de fases
| Fase | Entregable demostrable | Escribe | Estado |
|---|---|---|---|
| 0 | Inventario de restos y entorno comprobado | Claude (inventario) + Alfredo (ejecutar app) | [x] (2026-11-19) |
| 1 | Vista única: la app abre directamente con las 4 mesas; sin "Mesas", "Atrás" ni Esc | Alfredo en diseñador + Claude propone `.cs` | [x] (2026-11-22) |
| 2 | ~~Dificultad con punto de color~~ | — | descartada (2026-11-19) |
| 3 | ~~Avatares circulares~~ | — | descartada (2026-11-19) |
| 4 | Barra lateral con 4 `Button` planos en lugar de `FilaMesa`; paleta y Segoe UI en el menú; sin fondo Terraria | Alfredo en diseñador + Claude propone `.cs` | [x] (2026-11-22) |
| 5 | Área principal: logo "POO26B" en texto, saludo, caja de mensaje y chips que abren mesas | Alfredo en diseñador + Claude propone `.cs` | [x] (2026-11-22) |
| 6 | Mesas 1-3 con paleta y fuentes | Alfredo en diseñador | [ ] |
| 7 | Marco del acuario en `MesaDePracticas4` con paleta y fuentes | Alfredo en diseñador | [ ] |
| 8 | Lienzo del acuario: fondo y colores de entidades (esqueleto) | Claude propone `.cs` | [ ] |
| 9 | Limpieza: sin `FilaMesa`, sin código ni recursos Terraria, `CLAUDE.md` sin la nota de restos | Claude propone | [ ] |

---

## Fase 0: Validación e inventario  [x] (2026-11-19)
**Objetivo:** saber exactamente qué hay que cambiar y que el entorno sirve, antes de tocar nada.
**Entregable demostrable:** la tabla de inventario de abajo y la app ejecutándose tal como está.
**Escribe:** Claude (inventario con Grep, hecho el 2026-11-19); Alfredo (ejecutar la app y comprobar las fuentes).
**Decisiones:**
- [Validado 2026-11-19] Georgia y Segoe UI están instaladas (ver Notas): no hace falta otra fuente.
- [Validado 2026-11-19] El diseñador acepta el color escrito en la propiedad `BackColor` con punto y coma: `245; 244; 238` (no con comas). No cambia el plan, solo los pasos.
- [Validado 2026-11-19] Los colores `LightBlue` / `LightPink` de `MesaDePracticas1.cs` y los de `CDron.cs` **sí** significan un estado (ver Notas); la Fase 6 elige los colores de la paleta que los sustituyen.
**Tareas:**
- [x] (2026-11-19) Grep de `AplicarEstiloTerraria`, `Andy`, `Color.FromArgb`, `Color.<Nombre>`, `Brushes.` y `Pens.` en los `.cs` de `PO26B_FSA\`. Verificado al recorrer las pantallas: no aparecen restos fuera del inventario.
- [x] (2026-11-19) Grep de recursos (`Properties.Resources.*`, `Resources.resx`, `.csproj`) y de FontAwesome.Sharp (no está referenciado; `packages.config` solo tiene `Bogus`).
- [x] (2026-11-19) Ejecutar la app y recorrer las 5 pantallas (menú, mesas 1-4). Menú: `docs/Screenshot_4.png`, `docs/Screenshot_5.png`. Mesas 1-4: sin imágenes salvo `Dron.png` (ya inventariada).
- [x] (2026-11-19) Comprobar que Georgia y Segoe UI están instaladas.
- [x] (2026-11-19) Mirar el ícono de la ventana (`$this.Icon`): si es de Terraria, se añade a la Fase 9 y se elige otro.
**Hecho cuando:** la tabla está completa, la app corre y los tres supuestos y el ícono tienen su resultado anotado en Notas.
**Verificación:** manual (ejecutar la app, mirar la lista de fuentes); el inventario es una búsqueda automática.
**Fuera de alcance:** cambiar cualquier color o fuente.
**Riesgos:** un color fijo que el Grep no encuentra, por ejemplo una imagen de fondo en un `.resx` → aparecerá al recorrer las pantallas; se añade a la tabla.
**Qué debes entender:** por qué un color definido en el `.Designer.cs` se cambia desde el diseñador y uno definido en un `.cs` se cambia en código. Son dos sitios distintos con dos formas de editarlos.
**Punto de revisión:** ¿algún color significa un estado? ¿Alguna pantalla tiene más restos de los esperados? Si es así, se reajustan las Fases 6-8.
**Notas:**
- 2026-11-19 · Fuentes: Georgia (4 variantes) y Segoe UI (incluye Semibold y Light) están instaladas (registro de fuentes de Windows).
- 2026-11-19 · Color en el diseñador: se escribe `245; 244; 238`. El separador es `;` probablemente por la configuración regional en español (su separador de listas es `;`).
- 2026-11-19 · Ícono: `$this.Icon` de `DlgPrincipal.resx` es el árbol pixel-art de Terraria → se elimina en la Fase 9. Las mesas no tienen ícono propio (usan el de WinForms).
- 2026-11-19 · Mesa 1: **sí** significan algo. `LightBlue` = `CHombre`, `LightPink` = `CMujer` (`MesaDePracticas1.cs:252`, `:260`).
- 2026-11-19 · Dron: **sí** indican estado. `FromArgb(0,G,B)` al azar = recién creado, un color distinto por dron (`CDron.cs:93`); `Red` = encendido (`:115`); `Black` = se le dio clic (`:181`).
- 2026-11-19 · Punto de revisión: la paleta tiene un solo acento y la Fase 6 necesita 2 colores distinguibles (Mesa 1) y 3 estados (dron, uno de ellos al azar). Ver "Preguntas abiertas".
- 2026-11-19 · Compila con MSBuild y los 36 tests pasan.
- 2026-11-19 · Capturas del menú (portada y selector): no aparecen restos fuera del inventario.

### Inventario (Grep del 2026-11-19)

Hallazgo general: las mesas 1-4 no son pixel-art. Usan colores con nombre de WinForms (`Aquamarine`, `AliceBlue`, `SteelBlue`…). En ellas la migración es "de colores sueltos a la paleta", no "quitar Terraria". Lo que es Terraria de verdad está en el menú: `DlgPrincipal`, `FilaMesa` y `UIHelper`.

| Archivo | Pantalla | Qué hay | Se cambia en | Fase |
|---|---|---|---|---|
| `DlgPrincipal.Designer.cs` | Menú | Fuente Andy en el formulario, `LblSelectMesas`, `LblMesas` (Andy está instalada en Windows, no es un archivo del proyecto) | Diseñador | 4 |
| `DlgPrincipal.Designer.cs` | Menú | `PnlBackground.BackgroundImage = TerrariaBackground` | Diseñador | 4 |
| `DlgPrincipal.Designer.cs` | Menú | Fondos azules de `PnlMesas`, `PnlLabelMesas`, `UcMesa1-4`; texto `Gainsboro` | Se quitan con los controles | 1 y 4 |
| `DlgPrincipal.Designer.cs` | Menú | `BackColor = Transparent` en `PbxLogoMenu`, `PnlContenedor`, `PnlMenu` | Revisar en Fase 4: sobre un fondo liso no se ve como transparencia | 4 |
| `DlgPrincipal.Designer.cs` | Menú | `PbxLogoMenu.Image = LogoPOO1` | Diseñador / `.cs` | 5 |
| `DlgPrincipal.resx` | Menú | `$this.Icon`: árbol pixel-art de Terraria | Diseñador (propiedad `Icon` del formulario) | 9 |
| `DlgPrincipal.cs` | Menú | `AplicarEstiloTerraria(LblMesas/LblAtras)`, `LblMesas_Click`, `VolverAlMenu`, `DlgPrincipal_KeyDown` | `.cs` | 1 |
| `DlgPrincipal.cs` | Menú | `Configurar(...)` con dificultad, `Personaje1-4`, vida, maná, tiempo; borde negro de `PnlLabelMesas` | `.cs` | 4 |
| `FilaMesa.*` | Menú | Control completo (colores fijos, Andy, `EstrellaMana`, `CorazonVida`) | Se elimina | 4 (deja de usarse), 9 (se borra) |
| `UIHelper.cs` | — | `AplicarEstiloTerraria` e `Interpolar` (solo lo usa ese método, `UIHelper.cs:59`) | Se eliminan | 9 |
| `UIHelper.cs` | Varias | `RoundControl`: bordes redondeados. Compatible con el diseño plano; solo cambian los colores que recibe | Se conserva | — |
| `Resources\` | — | `TerrariaBackground`, `Personaje1-4`, `CorazonVida`, `EstrellaMana`, `LogoPOO1`; sin uso: `LogoPOO`, `BotonJugar`, `PlayButton`; huérfano (sin entrada en `.csproj` ni en `Resources.resx`): `corazoonvida.png` | Se eliminan | 9 |
| `Resources\Dron.png` | Mesa 2 | No es de Terraria | Se conserva | — |
| `MesaDePracticas1-3.Designer.cs` | Mesas 1-3 | `PanelSide` Aquamarine, `PnlFill` AliceBlue, `PnlPracticas1-4` SteelBlue/Lavender/Indigo/YellowGreen, `LblNumPersonas` Gainsboro | Diseñador | 6 |
| `MesaDePracticas1.cs` | Mesa 1 | `LightBlue` / `LightPink`: color por tipo de persona (confirmado) | `.cs` | 6 |
| `CDron.cs` | Mesa 2 | `BackColor` del dron: `FromArgb(0,G,B)` creado, `Red` encendido, `Black` clic (confirmado: indica su estado) | `.cs` | 6 |
| `MesaDePracticas4.Designer.cs` | Mesa 4 / acuario | `PanelSide` Aquamarine, `PnlFill` AliceBlue, `PnlProyectoF` Lavender, `PnlProyectoFIA` YellowGreen | Diseñador | 7 |
| `MesaDePracticas4.Designer.cs` | Acuario | `PbxAcuario` LightSteelBlue (fondo del lienzo) | Diseñador | 8 |
| `Acuario\CHerbivoro.cs`, `CDepredador.cs`, `CPlanta.cs` | Acuario | `Brushes.Gold`, `Brushes.IndianRed`, `Brushes.ForestGreen` en el dibujo de cada entidad | `.cs` | 8 |

---

## Fase 1: Vista única  [x] (2026-11-22)
**Objetivo:** que el menú principal y el selector de mesas sean una sola vista, sin cambiar todavía el aspecto.
**Entregable demostrable:** al abrir la app se ven directamente las 4 mesas; no existen "Mesas", "Atrás" ni Esc.
**Escribe:** Alfredo en el diseñador (borrar `PnlMenu`, `LblMesas`, `LblAtras`; `PnlMesas` y `PnlLabelMesas` visibles al inicio); Claude propone los cambios de `DlgPrincipal.cs`.
**Decisiones:**
- [Firme] Es la única fase que cambia comportamiento, y solo la navegación de `DlgPrincipal` (registro 2026-11-19).
- [Firme] Esc deja de tener función en el menú: no cierra la app. La app se cierra con la X; volver de una mesa sigue siendo cerrarla.
**Tareas:** diseñador de `DlgPrincipal`; `DlgPrincipal.cs`: quitar `LblMesas_Click`, `LblAtras_Click`, `VolverAlMenu`, `DlgPrincipal_KeyDown`, `KeyPreview` y las dos llamadas a `AplicarEstiloTerraria`.
**Hecho cuando:** la app abre mostrando las 4 mesas, cada clic abre la mesa correcta, al cerrarla vuelve el menú, la X cierra la app y no queda ningún método sin uso de la navegación anterior.
**Verificación:** manual en la app; compilación y tests como en la cabecera.
**Fuera de alcance:** colores, fuentes, barra lateral.
**Riesgos:** borrar un control en el diseñador deja su handler en el `.cs` → se borran juntos; la compilación lo detecta si queda una referencia.
**Qué debes entender:** por qué borrar un control en el diseñador no borra su método `_Click` del `.cs`, pero sí la línea `+=` del `.Designer.cs`.
**Punto de revisión:** ¿se nota que falta algo sin "Atrás"? Si sí, se anota antes de la Fase 4.
**Notas:**
- 2026-11-22 · Diseñador (Alfredo): borrados `LblAtras`, `PnlMenu` (con `LblMesas` dentro); `PnlMesas` y `PnlLabelMesas` en `Visible = True`. Comprobación en el `.Designer.cs`: ya no tienen la línea `Visible = false` (el diseñador no escribe el valor por defecto).
- 2026-11-22 · `DlgPrincipal.cs` (Claude): quitados `AplicarEstiloTerraria(LblMesas/LblAtras)`, `KeyPreview`, la suscripción a `KeyDown`, `LblMesas_Click`, `DlgPrincipal_KeyDown`, `LblAtras_Click` y `VolverAlMenu`.
- 2026-11-22 · Compila; 36/36 tests; Grep de `LblMesas`, `LblAtras`, `PnlMenu`, `VolverAlMenu`, `KeyDown`, `KeyPreview` en los `.cs`: sin resultados. Prueba de regresión manual correcta.
- 2026-11-22 · `UIHelper.AplicarEstiloTerraria` queda sin usos: se borra en la Fase 9.
- 2026-11-22 · Punto de revisión: no se nota que falte nada sin "Atrás"; según Alfredo, `LblAtras` ni siquiera se veía antes.

---

## Fase 2: Dificultad con punto de color  [descartada (2026-11-19)]
Motivo: la dificultad no encaja en una lista de conversaciones (registro 2026-11-19).

---

## Fase 3: Avatares circulares  [descartada (2026-11-19)]
Motivo: los retratos no encajan en una lista de conversaciones (registro 2026-11-19).

---

## Fase 4: Barra lateral y paleta del menú  [x] (2026-11-22)
**Objetivo:** las 4 mesas en una barra lateral izquierda como lista de conversaciones, con la paleta y Segoe UI.
**Entregable demostrable:** barra `#EDEBE3` a la izquierda con 4 botones planos "Mesa de prácticas 1-4"; fondo `#F5F4EE`; sin azules, sin Andy y sin fondo Terraria.
**Escribe:** Alfredo en el diseñador; Claude propone `DlgPrincipal.cs`.
**Decisiones:**
- [Firme] `FilaMesa` se elimina: sin dificultad, retrato ni estadísticas solo le queda nombre + hover + clic, y un `Button` con `FlatStyle = Flat` ya trae `Text`, `FlatAppearance.MouseOverBackColor` y `Click`. Aquí deja de usarse; los archivos se borran en la Fase 9.
- [Firme] `Button` de WinForms, no `IconButton` de FontAwesome.Sharp: no está en el proyecto y los botones son solo texto.
- [Firme] Cada botón llama a `AbrirMesa`, igual que hoy `AbrirClick`.
- [Validado 2026-11-22] Hover `#DDD9CE` sobre `#EDEBE3`: se distingue; no hace falta borde `#C96442`.
- [Validado 2026-11-22] Colores en código: ninguno; todos viven en el diseñador → no hace falta un lugar común todavía.
**Tareas:** diseñador: panel con `Dock = Left` y 4 `Button` (`BtnMesa1-4`) con `FlatStyle = Flat` y `FlatAppearance.BorderSize = 0`; quitar `UcMesa1-4`, `PnlMesas`, `PnlLabelMesas` y `LblSelectMesas` si sobran; `BackgroundImage` de `PnlBackground` a ninguna; `Font` del formulario a Segoe UI. `DlgPrincipal.cs`: quitar las 4 llamadas a `Configurar`, los retratos y el `RoundControl` de `PnlLabelMesas`; conectar `BtnMesa1-4_Click` a `AbrirMesa`.
**Hecho cuando:** las 4 mesas se ven a la izquierda, el hover se nota, cada clic abre la mesa correcta, ningún control del menú usa Andy ni azul, y pasa la cabecera.
**Verificación:** manual en la app.
**Fuera de alcance:** logo, saludo, caja, chips (Fase 5); borrar archivos (Fase 9).
**Riesgos:** controles acoplados que se tapan → regla de `Dock = Fill` del `CLAUDE.md`.
**Qué debes entender:** cómo reparte el espacio `Dock`, por qué importa el orden de `Controls.Add`, y por qué un `Button` plano sustituye a `FilaMesa` (qué hacía `ConectarEventos` y por qué un `Button` no lo necesita).
**Punto de revisión:** ¿los botones de la barra sirven de modelo para los chips? ¿Hizo falta un lugar común para los colores? Si sí, las Fases 5, 6 y 8 lo usan.
**Notas:**
- 2026-11-22 · Diseñador (Alfredo): borrado `PnlContenedor` (con `PnlMesas`, `UcMesa1-4`, `PnlLabelMesas`, `LblSelectMesas`); `PnlBarraLateral` (`Dock = Left`, `#EDEBE3`) con `BtnMesa1-4` planos; `PnlBackground` sin `BackgroundImage`, sin `Min/MaximumSize` y sin borde; formulario en Segoe UI 8,25, botones en 9,75. Tamaños restaurados a mano tras el autoescalado.
- 2026-11-22 · `DlgPrincipal.cs` (Claude): quitados `Configurar`, retratos, `RoundControl` y las suscripciones a `AbrirClick`; añadidos `BtnMesa1-4_Click` → `AbrirMesa`. Los handlers se conectaron desde Propiedades → Eventos (no con doble clic) porque ya existían.
- 2026-11-22 · Causa de error: `Min/MaximumSize` = 783 × 718 en `PnlBackground` ganaba al `Dock = Fill` y lo empujaba 240 px fuera de la ventana.
- 2026-11-22 · Causa de error: con `AutoScaleMode = Font`, cambiar la fuente del formulario multiplica tamaños por `AutoScaleDimensions nuevo / viejo`. Andy 8,25 (5 × 13) → Segoe UI 8,25 (6 × 13): anchos ×1,2. Con 9,75 los botones pasaban de 40 a 52 de alto.
- 2026-11-22 · Compila; 36/36 tests; prueba de regresión manual correcta (Alfredo).
- 2026-11-22 · Punto de revisión: los botones de la barra sirven de modelo para los chips (mismo `FlatStyle` y hover). No hizo falta un lugar común para los colores.

---

## Fase 5: Área principal del menú  [x] (2026-11-22)
**Objetivo:** logo "POO26B", saludo "¿Qué practicamos hoy?" en Georgia, caja de mensaje y chips de sugerencia.
**Entregable demostrable:** el menú se parece a la referencia en la distribución, con la paleta.
**Escribe:** Alfredo en el diseñador; Claude propone `DlgPrincipal.cs`.
**Decisiones:**
- [Firme] El logo es texto "POO26B" en Georgia (registro 2026-11-22); `LogoPOO1.png` deja de usarse.
- [Firme] Los chips abren mesas con `AbrirMesa`, igual que la barra (duplicado aceptado: así es la referencia).
- [Firme] El logo es fijo, sin animación de crecer y encoger (registro 2026-11-20). Es la segunda excepción a "solo aspecto": se quita aquí, al sustituir el logo, y no en la Fase 1, para no mezclarla con la navegación.
- [Firme] El logo es un `Label` (`LblLogo`) en Georgia; sustituye a `PbxLogoMenu`.
- [Firme] La caja de mensaje es decorativa por ahora: se ve, pero no hace nada.
**Tareas:** diseñador: desconectar el evento `Load`; borrar `PbxLogoMenu`; `LblLogo`, `LblSaludo`, `PnlMensaje` (redondeado) con `TbxMensaje` (`Dock = Fill`) y `BtnEnviar` dentro, 4 `Button` planos para los chips (`BtnChip1-4`); barra lateral con título y pie. `DlgPrincipal.cs`: borrar `timerLogoAnimation` (es un campo creado con `new` en el `.cs`, no un componente del diseñador), `stopwatch`, `AnimationLogoTick`, `GetScale`, `ApplyScale`, `StartLogoAnimation`, `Clamp`, los campos `original*` y `DlgPrincipal_Load`; redondear chips, caja y botón con `UIHelper.RoundControl`.
**Hecho cuando:** se ven logo, saludo, caja y chips; no queda código de la animación; cada chip abre su mesa; pasa la cabecera.
**Verificación:** manual en la app.
**Fuera de alcance:** que la caja haga algo (ver "Después del MVP").
**Riesgos:** el área principal queda estrecha junto a la barra → se ajusta el ancho de la barra (ver "Qué observación la haría revisar").
**Qué debes entender:** por qué un `Timer` creado con `new` en el `.cs` no aparece en el diseñador y uno arrastrado desde el cuadro de herramientas sí; por qué el orden de borrado sale de quién nombra a quién (`Load +=` en el `.Designer.cs`, `PbxLogoMenu` en el `.cs`); por qué un `TextBox` no dispara `Paint`; y en qué orden acopla WinForms los controles con `Dock`.
**Punto de revisión:** ¿el menú se parece a la referencia? Si no, se anota qué falta antes de pasar a las mesas.
**Notas:**
- 2026-11-22 · `DlgPrincipal.cs` (Claude): borrado todo el código de la animación y `DlgPrincipal_Load`; `using` reducidos a 3 (+ `System.Drawing`). Grep de los nombres de la animación en los `.cs`: sin resultados.
- 2026-11-22 · Diseñador (Alfredo): `Load` desconectado, `PbxLogoMenu` borrado, controles del área principal agregados. Chips con `Click` = `BtnMesaN_Click` (elegido de la lista en Eventos, sin crear métodos).
- 2026-11-22 · `UIHelper.RoundControl` (Claude): `borderWidth/2` era división entera (1/2 = 0): con borde de 1 px no se dibujaban los lados derecho e inferior. Ahora el rectángulo del borde es `(w/2, w/2, Width - w, Height - w)` y el diámetro se limita al lado más corto.
- 2026-11-22 · Causa de error: `PnlBackground.BackColor` cambió por error a `#EDEBE3`, el mismo color que `TbxMensaje`, y la caja sin borde quedó invisible. Se volvió a `#F5F4EE`.
- 2026-11-22 · Causa de error: la primera captura era del diseñador con cambios sin guardar; el `.Designer.cs` no tenía `LblLogo`. Verificar siempre después de "Guardar todo".
- 2026-11-22 · `BtnEnviar`: 70 × 70 en `520; 10`, `Padding` del panel `14; 10; 90; 10` (margen de 10 por los cuatro lados, como la referencia).
- 2026-11-22 · Barra lateral (Alfredo): `LblTituloMesas` (Top), `BtnMesa1-4`, `PnlLineaPie` y `LblPie` (Bottom); orden de `Controls.Add` correcto. `PnlLineaLateral` no se agregó. `LblPie.Text` termina en un `\r\n` de más (sube un poco el texto).
- 2026-11-22 · Compila; 36/36 tests (tras la barra lateral). Fase terminada según Alfredo ("quedó todo listo").
- 2026-11-22 · Punto de revisión (`docs/Screenshot_6.png`, app ejecutándose): el menú se parece a la referencia en distribución; las diferencias que quedan (dificultad, avatares, "Atrás", rombo del logo) son decisiones del registro.

---

## Fase 6: Mesas 1-3  [ ]
**Objetivo:** aplicar el lenguaje visual (paleta, Segoe UI, diseño plano) a las mesas 1-3, sin cambiar su distribución.
**Entregable demostrable:** las tres mesas sin colores sueltos de WinForms (`Aquamarine`, `AliceBlue`, `SteelBlue`…).
**Escribe:** Alfredo en el diseñador; Claude propone los cambios en `MesaDePracticas1.cs` y `CDron.cs`.
**Decisiones:**
- [Firme] Las mesas no copian la distribución de chat (`CLAUDE.md`).
- [Firme] (validado en Fase 0, 2026-11-19) `LightBlue`/`LightPink` y los colores del dron significan un estado: se conserva la distinción con colores que se lean sobre la paleta. No se quita información. Qué colores usar: ver "Preguntas abiertas".
- [Validar en Fase 6] Qué color de la paleta lleva cada `PnlPracticasN` → hoy cada panel tiene un color distinto. Si el color solo decora, todos pasan a `#EDEBE3`; si sirve para distinguir prácticas, se usa el acento `#C96442` en el elemento seleccionado.
**Tareas:** diseñador de `MesaDePracticas1`, `2` y `3` (`PanelSide`, `PnlFill`, `PnlPracticas1-4`, `LblNumPersonas`, fuentes; botones a `FlatStyle = Flat` con `FlatAppearance` en la paleta); `MesaDePracticas1.cs`, `CDron.cs`.
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
**Tareas:** diseñador de `MesaDePracticas4` (`PanelSide`, `PnlFill`, `PnlProyectoF`, `PnlProyectoFIA`, etiquetas, fuentes; botones a `FlatStyle = Flat`).
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
**Objetivo:** quitar el código y los recursos de Terraria que ya no se usan, y la nota de restos del `CLAUDE.md`.
**Entregable demostrable:** el proyecto sin `FilaMesa`, sin `AplicarEstiloTerraria`, sin la fuente Andy, sin los PNG de Terraria y con el `CLAUDE.md` sin la viñeta "Restos de Terraria".
**Escribe:** Claude propone; Alfredo aplica o dice "aplícalo".
**Decisiones:** [Firme] Solo se borra lo que el Grep confirma sin uso.
**Tareas:** borrar `FilaMesa.cs`, `FilaMesa.Designer.cs`, `FilaMesa.resx` y sus entradas en el `.csproj`; `AplicarEstiloTerraria` e `Interpolar` de `UIHelper.cs`; los PNG del inventario (`TerrariaBackground`, `Personaje1-4`, `CorazonVida`, `EstrellaMana`, `LogoPOO`, `LogoPOO1`, `BotonJugar`, `PlayButton`) desde Recursos del proyecto en Visual Studio (borra la entrada de `Resources.resx` y del `.csproj`); `corazoonvida.png` del disco; el ícono si la Fase 0 lo marcó; viñeta "Restos de Terraria" de `CLAUDE.md`.
**Hecho cuando:** un Grep de `AplicarEstiloTerraria`, `"Andy"`, `FilaMesa` y de cada nombre de recurso borrado en los `.cs`, `.resx` y `.csproj` no devuelve nada, y pasa la cabecera.
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
| ¿Qué ícono sustituye al árbol de Terraria? | El de WinForms por defecto o uno propio; sin decidir | Fase 9 |
| ¿Cómo se distinguen 2 categorías (Mesa 1) y 3 estados del dron (uno al azar) con una paleta de un solo acento? | Sin decidir; no se quita el significado de los colores | Fase 6 |

## Registro de decisiones
| Fecha | Fase | Decisión | Alternativa descartada | Motivo |
|---|---|---|---|---|
| 2026-11-19 | — | El estilo chat se aplica a todo el proyecto: menú, mesas 1-3 y `MesaDePracticas4`, acuario incluido | Solo el menú y las pantallas nuevas | Un proyecto con dos estilos se ve a medio hacer; el aspecto no toca las clases `C*` |
| 2026-11-19 | — | Lenguaje visual (paleta, fuentes, plano) en todas las pantallas; distribución de chat solo en `DlgPrincipal` | Aplicar la distribución de chat también a las mesas | Una barra lateral y unos chips no tienen sentido dentro de una mesa de prácticas |
| 2026-11-19 | — | El plan se llama `PLAN-ESTILO.md`, separado de `PLAN.md` | `PLAN-MENU.md`, o una sección en `PLAN.md` | El alcance ya no es solo el menú; `PLAN.md` sigue siendo solo del comportamiento del acuario |
| 2026-11-19 | — | La imagen de referencia es inspiración, no especificación; si choca con los valores del `CLAUDE.md`, mandan los valores | Tomar medidas y colores de la imagen | La imagen generada no tiene valores exactos |
| 2026-11-19 | — | En el acuario, el marco (Fase 7) va antes que el lienzo (Fase 8) | Migrar todo el acuario en una fase | Hay riesgo de contraste sobre `#F5F4EE`, y separar protege la simulación |
| 2026-11-19 | — | Ninguna fase mezcla aspecto y comportamiento | Ajustar comportamiento cuando convenga durante la migración | Si algo se rompe, se sabe de qué plan vino |
| 2026-11-19 | 1 | [Firme] Menú y selector de mesas son una sola vista | Mantener dos vistas con "Mesas"/"Atrás" | Una lista de conversaciones siempre visible; no hay a dónde volver |
| 2026-11-19 | 1 | [Firme] Excepción a "ninguna fase mezcla aspecto y comportamiento": la Fase 1 cambia la navegación del menú, y nada más | Cambiar navegación y colores a la vez | Separada de los colores, si algo falla se sabe de dónde viene |
| 2026-11-19 | 1 | [Firme] Se eliminan "Atrás" y Esc del menú; Esc no cierra la app, se cierra con la X | Esc cierra la app | Sin destino al que volver; cerrar con Esc sería una salida accidental |
| 2026-11-19 | 2, 3 | [Firme] Se descartan dificultad, retratos, maná, vida y tiempo | Rediseñarlos con punto y avatar | No encajan en una lista de conversaciones |
| 2026-11-19 | 4 | [Firme] `FilaMesa` se elimina; se usan `Button` planos | Reducir `FilaMesa` a solo nombre | Le quedaría nombre + hover + clic, que `Button` ya trae |
| 2026-11-19 | 4 | [Firme] `Button` con `FlatStyle = Flat`, no `IconButton` de FontAwesome.Sharp | `IconButton` | Botones solo de texto; evita una dependencia NuGet |
| 2026-11-19 | 5 | [Firme] Mesas en la barra lateral y también como chips | Solo barra, o solo chips | Así es la referencia; el duplicado son 4 handlers |
| 2026-11-19 | 5 | [Firme] La caja de mensaje es decorativa por ahora | Que acepte texto | Sería comportamiento nuevo; va en "Después del MVP" |
| 2026-11-19 | 4, 5, 9 | [Firme] Se deja de usar Andy y se eliminan todos los recursos de Terraria | Conservar los que no estorban | El diseño anterior está retirado; recursos sin uso confunden |
| 2026-11-19 | 5 | [Firme] Logo "PO26B" en texto Georgia en lugar de `LogoPOO1.png` | Conservar la imagen | Coherente con el estilo chat |
| 2026-11-20 | 5 | [Firme] Logo fijo, sin animación de crecer y encoger; se quita en la Fase 5 junto con `PbxLogoMenu` | Conservar la animación con `Timer` + `Stopwatch`; quitarla en la Fase 1 | Un logo fijo es más fiel al diseño plano. En la Fase 1 se mezclaría con la navegación |
| 2026-11-22 | 4 | Se amplía el formulario para dar sitio a la barra lateral | Encoger el área principal a 543 px | El logo de 690 px no cabía; la Fase 5 necesita el ancho original |
| 2026-11-22 | 4 | Fuente del formulario Segoe UI 8,25; los botones llevan 9,75 propia | Formulario en 9,75 | 9,75 reescala todos los controles (altos ×17/13); 8,25 conserva los altos |
| 2026-11-22 | 4 | Colores solo en el diseñador; sin clase ni constantes comunes | Un lugar común para los colores ya en la Fase 4 | Ningún `.cs` usa colores; se reevalúa cuando un color aparezca en dos o más `.cs` |
| 2026-11-22 | 4 | Hover `#DDD9CE` sin borde | Borde `#C96442` | El hover se distingue (validado por Alfredo) |
| 2026-11-22 | 5 | Logo "POO26B" (doble O, como el `.csproj`); sustituye a "PO26B" del registro 2026-11-19 | "PO26B" (como el namespace) | Decisión de Alfredo |
| 2026-11-22 | 5 | Los chips reutilizan `BtnMesa1-4_Click` | 4 handlers `BtnChipN_Click` | La regla "este botón abre la mesa N" queda en un solo sitio; coste: el nombre del handler no sigue `Control_Evento` para los chips |
| 2026-11-22 | 5 | Caja redondeada con un `Panel` contenedor (`PnlMensaje`) | Redondear el `TextBox` directamente | `TextBox` es un control nativo y no dispara `Paint`: no tendría borde y el texto se cortaría en las esquinas |
| 2026-11-22 | 5 | Flecha de `BtnEnviar` como texto "↑" | Imagen PNG; dibujarla en `Paint` | Vectorial (nítida con cualquier escala), color con `ForeColor`, sin recurso nuevo |
| 2026-11-22 | 5 | `BtnEnviar` decorativo: sin `Click`, sin cambio de color en hover, `TabStop = False` | Hover visible | La caja es decorativa; no debe parecer que el botón hace algo |
| 2026-11-22 | 5 | Barra lateral: título "Mesas", botones de 48 px y pie en texto secundario | Subtítulo por mesa; barra más estrecha | Llena el hueco sin contenido nuevo y sin recolocar el área principal |
| 2026-11-22 | 5 | Se corrige `UIHelper.RoundControl` (borde y tope del diámetro) | Dejarlo como estaba | Con borde de 1 px faltaban dos lados |

## Riesgos globales
- **Dos planes sobre `MesaDePracticas4`:** no se ejecutan a la vez una fase de `PLAN.md` y la Fase 7 u 8 de este plan.
- **Romper la navegación al rehacer el menú:** la prueba de regresión de la cabecera se hace en cada fase, no solo al final.
- **Que el color se vuelva a escribir en muchos sitios:** en la Fase 5 aparecen colores en código, pero solo en `DlgPrincipal.cs` (`RedondearControles`). Sigue sin hacer falta un lugar común; se reevalúa en las Fases 6 y 8 si un color aparece en dos o más `.cs`.
