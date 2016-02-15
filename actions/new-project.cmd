@ECHO OFF
CALL "%~dp0\..\auto\env.cmd"
runps New-Project %*
