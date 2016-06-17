@ECHO OFF
CD /D "%~dp0\.."
CALL .\env.cmd
.\auto\runps.cmd Shell 'BENCH ROOT' %*
