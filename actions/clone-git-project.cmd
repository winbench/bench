@ECHO OFF
CALL "%~dp0\..\env.cmd"
runps Clone-GitProject.ps1 %*
