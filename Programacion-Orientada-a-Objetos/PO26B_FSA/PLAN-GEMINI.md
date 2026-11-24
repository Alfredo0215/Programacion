# Plan: Chat con Gemini en el menú principal

Formato: planificar-fases v1 · módulos: software

## Estado actual
- Fase en curso: 0, Validación · Última actualización: 2026-11-23
- **Siguiente acción:**
  1. Crear una clave de API de Gemini y guardarla en la variable de entorno de usuario `GEMINI_API_KEY`.
  2. Hacer la petición de prueba de la Fase 0 desde PowerShell y anotar el JSON real en "Notas".

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
- [Validar en Fase 0] La conexión HTTPS funciona en 4.7.2 en Windows 11 sin configurar TLS a mano. → Si falla, se añade la configuración de TLS al arrancar.
- [Firme] La clave vive en la variable de entorno de usuario `GEMINI_API_KEY` y nunca se escribe en el repo.
**Tareas:**
1. Crear la clave y guardarla en `GEMINI_API_KEY` (variable de usuario de Windows).
2. Petición de prueba desde PowerShell leyendo `$env:GEMINI_API_KEY`; guardar en "Notas" un ejemplo del JSON enviado y del recibido (sin la clave).
3. En Visual Studio: agregar la referencia `System.Net.Http` y el paquete NuGet `Newtonsoft.Json` a `POO26B_FSA.csproj`.
4. Compilar.
**Hecho cuando:** PowerShell imprime la respuesta de Gemini; el proyecto compila con las dos referencias; "Notas" tiene endpoint, modelo, límites (con fuente y fecha) y el ejemplo de JSON.
**Verificación:** manual (petición en PowerShell) y automática (compilación).
**Fuera de alcance:** cualquier cambio en `DlgPrincipal` o código C# de la función.
**Riesgos:** un supuesto resulta falso → se registra y se reajustan las fases en el punto de revisión. La clave aparece en un archivo del repo → se revoca y se crea otra.
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
**Objetivo:** que `BtnEnviar` mande el texto de la caja a Gemini y muestre la respuesta, sin historial y sin congelar la ventana. · **Entregable demostrable:** se escribe una pregunta, se pulsa el botón, la ventana sigue respondiendo mientras espera y aparece la respuesta en la conversación. · **Hecho cuando:** pregunta y respuesta se ven en el control; durante la espera se puede mover la ventana y el botón está desactivado; un mensaje vacío no se envía.

## Fase 3: Historial y errores  [ ]  (esqueleto: depende de [Validar en Fase 0])
**Objetivo:** mantener una conversación de hasta 10 elementos y manejar los fallos sin romperla. · **Entregable demostrable:** una conversación de varios turnos en la que Gemini recuerda lo anterior, y un mensaje claro si no hay red, la clave falta o se agota la cuota. · **Hecho cuando:** los tests del historial pasan (recorte por pares al pasar de 10, la lista nunca empieza con `model`, una petición fallida no deja la pregunta en el historial); en la app, Gemini responde usando un dato dicho dos turnos antes; con la red desconectada aparece el error y la siguiente pregunta funciona.

---

## Después del MVP
- Guardar la conversación entre sesiones.
- Mostrar la respuesta a medida que llega (*streaming*).
- Botón para borrar la conversación.

## Preguntas abiertas
| Pregunta | Supuesto mientras tanto | Cuándo se resuelve |
|---|---|---|
| ¿Una clase o dos (conversación + cliente HTTP)? | Dos, solo si separar aporta algo real. Criterio: se separan si el historial tiene reglas que deben probarse sin red (recorte por pares, descarte de la pregunta si la petición falla). Si no, una sola | Propuesta de la Fase 2 |

## Registro de decisiones
| Fecha | Fase | Decisión | Alternativa descartada | Motivo |
|---|---|---|---|---|
| 2026-11-23 | — | Plan propio (`PLAN-GEMINI.md`) | Meterlo en `PLAN.md` o `PLAN-ESTILO.md` | No es del acuario y mezcla aspecto y comportamiento |
| 2026-11-23 | — | REST directo con `HttpClient` | SDK de Google | El objetivo es ver la petición y la respuesta |
| 2026-11-23 | 1 | Conversación entre saludo y caja; caja al fondo | Reemplazar el saludo con la respuesta | Entrada fija abajo y lectura en orden de tiempo |
| 2026-11-23 | 1 | Reducir la caja (~50 px) con scroll | Agrandar el formulario | No depende de la resolución ni del escalado de la pantalla de demo |
| 2026-11-23 | 1 | Enter baja de línea (`AcceptsReturn = true`); envía `BtnEnviar` | Enter envía, Shift+Enter baja | Sin código extra en `KeyDown` |
| 2026-11-23 | 3 | Historial de 10 elementos (5 pares), en memoria, recortado por pares | Recortar de uno en uno | Mantiene la alternancia `user`/`model` |
| 2026-11-23 | 2 | `BtnEnviar` desactivado durante la petición | Dejarlo activo | Dos peticiones a la vez romperían el historial |
| 2026-11-23 | 0 | El modelo se elige tras consultar cuotas y límites, con criterio fijado antes | Elegir el modelo de antemano | Los límites del plan gratuito cambian por modelo y deciden si alcanza para probar y hacer la demo |
| 2026-11-23 | 2 | Dos clases solo si aportan algo real (criterio en "Preguntas abiertas") | Decidirlo por buenas prácticas en abstracto | Evitar sobre-ingeniería |
| 2026-11-23 | 0 | Clave en `GEMINI_API_KEY`; la demo la hace Alfredo | Clave en un archivo de configuración | El repo es público en GitHub |

## Riesgos globales
- **Filtrar la clave a GitHub:** solo se lee de la variable de entorno; antes de cada commit, revisar que no aparece en el diff.
- **Sin internet o API caída en la demo:** probar la red antes de presentar; la Fase 3 garantiza un mensaje de error en vez de un cuelgue.
- **La API cambia (modelo, endpoint, límites):** son datos `[volátil]`; se vuelven a comprobar en la documentación oficial si algo deja de funcionar.
