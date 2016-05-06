@ECHO OFF
CD /D "%~dp0\.."
CALL .\auto\env.cmd
.\auto\runps.cmd Shell 'BENCH ROOT' %*
