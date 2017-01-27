@ECHO OFF
Setlocal EnableDelayedExpansion
SET SCRIPT=%~dp0..\lib\%1

SHIFT
SET "args="
:parse
IF "%~1" NEQ "" (
  REM check for spaces
  SET arg=%~1
  IF "!arg!"=="!arg: =!" (
    SET args=%args% !arg!
  ) ELSE (
    REM quote if necessary
    SET args=%args% '!arg!'
  )
  SHIFT
  GOTO :parse
)
IF DEFINED args SET "args=%args:~1%"

REM ECHO.%SCRIPT%
REM ECHO.%args%

powershell -NoLogo -NoProfile -ExecutionPolicy Unrestricted "& ('%SCRIPT%')" %args%