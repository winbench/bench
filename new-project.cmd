@ECHO OFF
CALL "%~dp0\auto\env.cmd"
CALL "%~dp0\auto\lib\init.cmd"
runps New-Project %*
