@ECHO OFF
CALL "%~dp0\auto\env.cmd"
CD "%~dp0"
runps Shell 'BENCH ROOT' %*
