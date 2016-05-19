@ECHO OFF
SETLOCAL
SET SCRIPT=%~dp0\lib\%1.ps1

SHIFT
SET "args="
:parse
IF "%~1" NEQ "" (
  SET args=%args% '%~1'
  SHIFT
  GOTO :parse
)
IF DEFINED args SET "args=%args:~1%"

REM ECHO.%SCRIPT%
REM ECHO.%args%

powershell -NoLogo -NoProfile -ExecutionPolicy Unrestricted "& ('%SCRIPT%')" %args%
