@ECHO OFF
CALL "%~dp0\..\..\env.cmd"

IF "_%1_" == "__" (
  ECHO.BENCH v%BENCH_VERSION% PowerShell
  ECHO.
  powershell -NoLogo -NoProfile -ExecutionPolicy Unrestricted -NoExit
  GOTO:EOF
)

powershell -NoLogo -NoProfile -ExecutionPolicy Unrestricted %*
