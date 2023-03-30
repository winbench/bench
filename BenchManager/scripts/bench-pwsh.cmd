@ECHO OFF
CALL "%~dp0\..\..\env.cmd"

IF "_%1_" == "__" (
  ECHO.BENCH v%BENCH_VERSION% PowerShell
  ECHO.
  pwsh -NoLogo -NoProfile -ExecutionPolicy Unrestricted -NoExit
  GOTO:EOF
)

pwsh -NoLogo -NoProfile -ExecutionPolicy Unrestricted %*
