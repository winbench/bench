@ECHO OFF
CALL "%~dp0\auto\env.cmd"
CD "%~dp0"
IF "_%1_" == "__" (
   ECHO.BENCH v%BENCH_VERSION% CMD
   ECHO.
)
CALL CMD /D %*
