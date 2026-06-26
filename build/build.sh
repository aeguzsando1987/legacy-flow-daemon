#!/usr/bin/env bash
#
# Build de HypertermFlow desde Ubuntu hacia Windows XP (.NET Framework 4.0 / x86).
# Compila las 5 partes, corre los tests del Core y empaqueta dist/ para la VM.
#
# Usa msbuild si esta disponible; si no, cae a xbuild (el que trae mono-devel).
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
CONFIG="${1:-Release}"

BUILD="xbuild"
if command -v msbuild >/dev/null 2>&1; then
  BUILD="msbuild"
fi
echo "== Toolchain de build: $BUILD  (config: $CONFIG) =="

build_proj() {
  echo "== Build: $(basename "$1") =="
  "$BUILD" "$1" /p:Configuration="$CONFIG" /verbosity:minimal
}

# Orden: dependencias primero.
build_proj "$ROOT/src/HypertermFlow.Core/HypertermFlow.Core.csproj"
build_proj "$ROOT/src/HypertermFlow.Win32/HypertermFlow.Win32.csproj"
build_proj "$ROOT/src/HypertermFlow.App/HypertermFlow.App.csproj"
build_proj "$ROOT/tools/DummyTarget/DummyTarget.csproj"
build_proj "$ROOT/tests/HypertermFlow.Core.Tests/HypertermFlow.Core.Tests.csproj"

echo "== Tests del Core (modo simulacion, en Ubuntu) =="
mono "$ROOT/tests/HypertermFlow.Core.Tests/bin/$CONFIG/HypertermFlow.Core.Tests.exe"

echo "== Empaquetando dist/ para la VM =="
DIST="$ROOT/dist"
rm -rf "$DIST"
mkdir -p "$DIST"
APPOUT="$ROOT/src/HypertermFlow.App/bin/$CONFIG"
cp "$APPOUT"/HypertermFlow.App.exe          "$DIST/"
cp "$APPOUT"/HypertermFlow.App.exe.config   "$DIST/" 2>/dev/null || true
cp "$APPOUT"/HypertermFlow.Core.dll         "$DIST/"
cp "$APPOUT"/HypertermFlow.Win32.dll        "$DIST/"
cp "$ROOT/tools/DummyTarget/bin/$CONFIG/DummyTarget.exe" "$DIST/"

echo ""
echo "== dist/ listo =="
ls -1 "$DIST"
echo ""
echo "Copia el contenido de dist/ a la VM Windows XP SP3 y ejecuta:"
echo "  1) DummyTarget.exe                       (blanco de clicks, pantalla completa)"
echo "  2) HypertermFlow.App.exe --mode=prod     (o --mode=sim para solo loguear)"
echo "  3) Pulsa F9  -> el daemon recorre y clickea las 5 zonas"
echo "  El log queda en hyperterm_flow.log junto al .exe"
