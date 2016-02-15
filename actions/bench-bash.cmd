@ECHO OFF
CALL "%~dp0\..\auto\env.cmd"
CD /D "%~dp0\.."
IF "_%1_" == "__" (
   ECHO.BENCH v%BENCH_VERSION% BASH
)
CALL "%BENCH_APPS%\git\bin\bash.exe" %*
