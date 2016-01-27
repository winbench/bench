@ECHO OFF
CALL "%~dp0\auto\env.cmd"
CD "%~dp0"
CALL "%BENCH_APPS%\git\bin\bash.exe" %*
