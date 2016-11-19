@ECHO OFF
CD /D "%~dp0\.."
CALL .\env.cmd
.\auto\runps.cmd Shell.ps1 'BENCH ROOT' %*
