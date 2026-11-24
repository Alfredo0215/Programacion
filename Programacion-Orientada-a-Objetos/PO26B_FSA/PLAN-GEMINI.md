# Plan: Chat con Gemini en el menú principal

Formato: planificar-fases v1 · módulos: software

## Estado actual
- Fase en curso: 3, Historial y errores (código escrito, falta verificar) · Última actualización: 2026-11-24
- **Siguiente acción:** Alfredo verifica la Fase 3 con F5 según su "Hecho cuando" (4-5 peticiones) y la marca `[x]`. Con eso termina el MVP.

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
| 0 | Una respuesta de Gemini obtenida desde PowerShell y el proyecto compilando con `System.Net.Http` y `Newtonsoft.Json` | Alfredo guiado por Claude | [x] 2026-11-24 |
| 1 | Menú con la nueva distribución (conversación, caja reducida con scroll, chips debajo), sin lógica | Alfredo (diseñador) y Claude (completó propiedades) | [x] 2026-11-24 |
| 2 | Una pregunta y su respuesta en el menú, sin historial | Claude (autorizado por Alfredo) | [x] 2026-11-24 |
| 3 | Conversación con historial de 10 elementos y manejo de errores | Claude (autorizado por Alfredo) | [h] |

---

## Fase 0: Validación  [x] 2026-11-24
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
**Respuesta (2026-11-24):** sí, uno: el criterio (2) pedía unas 50 RPD y ningún modelo revisado (3.8 y 3.6 Flash) pasa de 20. Se acepta 20 (ver registro) y se ajusta la Fase 3. La forma del JSON no cambia las Fases 2 y 3; sí cambian los errores (clasificar por `status`/`reason`) y queda por validar `thoughtSignature`.
**Notas:** (2026-11-24)
- Documentación: `ai.google.dev/gemini-api/docs/*` redirige a `aistudio.google.com/docs/*` (consultada 2026-11-24).
- Endpoint: `https://generativelanguage.googleapis.com/v1beta/models/{modelo}:generateContent`, POST.
- Clave: cabecera `x-goog-api-key`. Verificado con petición real.
- Modelo elegido: `gemini-3.6-flash` (Stable, gratis). Límites del plan gratuito: 20 RPD, 5 RPM, 250K TPM (AI Studio, 2026-11-24). 3.8 Flash tiene los mismos.
- Las peticiones fallidas (503) parecen contar en la cuota: 3.8 marcaba 3/20 tras tres 503 (inferencia).
- El TPM cuenta solo tokens de entrada; los de pensamiento no lo afectan (gráfica de AI Studio: "Tokens de entrada máximos por minuto").
- Descartado `gemini-3.8-flash`: 503 `UNAVAILABLE` "high demand" dos veces el 2026-11-24; desde 2026-12-01 recibe además el tráfico de `gemini-2.5-pro`.
- Uso de datos: en todos los modelos gratuitos "Used to improve our products: Yes". No enviar datos personales.
- Límites: por proyecto y por modelo; se ven en AI Studio, no en la documentación.
- Respuesta: texto en `candidates[0].content.parts[0].text`; `role` = `"model"`; `finishReason` = `"STOP"`.
- Tokens (pregunta corta): 14 entrada, 40 salida, 347 de pensamiento, 401 en total.
- La respuesta trae `thoughtSignature` junto al texto. → [Validar en Fase 3] ¿Hay que devolverla en el historial?
- Error clave inválida: 400, `status` `INVALID_ARGUMENT`, `reason` `API_KEY_INVALID`.
- Error servidor saturado: 503, `status` `UNAVAILABLE` (temporal; tiene sentido reintentar).
- PowerShell 5.1: `$_.ErrorDetails.Message` sale vacío; el cuerpo del error se lee con `StreamReader` sobre `GetResponseStream()`.
- HTTPS desde PowerShell: funciona sin configurar TLS. (La app C# se valida en la Fase 2.)
- `System.Net.Http`: ya estaba en el .csproj. `Newtonsoft.Json`: 13.0.4 por `packages.config`; compila y los 36 tests pasan (2026-11-24).

---

## Fase 1: Distribución del menú  [x] 2026-11-24
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
**Notas:** (2026-11-24)
- Distribución dentro de `PnlBackground` (920 × 679): `LblLogo` y=30; `LblSaludo` y=76; `TbxConversacion` (172; 148) 600 × 360, sin borde, fondo `245; 244; 238`, sin panel; `PnlMensaje` (172; 524) 600 × 50, `Padding` `14; 8; 52; 8`; `BtnEnviar` (556; 8) 34 × 34, fuente 12 pt; chips y=590.
- Alfredo hizo la distribución en el diseñador; Claude completó en el `.Designer.cs`, con permiso y el diseñador cerrado, las propiedades que faltaban (`ReadOnly`, `BorderStyle`, colores, `TabStop`, `AcceptsReturn`, `Padding`, fuente del botón).
- Se corrigió un solapamiento previo: con `Padding` derecho 64 y el botón en x=520 (70 px), el texto quedaba 16 px bajo el botón.
- Después de verificar, se quitaron las barras de scroll de `TbxConversacion` y `TbxMensaje` (`ScrollBars = None`): WinForms las mostraba siempre, en gris aunque no hicieran falta. El texto se desplaza con la rueda y con las flechas.
- Verificado por Alfredo con F5: todo se ve sin taparse, Enter baja de línea, chips y barra lateral abren sus mesas.

---

## Fase 2: Una pregunta, una respuesta  [x] 2026-11-24
**Objetivo:** que `BtnEnviar` mande el texto de la caja a Gemini y muestre la respuesta, sin historial y sin congelar la ventana.
**Entregable demostrable:** se escribe una pregunta, se pulsa el botón, la ventana sigue respondiendo mientras espera y aparece la respuesta en la conversación.
**Depende de:** Fase 0 (endpoint, modelo, forma del JSON) y Fase 1 (`TbxConversacion`).
**Escribe:** Claude, con autorización explícita de Alfredo (ver registro).
**Decisiones:**
- [Firme] Una sola clase, `CClienteGemini` (`internal`, raíz del proyecto, namespace `PO26B_FSA`), con `EnviarAsync(string pregunta)` → `Task<string>`. Cierra la pregunta abierta "¿una o dos clases?" (ver registro).
- [Firme] `HttpClient` estático y compartido; modelo `gemini-3.6-flash` en una constante; clave leída de `GEMINI_API_KEY` en cada petición y enviada en la cabecera `x-goog-api-key` del `HttpRequestMessage` (no en `DefaultRequestHeaders`). Si falta la variable, `InvalidOperationException`.
- [Firme] Cuerpo con un tipo anónimo serializado con `JsonConvert`; texto leído con `JObject` en `candidates[0].content.parts[0].text` (sin clases para la respuesta). Un código HTTP de error lanza `HttpRequestException` con el código y el cuerpo.
- [Firme] En `DlgPrincipal`: `async void BtnEnviar_Click` (`Trim`, sale si queda vacío, `Enabled = false`, `try`/`catch`/`finally`) y `AgregarALaConversacion(quien, texto)`, que convierte `\n` en `Environment.NewLine` (el `TextBox` solo salta con `\r\n`).
- [Firme] `catch` mínimo: escribe `Error: <mensaje>` en la conversación para que una excepción en un `async void` no abra el cuadro de excepción no controlada. Los mensajes claros por `status`/`reason` son de la Fase 3.
- [Validar en Fase 2] La conexión HTTPS funciona desde la app en 4.7.2 en Windows 11 sin configurar TLS a mano. → Si falla (error de "canal seguro" en la conversación), se añade la configuración de TLS al arrancar.
**Tareas (hechas por Claude, 2026-11-24):** crear `CClienteGemini.cs`; agregar `<Compile Include="CClienteGemini.cs" />` al `.csproj`; atributo `gemini`, handler y `AgregarALaConversacion` en `DlgPrincipal.cs`; conectar `BtnEnviar.Click` en `DlgPrincipal.Designer.cs` (con permiso y el diseñador cerrado); compilar y ejecutar los tests.
**Hecho cuando:** pregunta y respuesta se ven en el control; durante la espera se puede mover la ventana y el botón está desactivado; un mensaje vacío o con solo espacios o saltos de línea no se envía; si la petición falla, el botón vuelve a activarse y el error aparece en la conversación.
**Verificación:** automática (compila; 36 tests pasan, 2026-11-24) y manual con F5 por Alfredo (2-3 peticiones; no hay test de `EnviarAsync` porque exige red y gasta cuota).
**Fuera de alcance:** historial, clasificación de errores por `status`/`reason`, `thoughtSignature`, tiempo de espera propio, indicador de "pensando…", formato Markdown de las respuestas.
**Qué debes entender:** qué hace `await` con el hilo de la ventana (lo libera mientras espera y vuelve a él); por qué `async void` solo en el evento; que `using` y `finally` se ejecutan aunque haya excepción; cómo `JObject` recorre el JSON con `["campo"][índice]`.
**Punto de revisión:** ¿funcionó HTTPS sin configurar TLS? ¿La respuesta se lee bien (saltos de línea, Markdown)?
**Respuesta (2026-11-24):** HTTPS funciona desde la app sin configurar TLS (el supuesto `[Validar en Fase 2]` se cumple). Lectura de la respuesta (Markdown, rueda del ratón sin barra): no reportado.
**Notas:** (2026-11-24) Verificado por Alfredo con F5: pruebas del "Hecho cuando" pasan.

## Fase 3: Historial y errores  [h]
**Objetivo:** mantener una conversación de hasta 10 elementos y manejar los fallos sin romperla.
**Entregable demostrable:** una conversación de varios turnos en la que Gemini recuerda lo anterior, y un mensaje claro si no hay red, la clave falta o no es válida, se agota la cuota, el servidor falla, la respuesta llega sin texto o se pasa el tiempo de espera.
**Depende de:** Fase 2 (`CClienteGemini`, `BtnEnviar_Click`).
**Escribe:** Claude, con autorización explícita de Alfredo (ver registro).
**Decisiones:**
- [Firme] El historial (`List<JObject>`, máx. 10 elementos) vive en `CClienteGemini`. Cada elemento es un `content` de la API (`{ role, parts }`); las respuestas del modelo se guardan **tal como llegan**, con su `thoughtSignature`, y se reenvían igual.
- [Firme] `EnviarAsync` envía una copia (`new JArray(historial)` + el turno del usuario); el historial solo cambia en `AgregarTurno`, que se llama tras una respuesta válida. Recorte por pares con `RemoveRange(0, 2)`: nunca empieza con `model`.
- [Firme] Funciones sin red y probables: `AgregarTurno`, `GetHistorial`, `ExtraerContenido`, `TextoDe` y `MensajeDeError` (las tres últimas `static`), todas `internal`. `TextoDe` une el `text` de todas las partes.
- [Firme] Errores: `HttpRequestException` y `TaskCanceledException` se capturan solo alrededor de `SendAsync` (sin conexión / tiempo agotado). Un código HTTP de error se traduce con `MensajeDeError`: `reason` `API_KEY_INVALID` → clave; 429 → límite y cuota; 500/503/504 → reintentar en unos minutos; 403 → permiso; 404 → modelo; otro → código y `error.message`. Un cuerpo que no es JSON se clasifica solo por el código. `promptFeedback.blockReason` → pregunta bloqueada; sin texto → `finishReason`.
- [Firme] En `DlgPrincipal`, si falla y la caja está vacía, la pregunta vuelve a `TbxMensaje`: reintentar es un clic y lo decide el usuario.
- [Firme] Cuota de 20 RPD: sin reintento automático (un fallo también gasta cuota); la lógica del historial se prueba con tests sin red; las pruebas manuales se limitan a lo que pide el "Hecho cuando".
- Resuelto (2026-11-24) [volátil] `thoughtSignature`: la documentación dice que las firmas *"are required to maintain reasoning continuity across multi-turn interactions"* y existe el error `MISSING_THOUGHT_SIGNATURE`, pero no aclara si en chat de solo texto con `generateContent` faltar da 400 o solo degrada el razonamiento. Decisión: guardar cada `content` tal como llega, correcto en los dos casos. Fuente: `aistudio.google.com/docs/thinking` (#signatures) y `/docs/thought-signatures`.
- Resuelto (2026-11-24) Tiempo de espera: `HttpClient.Timeout` vale 100 s por defecto (learn.microsoft.com, `HttpClient.Timeout`). Se fija en 60 s. Hipótesis sin confirmar: en .NET Framework, agotarlo lanza `TaskCanceledException` (la página de Microsoft no lo dice); si fuera otra, el error sale con el mensaje genérico de .NET, sin romper nada.
**Tareas (hechas por Claude, 2026-11-24):** reescribir `EnviarAsync` y añadir historial y funciones de lectura/errores en `CClienteGemini.cs`; cambiar el `catch` de `BtnEnviar_Click`; crear `PO26B_FSA.Tests\CClienteGeminiTests.cs` (11 tests); añadir `<PackageReference Include="Newtonsoft.Json" Version="13.0.4" />` a `PO26B_FSA.Tests.csproj` (los tests usan `JObject` y la referencia del proyecto principal, por `packages.config`, no llega al de tests).
**Hecho cuando:** los tests del historial pasan (recorte por pares al pasar de 10, la lista nunca empieza con `model`, una petición fallida no deja la pregunta en el historial) junto con los de lectura y errores; en la app, Gemini responde usando un dato dicho dos turnos antes; con la red desconectada aparece "No hay conexión…", la pregunta vuelve a la caja y, al reconectar, la siguiente pregunta funciona y Gemini conserva el recuerdo.
**Verificación:** automática (compila; 47 tests pasan: 36 antiguos + 11 nuevos, 2026-11-24) y manual con F5 por Alfredo (4-5 peticiones).
**Fuera de alcance:** Markdown, *streaming*, guardar la conversación entre sesiones, botón para borrar, reintento automático, clase de excepción propia.
**Qué debes entender:** por qué el recorte es por pares; que `new JArray(historial)` es una lista nueva para enviar y el historial solo cambia en `AgregarTurno`; qué es la `thoughtSignature` y por qué se reenvía sin leerla; que la pantalla muestra toda la conversación pero Gemini solo recuerda los últimos 5 pares; por qué `TaskCanceledException` es una hipótesis.
**Punto de revisión:** ¿Gemini recuerda el dato? ¿Los mensajes de error se entienden? ¿Hizo falta algún reintento por 503?
**Notas:** (2026-11-24) Hechos de la documentación (aistudio.google.com/docs, consultada 2026-11-24):
- Para 3.6: si el último turno no vacío de la petición es `model`, la API responde 400 (*"Prefilled model turn validation"*). El historial siempre termina en el turno del usuario.
- No hay regla documentada de alternancia `user`/`model`.
- Pregunta bloqueada: llega `promptFeedback.blockReason` y no llega `candidates` (*"Returns no candidates at all only if there was something wrong with the prompt"*).
- Reintentos: *"Only retry on transient errors (like 429, 408, or 5xx). Do not retry on client errors (like 400, 402, or 403)."* (`/docs/troubleshooting`).
- `temperature`, `top_p` y `top_k` están *"deprecated and ignored"* en 3.6 y darán 400 en modelos futuros; la app no los envía.

---

## Después del MVP
- Guardar la conversación entre sesiones.
- Mostrar la respuesta a medida que llega (*streaming*).
- Botón para borrar la conversación.

## Preguntas abiertas
| Pregunta | Supuesto mientras tanto | Cuándo se resuelve |
|---|---|---|
| (ninguna por ahora; "¿una clase o dos?" se cerró el 2026-11-24, ver registro) | — | — |

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
| 2026-11-24 | 0 | Modelo `gemini-3.6-flash` | `gemini-3.8-flash` | 3.8 dio 503 por alta demanda y desde 2026-12-01 absorbe el tráfico de `gemini-2.5-pro`; 3.6 es Stable, gratis y respondió |
| 2026-11-24 | 0 | Se acepta 20 RPD con `gemini-3.6-flash` (cambio del criterio 2, que pedía ~50) | Buscar un modelo con ≥ 50 RPD o usar dos modelos | Alfredo: "ni siquiera las alcanzaré al hacer la demo". El desarrollo se cubre con tests sin red y pocas pruebas manuales por día |
| 2026-11-24 | 1 | Conversación sin caja: `TextBox` sin borde, color del fondo, sin `Panel` | Conversación dentro de un `Panel` con borde y esquinas | Elección de Alfredo; más simple, no toca `RedondearControles()` |
| 2026-11-24 | 1 | `ScrollBars = None` en `TbxConversacion` y `TbxMensaje` | `ScrollBars = Vertical` (siempre visible) o `RichTextBox` (barra solo si hace falta) | Elección de Alfredo: la barra gris permanente no gustaba; `None` es una sola propiedad. Se pierde la pista visual de que hay más texto |
| 2026-11-24 | 2 | Una sola clase (`CClienteGemini`) | Dos clases (conversación + cliente HTTP) | Criterio de "Preguntas abiertas": las reglas del historial de la Fase 3 serán métodos que no tocan `HttpClient`, así que se prueban sin red en la misma clase |
| 2026-11-24 | 2 | `catch` mínimo que escribe `Error: <mensaje>` en la conversación | Sin `catch` hasta la Fase 3 | En un `async void`, una excepción sin capturar abre el cuadro de excepción no controlada de WinForms |
| 2026-11-24 | 2 | Convertir `\n` en `Environment.NewLine` al mostrar | Mostrar el texto tal cual | El `TextBox` solo salta de línea con `\r\n`; la API usa `\n` |
| 2026-11-24 | 2 | La Fase 2 la escribe Claude | "Alfredo guiado por Claude" | Autorización explícita de Alfredo ("tú escribes el código de este plan") |
| 2026-11-24 | 3 | Guardar cada `content` del modelo tal como llega (con `thoughtSignature`) | Guardar solo el texto | La documentación no aclara si la firma es obligatoria en texto plano; guardar todo es correcto en los dos casos y averiguarlo gastaría cuota |
| 2026-11-24 | 3 | Enviar una copia del historial y tocarlo solo tras un éxito | Añadir la pregunta y quitarla si falla | La regla "un fallo no deja la pregunta" sale de la estructura; no hay un "quitar" que olvidar en el `catch` |
| 2026-11-24 | 3 | Tiempo de espera de 60 s | 100 s por defecto | En una demo, 100 s parece un cuelgue; 60 s deja margen para que el modelo piense. Criterio de Claude, no un dato |
| 2026-11-24 | 3 | Devolver la pregunta a `TbxMensaje` si falla y la caja está vacía | Que el usuario la reescriba | Reintentar es un clic y sigue siendo decisión del usuario; no pisa lo que haya escrito después |
| 2026-11-24 | 3 | Sin clase de excepción propia; `CClienteGemini` lanza excepciones estándar con mensajes claros | `CErrorGemini` | El formulario trata todos los errores igual (muestra `ex.Message`); una clase propia hoy no evita ningún coste |
| 2026-11-24 | 3 | La Fase 3 la escribe Claude | "Alfredo guiado por Claude" | Autorización explícita de Alfredo ("aplícalo") |

## Riesgos globales
- **Filtrar la clave a GitHub:** solo se lee de la variable de entorno; antes de cada commit, revisar que no aparece en el diff.
- **Agotar la cuota (20 RPD) el día de la demo:** ese día solo 1 o 2 peticiones de prueba antes de presentar; la cuota se reinicia a medianoche, hora del Pacífico.
- **Sin internet o API caída en la demo:** probar la red antes de presentar; la Fase 3 garantiza un mensaje de error en vez de un cuelgue.
- **La API cambia (modelo, endpoint, límites):** son datos `[volátil]`; se vuelven a comprobar en la documentación oficial si algo deja de funcionar.
