@ECHO OFF
CALL "%~dp0\auto\env.cmd"
CALL powershell -NoLogo -NoProfile -NoExit -ExecutionPolicy Unrestricted "%~dp0\auto\Open-ProjectShell.ps1" %*
