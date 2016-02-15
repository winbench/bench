@ECHO OFF
CALL "%~dp0\..\auto\env.cmd"
runps Clone-GitProject %*
