@ECHO OFF
CALL "%~dp0\..\..\env.cmd"

IF "_%1_" == "__" (
   ECHO.BENCH v%BENCH_VERSION% BASH
)

IF EXIST "%BENCH_APPS%\git\bin\bash.exe" (
  CALL "%BENCH_APPS%\git\bin\bash.exe" %*
) ELSE (
  ECHO.No Bash executable found. Is Git installed?
)
