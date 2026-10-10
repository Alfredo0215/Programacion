---
name: compilar
description: Compila el proyecto WinForms con el MSBuild de Visual Studio y resume los errores de compilación.
disable-model-invocation: true
---

Compila el proyecto desde la raíz de `PO26B_FSA` con PowerShell:

```powershell
$msbuild = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe'
& $msbuild "PO26B_FSA\POO26B_FSA.csproj" /p:Configuration=Debug /nologo /v:m
```

`vswhere` localiza el MSBuild de la versión de Visual Studio instalada, así que no hay ruta fija. Si no devuelve nada, avisa que falta el componente MSBuild de Visual Studio en lugar de recurrir a `dotnet build`.

Al terminar:
- Si compila, dilo en una línea (el ejecutable queda en `PO26B_FSA\bin\Debug\PO26B_FSA.exe`).
- Si falla, lista cada error con archivo, línea y causa probable, en español. Si un archivo nuevo no compila, revisa primero que esté incluido en `POO26B_FSA.csproj`.
