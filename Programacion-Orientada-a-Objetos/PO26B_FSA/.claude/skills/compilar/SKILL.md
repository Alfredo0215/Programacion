---
name: compilar
description: Compila el proyecto WinForms con el MSBuild de Visual Studio y resume los errores de compilación.
disable-model-invocation: true
---

Compila el proyecto desde la raíz de `PO26B_FSA` con PowerShell:

```powershell
& "C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" "PO26B_FSA\POO26B_FSA.csproj" /p:Configuration=Debug /nologo /v:m
```

Si esa ruta no existe, búscala con
`& "C:\Program Files (x86)\Microsoft Visual Studio\Installer\vswhere.exe" -all -products * -find "MSBuild\**\Bin\MSBuild.exe"`.

Al terminar:
- Si compila, dilo en una línea (el ejecutable queda en `PO26B_FSA\bin\Debug\PO26B_FSA.exe`).
- Si falla, lista cada error con archivo, línea y causa probable, en español. Si un archivo nuevo no compila, revisa primero que esté incluido en `POO26B_FSA.csproj`.
