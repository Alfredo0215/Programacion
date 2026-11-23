# CLAUDE.md

Guía para Claude Code al trabajar en este repositorio. Las preferencias generales del usuario (cómo corregir, sugerir en vez de escribir, explicar) están en su `CLAUDE.md` de usuario y también aplican aquí.

## Proyecto

Aplicación Windows Forms en C# (.NET Framework 4.7.2) para la materia Programación Orientada a Objetos (26B). Es un proyecto de aprendizaje.

- La raíz de git es `C:\Ingeniería\Repositorio-Github`, un monorepo con proyectos de Python no relacionados. Este proyecto vive en `Programacion-Orientada-a-Objetos/PO26B_FSA`.
- El archivo de proyecto se llama `POO26B_FSA.csproj` (con doble O), pero el namespace y el ensamblado son `PO26B_FSA`.
- El `.csproj` es del estilo antiguo y lista los archivos de forma explícita: todo `.cs` o `.resx` nuevo hay que agregarlo a mano al `.csproj`, o no compila.

## Compilar

`msbuild` no está en el PATH y `dotnet build` no es fiable con este `.csproj`. Usa el MSBuild de Visual Studio (o el skill `/compilar`):

```powershell
$msbuild = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe'
& $msbuild "PO26B_FSA\POO26B_FSA.csproj" /p:Configuration=Debug /nologo /v:m
```

`vswhere` localiza el MSBuild de la versión de Visual Studio instalada, así que no hay ruta fija que se rompa al actualizar. El hook y el skill `/compilar` usan el mismo método.

Un hook compila automáticamente tras cada edición de un `.cs` del proyecto principal y muestra los errores. No hay linter. En un cambio que abarca varios archivos, el hook muestra errores de los pasos intermedios (p. ej. una firma ya cambiada y sus llamadas aún no); no los persigas uno por uno: la verificación válida es compilar al terminar todas las ediciones.

## Tests

Proyecto MSTest en `PO26B_FSA.Tests` (`net472`, estilo SDK, no necesita listar archivos en el `.csproj`). Se ejecuta desde la raíz de `PO26B_FSA`:

```powershell
dotnet test PO26B_FSA.Tests\PO26B_FSA.Tests.csproj
```

- Las clases `C*` son `internal`; los tests las ven por `InternalsVisibleTo` en `Properties\AssemblyInfo.cs`.
- `CHombre` / `CMujer` usan `Random`; prueba rangos, no valores exactos.
- Los tests nuevos también van en español.
- Para aislar un caso de un pez (p. ej. morir de viejo sin morir de hambre), usa la subclase `CPezDePrueba` de `CEcosistemaTests.cs`, que recibe energía y edad máxima a elección.

## Convenciones

- Identificadores y comentarios en español.
- Clases con prefijo `C` (`CPersona`, `CHombre`); las de celular no lo llevan (`Celular`, `CelularAndroid`).
- Getters como métodos estilo Java (`GetSistemaOperativo()`), no propiedades.
- Controles con prefijo de 3 letras (`Btn`, `Lbl`, `Pnl`, `Pbx`, `Tbx`) y handlers `Control_Evento` (`LblMesa2_Click`).
- Excepciones aceptadas por Alfredo en `MesaDePracticas4`, que no se proponen renombrar: `TimerAcuario` (sin prefijo de 3 letras) y `BtnPractica2_Click`, conectado a `BtnProyectoF`. Los controles nuevos siguen la convención.
- Llaves en su propia línea, 4 espacios de indentación.
- Secciones de clase con banner `//----` / `// Atributos.` / `//----`.
- Los namespaces son `PO26B_FSA`, `PO26B_FSA.Mesas_De_Practicas` (carpeta `Mesas-De-Practicas`) y `PO26B_FSA.Acuario` (carpeta `Acuario`, clases del proyecto final).

## Interfaz y diseñador

- El diseñador no escribe en el `.Designer.cs` una propiedad con su valor por defecto (p. ej. `Visible = true`): para verificar un cambio así, busca que haya desaparecido la línea con el valor anterior.
- No edites los `.Designer.cs` a mano. Los controles se agregan desde el diseñador de Visual Studio; cuando haga falta uno, explica los pasos a seguir en el diseñador. Cuando se pida editar, solo se toca la lógica en el `.cs`.
- Estética: todo el proyecto (menú, mesas 1-3 y `MesaDePracticas4`, acuario incluido) usa el estilo de la sección "Estilo chat". El pixel-art estilo Terraria está retirado: no lo propongas.
- Un control con `Dock = Fill` debe quedar al frente de sus hermanos acoplados (clic derecho → Traer al frente); si no, los controles `Top`/`Bottom` lo tapan y su `ClientSize` incluye la zona tapada. En el `.Designer.cs`, su `Controls.Add` aparece primero. En general, WinForms acopla en orden z inverso: el control más al fondo se acopla primero (queda arriba con `Top`, abajo con `Bottom`). El orden se ve y se cambia en Ver → Otras ventanas → Esquema del documento (arriba = frente).
- `TextBox`, `ComboBox` y `ListBox` son controles nativos y no disparan `Paint`: para darles bordes o esquinas propias (p. ej. `UIHelper.RoundControl`), se meten en un `Panel` y se decora el panel.
- Cambiar el `Font` de un formulario con `AutoScaleMode = Font` reescala posiciones y tamaños de sus controles (factor = `AutoScaleDimensions` nuevo / viejo); tras cambiarlo, revisa los tamaños. Y `MinimumSize`/`MaximumSize` ganan a `Dock`: un control con `Fill` y tamaño fijo se sale de su contenedor.
- Navegación entre vistas (p. ej. el menú y una mesa): un formulario abre u oculta a otro sin tocar sus controles. No añadir interfaces ni gestores de navegación hasta que haya un problema real que justifique su coste.

## Estilo chat

Inspirado en la interfaz de chat de Claude.ai. Es software de estudio, sin publicar: se pueden usar nombres de productos (p. ej. los modelos de Claude como inspiración de cada mesa); no se copian logos ni imágenes de marca. Referencia visual: `docs/referencia-estilo-chat.png`; si choca con esta lista, manda la lista.

En todas las pantallas:
- Paleta: fondo `#F5F4EE`, paneles laterales `#EDEBE3`, bordes `#DDD9CE`, texto `#2B2A27`, texto secundario `#7A776E`, acento `#C96442`.
- Cada mesa tiene una variante inspirada en un modelo: misma base, cambian el acento y la densidad. Mesa 1 Haiku (salvia `#587759`, paneles sin relleno), Mesa 2 Sonnet (terracota `#C96442`, el estilo base del menú), Mesa 3 Opus (ciruela `#6B4E7A`, títulos en Georgia), Mesa 4/acuario Fable (ocre `#8C6420`). Detalle y estado en `PLAN-ESTILO.md`.
- Contraste: texto blanco sobre un acento ≥ 4.5:1; el terracota (3.90) solo va en texto de 14 pt negrita o más, en bordes o en elementos que no son texto.
- Fuentes: Georgia en el logo y el saludo, Segoe UI en el resto. Andy ya no se usa.
- Diseño plano: sin degradados, transparencias ni brillos. Botones: `Button` con `FlatStyle = Flat` y `FlatAppearance` en la paleta (no FontAwesome.Sharp).
- Esquinas redondeadas: `UIHelper.RoundControl`. Con borde visible (chips, cajas): píldora, diámetro = alto. En botones rellenos sin borde visible: diámetro 5 px, porque `Region` no suaviza y una curva larga se ve dentada. Si el control ya tiene borde (`FlatAppearance.BorderSize`), se pone a 0 para no dibujar dos.
- Un `Button` con `FlatStyle = Flat` necesita `BackColor` propio: si se deja `UseVisualStyleBackColor = true`, Windows lo pinta en su gris y el Grep de colores no lo detecta (la propiedad no aparece en el `.Designer.cs`).

Solo en el menú principal (`DlgPrincipal`): una sola vista (menú y selector de mesas juntos): barra lateral izquierda con las 4 mesas como lista de conversaciones; área principal con logo "POO26B" en texto, saludo "¿Qué practicamos hoy?", caja de mensaje (decorativa por ahora) y chips que abren mesas. Sin botón "Atrás" ni Esc; la app se cierra con la X. Las mesas no copian esta distribución.

- Cambia la apariencia, no el comportamiento. Excepciones ya decididas: la vista única del menú (Fase 1 de `PLAN-ESTILO.md`) y el logo fijo sin animación (Fase 5). Se conserva `AbrirMesa` (al cerrar una mesa vuelve el menú).
- Acuario: primero migra el marco (panel, botones, etiquetas). Los colores de peces y plantas en el `Paint` se deciden en su propia fase.
- Restos de Terraria (no tomarlos como referencia; se eliminan en la Fase 9 de `PLAN-ESTILO.md`): `FilaMesa` entera, `UIHelper.AplicarEstiloTerraria` e `Interpolar`, la fuente Andy (instalada en Windows, se quita cambiando `Font` en el diseñador), `TerrariaBackground` y los PNG que liste el inventario.
- Fases y avance: `PLAN-ESTILO.md`; léelo solo en tareas de estilo. `PLAN.md` decide el comportamiento del acuario y `PLAN-ESTILO.md` su aspecto; una fase no mezcla las dos cosas.

## Proyecto final: Acuario

Simulación de ecosistema (plantas, peces herbívoros y depredadores). Fases, estado y registro de decisiones en `PLAN.md`; léelo solo cuando la tarea sea del proyecto final.

- Vive en un panel de `MesaDePracticas4`, abierto con `AlternarPractica`.
- Movimiento con un solo `System.Windows.Forms.Timer`; nada de hilos por objeto ni `CheckForIllegalCrossThreadCalls` (los problemas de `CDron`).
- El tiempo de la simulación se mide y se muestra en ticks, nunca en segundos: `Interval` es un mínimo, no una duración garantizada. Parámetros y criterios (p. ej. la Fase 7) van en ticks.
- Las entidades son objetos normales, no controles: se dibujan en el `Paint` de un solo control del área. Las plantas también son entidades.
- Claude propone diseño y código con justificación (incluye "qué debes entender para defenderlo") y espera la aprobación de Alfredo antes de implementar. Los controles se siguen agregando en el diseñador.
- Los eventos de controles ya existentes pueden suscribirse en código (p. ej. `PbxAcuario.Paint` en el constructor) para no tocar el `.Designer.cs`.
- El estado actual y el siguiente paso están en la sección "Estado actual" de `PLAN.md`.
