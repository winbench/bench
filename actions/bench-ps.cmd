@ECHO OFF
CALL "%~dp0\..\auto\env.cmd"
CD /D "%~dp0\.."
runps Shell 'BENCH ROOT' %*
