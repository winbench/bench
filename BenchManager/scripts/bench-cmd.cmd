@ECHO OFF
CALL "%~dp0\..\..\env.cmd"

IF "_%1_" == "__" (
   ECHO.BENCH v%BENCH_VERSION% CMD
   ECHO.
   CMD /D
   GOTO:EOF
)

CALL CMD /D /C %*
