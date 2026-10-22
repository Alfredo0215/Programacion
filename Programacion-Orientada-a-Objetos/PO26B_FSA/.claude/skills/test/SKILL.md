---
name: test
description: Ejecuta los tests MSTest del proyecto y resume los fallos.
disable-model-invocation: true
---

Ejecuta los tests desde la raíz de `PO26B_FSA` con PowerShell:

```powershell
dotnet test PO26B_FSA.Tests\PO26B_FSA.Tests.csproj
```

Al terminar:
- Si todos pasan, dilo en una línea con el total de pruebas.
- Si alguno falla, lista cada test con su mensaje y la causa probable, en español.

Reglas al escribir o corregir tests:
- `CHombre`/`CMujer` usan `Random`: prueba rangos, no valores exactos.
- Los tests nuevos van en español.
