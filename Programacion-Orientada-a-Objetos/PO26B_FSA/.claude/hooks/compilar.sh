#!/bin/bash
# Hook PostToolUse (Write|Edit): compila el proyecto si el archivo editado es .cs.
# Si el .cs es nuevo y no está en POO26B_FSA.csproj, avisa antes de compilar.
# Si hay errores los manda a stderr con exit 2 para que Claude los vea.

# El JSON del hook llega por stdin; sin jq, buscamos file_path con grep.
ENTRADA=$(cat)
RUTA=$(echo "$ENTRADA" | grep -oE '"file_path"[[:space:]]*:[[:space:]]*"[^"]*\.cs"' | head -n 1 | sed -E 's/.*:[[:space:]]*"(.*)"/\1/')
if [ -z "$RUTA" ]; then
  exit 0
fi

cd "$CLAUDE_PROJECT_DIR" 2>/dev/null || exit 0

# Los .cs del proyecto principal (no los de Tests) deben estar listados en el .csproj.
case "$RUTA" in
  *PO26B_FSA.Tests*) ;;
  *)
    ARCHIVO=$(echo "$RUTA" | sed -E 's/.*[\\/]//')
    if ! grep -qF "$ARCHIVO" "PO26B_FSA/POO26B_FSA.csproj"; then
      echo "Aviso: $ARCHIVO no está en POO26B_FSA.csproj. Agrégalo con <Compile Include=\"...\" /> o no compilará." >&2
      exit 2
    fi
    ;;
esac

# MSBuild se localiza con vswhere para no depender de la versión de Visual Studio.
VSWHERE="/c/Program Files (x86)/Microsoft Visual Studio/Installer/vswhere.exe"
MSBUILD=""
if [ -f "$VSWHERE" ]; then
  MSBUILD=$("$VSWHERE" -latest -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | head -n 1 | tr -d '\r')
fi
[ -f "$MSBUILD" ] || exit 0

SALIDA=$("$MSBUILD" "PO26B_FSA/POO26B_FSA.csproj" -p:Configuration=Debug -nologo -v:m -clp:ErrorsOnly 2>&1)
if [ $? -ne 0 ]; then
  echo "Error de compilación:" >&2
  echo "$SALIDA" | head -n 20 >&2
  exit 2
fi
exit 0
