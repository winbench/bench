@ECHO OFF
CALL "%~dp0\auto\env.cmd"
powershell -NoLogo -NoProfile -ExecutionPolicy Unrestricted "%~dp0\auto\New-Project.ps1"
