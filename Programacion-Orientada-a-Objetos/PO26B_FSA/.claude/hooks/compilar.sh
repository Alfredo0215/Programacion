#!/bin/bash
# Hook PostToolUse (Write|Edit): compila el proyecto si el archivo editado es .cs.
# Si hay errores de compilación los manda a stderr con exit 2 para que Claude los vea.

# El JSON del hook llega por stdin; sin jq, buscamos file_path con grep.
if ! grep -qE '"file_path"[[:space:]]*:[[:space:]]*"[^"]*\.cs"'; then
  exit 0
fi

MSBUILD="C:/Program Files/Microsoft Visual Studio/18/Community/MSBuild/Current/Bin/MSBuild.exe"
[ -f "$MSBUILD" ] || exit 0

cd "$CLAUDE_PROJECT_DIR" 2>/dev/null || exit 0

SALIDA=$("$MSBUILD" "PO26B_FSA/POO26B_FSA.csproj" -p:Configuration=Debug -nologo -v:m -clp:ErrorsOnly 2>&1)
if [ $? -ne 0 ]; then
  echo "Error de compilación:" >&2
  echo "$SALIDA" | head -n 20 >&2
  exit 2
fi
exit 0
