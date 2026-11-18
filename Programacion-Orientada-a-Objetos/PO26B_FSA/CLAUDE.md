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

- No edites los `.Designer.cs` a mano. Los controles se agregan desde el diseñador de Visual Studio; cuando haga falta uno, explica los pasos a seguir en el diseñador. Cuando se pida editar, solo se toca la lógica en el `.cs`.
- Estética de la interfaz: pixel-art estilo Terraria.
- Navegación entre vistas (p. ej. menú principal y selector de mesas): un formulario abre u oculta a otro sin tocar sus controles. No añadir interfaces ni gestores de navegación hasta que haya un problema real que justifique su coste.

## Proyecto final: Acuario

Simulación de ecosistema (plantas, peces herbívoros y depredadores). Fases, estado y registro de decisiones en `PLAN.md`; léelo solo cuando la tarea sea del proyecto final.

- Vive en un panel de `MesaDePracticas4`, abierto con `AlternarPractica`.
- Movimiento con un solo `System.Windows.Forms.Timer`; nada de hilos por objeto ni `CheckForIllegalCrossThreadCalls` (los problemas de `CDron`).
- Las entidades son objetos normales, no controles: se dibujan en el `Paint` de un solo control del área. Las plantas también son entidades.
- Claude propone diseño y código con justificación (incluye "qué debes entender para defenderlo") y espera la aprobación de Alfredo antes de implementar. Los controles se siguen agregando en el diseñador.
- Los eventos de controles ya existentes pueden suscribirse en código (p. ej. `PbxAcuario.Paint` en el constructor) para no tocar el `.Designer.cs`.
- El estado actual y el siguiente paso están en la sección "Estado actual" de `PLAN.md`.
