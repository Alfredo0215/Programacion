# Plan: Chat con Gemini en el menú principal

Formato: planificar-fases v1 · módulos: software

## Estado actual
- Fase en curso: 0, Validación · Última actualización: 2026-11-23
- **Siguiente acción:**
  1. Crear una clave de API de Gemini y guardarla en la variable de entorno de usuario `GEMINI_API_KEY` desde la ventana de Windows (no desde la consola); después reiniciar Visual Studio y PowerShell.
  2. Hacer la petición de prueba de la Fase 0 desde PowerShell y anotar en "Notas" el JSON real de éxito y el de un error.

Leyenda: [ ] pendiente · [~] en curso · [h] hecho sin verificar · [x] verificado (fecha)
Decisiones: [Firme] ya decidida · [Validar en Fase N] supuesto a comprobar
Hechos externos: [volátil] dato que caduca (precio, versión, condición de un servicio): se anota con fuente y fecha
Modo de trabajo: `CLAUDE.md` de usuario (sugerir, no escribir) y `CLAUDE.md` del proyecto (diseñador, `.csproj`, compilar, tests).

**Hecho siempre incluye:** el proyecto compila sin errores y los tests pasan, con los comandos de la sección "Compilar" y "Tests" del `CLAUDE.md` del proyecto.

Relación con los otros planes: `PLAN.md` es del acuario y `PLAN-ESTILO.md` del aspecto. Este plan cubre una función nueva del menú; el aspecto de los controles nuevos sigue el "Estilo chat" del `CLAUDE.md`.

## Resumen de la arquitectura
- **Decisión:** llamada REST directa a la API de Gemini con `HttpClient` y `async`/`await`; JSON con `Newtonsoft.Json`. El historial (máx. 10 elementos, roles `user`/`model`) lo guarda la app en memoria y lo reenvía completo en cada petición. La lógica de la API va en una clase propia, fuera de `DlgPrincipal`.
- **Alternativa descartada:** usar un SDK de Google. Esconde la petición y la respuesta, que es justo lo que se quiere aprender; además no está verificado que haya uno compatible con .NET Framework 4.7.2.
- **Qué observación la haría revisar:** que `HttpClient` o TLS den problemas en 4.7.2 que no se resuelvan con una referencia o una línea de configuración.

## Mapa de fases
| Fase | Entregable demostrable | Escribe | Estado |
|---|---|---|---|
| 0 | Una respuesta de Gemini obtenida desde PowerShell y el proyecto compilando con `System.Net.Http` y `Newtonsoft.Json` | Alfredo guiado por Claude | [ ] |
| 1 | Menú con la nueva distribución (conversación, caja reducida con scroll, chips debajo), sin lógica | Alfredo (diseñador) | [ ] |
| 2 | Una pregunta y su respuesta en el menú, sin historial | Alfredo guiado por Claude | [ ] (esqueleto) |
| 3 | Conversación con historial de 10 elementos y manejo de errores | Alfredo guiado por Claude | [ ] (esqueleto) |

---

## Fase 0: Validación  [ ]
**Objetivo:** comprobar la clave, el endpoint, la forma del JSON y que el proyecto puede hacer peticiones HTTP antes de tocar el menú.
**Entregable demostrable:** una petición desde PowerShell (`Invoke-RestMethod`) que devuelve texto de Gemini, y el proyecto compilando con las referencias nuevas.
**Escribe:** Alfredo guiado por Claude.
**Decisiones:**
- [Validar en Fase 0] [volátil] Endpoint, nombre de modelo y forma del JSON de petición (`contents`, `role`, `parts`) y de respuesta. Se toman de la documentación oficial, con fuente y fecha. → Fija el código de las Fases 2 y 3.
- [Validar en Fase 0] [volátil] Límites del plan gratuito (peticiones por minuto y por día) de cada modelo candidato. → Deciden el modelo (ver la decisión siguiente). Si son muy bajos, la Fase 3 añade un mensaje claro para el error de cuota.
- [Validar en Fase 0] Modelo a usar, elegido **después** de consultar cuotas y límites. Criterio, fijado antes de mirar: (1) disponible en el plan gratuito; (2) su límite diario cubre una sesión de pruebas, estimada en unas 50 peticiones; (3) entre los que cumplen, el de mejor calidad según la documentación oficial. → El nombre del modelo se fija en las Fases 2 y 3; si ninguno cumple (2), se revisa el alcance de la Fase 3.
- [Validar en Fase 0] `System.Net.Http` se puede referenciar en el `.csproj` de estilo antiguo y `Newtonsoft.Json` se instala por NuGet. → Si falla, se revisa la arquitectura.
- [Validar en Fase 0] [volátil] La clave se envía en la cabecera `x-goog-api-key` y no como parámetro `?key=` de la URL (comprobar en la documentación que la API lo acepta). → Evita que la clave aparezca al copiar la URL en "Notas" o en un registro.
- [Validar en Fase 0] [volátil] Condiciones de uso de datos del plan gratuito (si Google puede usar lo enviado para mejorar sus productos), con fuente y fecha. → Si es así, no se envían datos personales en pruebas ni en la demo.
- [Validar en Fase 0] `System.Net.Http` se puede referenciar en el `.csproj` de estilo antiguo y `Newtonsoft.Json` se instala por NuGet. → Si falla, se revisa la arquitectura.
- [Firme] La clave vive en la variable de entorno de usuario `GEMINI_API_KEY` y nunca se escribe en el repo.
- La conexión HTTPS desde C# no se valida aquí: la petición de PowerShell es otro proceso y compilar no abre conexiones. Se valida en la Fase 2.
**Tareas:**
1. Crear la clave y guardarla en `GEMINI_API_KEY` desde *Editar las variables de entorno de tu cuenta* en Windows, no con un comando en la consola (el historial de PowerShell la guardaría en texto plano). Después, cerrar y volver a abrir Visual Studio y PowerShell: un proceso solo ve las variables que existían al arrancar.
2. Petición de prueba desde PowerShell leyendo `$env:GEMINI_API_KEY`, con la clave en la cabecera; guardar en "Notas" un ejemplo del JSON enviado y del recibido (sin la clave). No imprimir la variable en pantalla ni en una sesión de Claude.
3. Petición con una clave inválida a propósito; anotar en "Notas" el código HTTP y el cuerpo del error.
4. En Visual Studio: agregar la referencia `System.Net.Http` y el paquete NuGet `Newtonsoft.Json` a `POO26B_FSA.csproj`.
5. Compilar.
**Hecho cuando:** PowerShell imprime la respuesta de Gemini; el proyecto compila con las dos referencias; "Notas" tiene endpoint, modelo, forma de enviar la clave, límites y condiciones de uso de datos (con fuente y fecha), el ejemplo de JSON de éxito y el de error.
**Verificación:** manual (peticiones en PowerShell) y automática (compilación).
**Fuera de alcance:** cualquier cambio en `DlgPrincipal` o código C# de la función.
**Riesgos:** un supuesto resulta falso → se registra y se reajustan las fases en el punto de revisión. La clave aparece en un archivo del repo → se revoca y se crea otra. La app lee `null` en `GEMINI_API_KEY` aunque la variable existe → Visual Studio estaba abierto al crearla; reiniciarlo.
**Qué debes entender:** que una API de LLM es una petición HTTP normal (URL, cabeceras, cuerpo JSON) y que la API no recuerda nada entre peticiones (*stateless*). Por qué la clave no va en el código.
**Punto de revisión:** ¿algún supuesto resultó falso? ¿Cambia la forma del JSON lo previsto en las Fases 2 y 3?
**Notas:** (se llena al ejecutar)

---

## Fase 1: Distribución del menú  [ ]
**Objetivo:** dejar la interfaz lista para la conversación, sin lógica nueva.
**Entregable demostrable:** el menú se abre con la nueva distribución y las mesas siguen abriéndose desde la barra lateral y los chips.
**Depende de:** nada; puede hacerse en paralelo con la Fase 0.
**Escribe:** Alfredo en el diseñador de Visual Studio; Claude explica los pasos.
**Decisiones:**
- [Firme] La conversación se muestra en un control de solo lectura entre el saludo y la caja (`TextBox` multilínea con `ReadOnly = true` y `ScrollBars = Vertical`). Si lleva esquinas redondeadas, va dentro de un `Panel` (los `TextBox` no disparan `Paint`).
- [Firme] La caja de mensaje baja al fondo y se reduce a unos 50 px de alto; `TbxMensaje` con `ScrollBars = Vertical` y `AcceptsReturn = true` (Enter baja de línea).
- [Firme] `BtnEnviar` se achica en proporción a la caja; es el único modo de enviar.
- [Firme] Los chips se quedan, debajo de la caja. El tamaño del formulario no cambia (1208 × 679).
**Tareas:** en `DlgPrincipal.cs [Diseño]`: mover y redimensionar `PnlMensaje`, `BtnEnviar` y los chips; agregar el control de la conversación (y su panel) con nombres según la convención (`Tbx…`, `Pnl…`); colores de la paleta del "Estilo chat". `DlgPrincipal.cs` no se toca, salvo redondear el panel nuevo en `RedondearControles()` si lo lleva.
**Hecho cuando:** al ejecutar, se ven saludo, área de conversación, caja y chips sin que nada se tape; en la caja se escriben varias líneas con Enter y aparece el scroll; los cuatro chips y los cuatro botones de la barra abren su mesa.
**Verificación:** manual en la app; no hay prueba automática porque es solo distribución visual.
**Fuera de alcance:** cualquier llamada a la API o handler de `BtnEnviar`.
**Riesgos:** la conversación queda demasiado baja para leerse → subir o compactar logo y saludo, y anotarlo en el registro.
**Qué debes entender:** cómo `Multiline`, `AcceptsReturn` y `ScrollBars` cambian el comportamiento de un `TextBox`, y por qué un control nativo necesita un `Panel` para tener bordes propios.
**Punto de revisión:** ¿cuántas líneas de conversación caben sin scroll? Si son muy pocas, ¿se reajusta el logo o el saludo?
**Notas:** (se llena al ejecutar)

---

## Fase 2: Una pregunta, una respuesta  [ ]  (esqueleto: depende de [Validar en Fase 0])
**Objetivo:** que `BtnEnviar` mande el texto de la caja a Gemini y muestre la respuesta, sin historial y sin congelar la ventana. · **Entregable demostrable:** se escribe una pregunta, se pulsa el botón, la ventana sigue respondiendo mientras espera y aparece la respuesta en la conversación. · **Hecho cuando:** pregunta y respuesta se ven en el control; durante la espera se puede mover la ventana y el botón está desactivado; un mensaje vacío o con solo espacios o saltos de línea no se envía (`string.IsNullOrWhiteSpace`); si la petición falla, el botón vuelve a activarse (`try`/`finally`), aunque el mensaje de error claro llegue en la Fase 3. · **Decisiones:** [Validar en Fase 2] La conexión HTTPS funciona desde la app en 4.7.2 en Windows 11 sin configurar TLS a mano. → Si falla, se añade la configuración de TLS al arrancar.

## Fase 3: Historial y errores  [ ]  (esqueleto: depende de [Validar en Fase 0])
**Objetivo:** mantener una conversación de hasta 10 elementos y manejar los fallos sin romperla. · **Entregable demostrable:** una conversación de varios turnos en la que Gemini recuerda lo anterior, y un mensaje claro si no hay red, la clave falta, se agota la cuota, la respuesta llega sin texto (candidato bloqueado o vacío; forma exacta según la documentación) o se pasa el tiempo de espera. · **Hecho cuando:** los tests del historial pasan (recorte por pares al pasar de 10, la lista nunca empieza con `model`, una petición fallida no deja la pregunta en el historial); en la app, Gemini responde usando un dato dicho dos turnos antes; con la red desconectada aparece el error y la siguiente pregunta funciona. · **Decisiones:** [Validar en Fase 3] Tiempo de espera de `HttpClient`: comprobar su valor por defecto en la documentación y decidir si sirve para la demo. · Los errores se programan a partir del JSON de error anotado en la Fase 0.

---

## Después del MVP
- Guardar la conversación entre sesiones.
- Mostrar la respuesta a medida que llega (*streaming*).
- Botón para borrar la conversación.

## Preguntas abiertas
| Pregunta | Supuesto mientras tanto | Cuándo se resuelve |
|---|---|---|
| ¿Una clase o dos (conversación + cliente HTTP)? | Probar sin red ya es obligatorio (lo exige la Fase 3), pero no obliga a tener dos clases. Criterio: ¿se pueden llamar desde un test las reglas del historial (recorte por pares, descarte de la pregunta si la petición falla) sin pasar por `HttpClient`? Si una sola clase lo permite con métodos separados, una; si no, dos | Propuesta de la Fase 2 |

## Registro de decisiones
| Fecha | Fase | Decisión | Alternativa descartada | Motivo |
|---|---|---|---|---|
| 2026-11-23 | — | Plan propio (`PLAN-GEMINI.md`) | Meterlo en `PLAN.md` o `PLAN-ESTILO.md` | No es del acuario y mezcla aspecto y comportamiento |
| 2026-11-23 | — | REST directo con `HttpClient` | SDK de Google | El objetivo es ver la petición y la respuesta |
| 2026-11-23 | 1 | Conversación entre saludo y caja; caja al fondo | Reemplazar el saludo con la respuesta | Entrada fija abajo y lectura en orden de tiempo |
| 2026-11-23 | 1 | Reducir la caja (~50 px) con scroll | Agrandar el formulario | No depende de la resolución ni del escalado de la pantalla de demo |
| 2026-11-23 | 1 | Enter baja de línea (`AcceptsReturn = true`); envía `BtnEnviar` | Enter envía, Shift+Enter baja | Sin código extra en `KeyDown` |
| 2026-11-23 | — | JSON con `Newtonsoft.Json` | Serializadores que trae .NET Framework | Más simple de usar con JSON anidado; es el paquete más común para 4.7.2 |
| 2026-11-23 | 3 | Historial de 10 elementos (5 pares), en memoria, recortado por pares | Recortar de uno en uno | Mantiene la alternancia `user`/`model`. Sin verificar si la API la exige o solo la recomienda; la decisión vale en los dos casos |
| 2026-11-23 | 0→2 | Validar HTTPS/TLS desde C# en la Fase 2 | Validarlo en la Fase 0 | La Fase 0 no tenía tarea que lo probara: PowerShell es otro proceso y compilar no conecta |
| 2026-11-23 | 2 | Botón reactivado con `try`/`finally` ya en la Fase 2 | Dejar todo el manejo de errores para la Fase 3 | Sin eso, un fallo deja el botón desactivado para siempre |
| 2026-11-23 | 2 | `BtnEnviar` desactivado durante la petición | Dejarlo activo | Dos peticiones a la vez romperían el historial |
| 2026-11-23 | 0 | El modelo se elige tras consultar cuotas y límites, con criterio fijado antes | Elegir el modelo de antemano | Los límites del plan gratuito cambian por modelo y deciden si alcanza para probar y hacer la demo |
| 2026-11-23 | 2 | Dos clases solo si aportan algo real (criterio en "Preguntas abiertas") | Decidirlo por buenas prácticas en abstracto | Evitar sobre-ingeniería |
| 2026-11-23 | 0 | Clave en `GEMINI_API_KEY`; la demo la hace Alfredo | Clave en un archivo de configuración | El repo es público en GitHub |

## Riesgos globales
- **Filtrar la clave a GitHub:** solo se lee de la variable de entorno; antes de cada commit, revisar que no aparece en el diff.
- **Sin internet o API caída en la demo:** probar la red antes de presentar; la Fase 3 garantiza un mensaje de error en vez de un cuelgue.
- **La API cambia (modelo, endpoint, límites):** son datos `[volátil]`; se vuelven a comprobar en la documentación oficial si algo deja de funcionar.
