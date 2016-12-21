@ECHO OFF
SET ROOT=%~dp0

SET VERSION=0.13.3
SET TAG=v%VERSION%
SET BENCH_ZIPURL=https://github.com/mastersign/bench/releases/download/%TAG%/Bench.zip
SET BENCH_ZIPFILE=%ROOT%Bench.zip
SET BENCH_BOOTSTRAP_FILE=%~f0
SET BENCH_SUBFLDR=
SET BENCH_DIR=%ROOT%

PUSHD "%ROOT%"

CALL :DOWNLOAD "%BENCH_ZIPURL%" "%BENCH_ZIPFILE%"

ECHO Removing old Bench files ...
FOR %%d IN (actions, auto, res, tmp, lib\conemu) DO (
  IF EXIST "%ROOT%\%%d\" RMDIR /S /Q "%ROOT%\%%d"
)

CALL :EXTRACT "%BENCH_ZIPFILE%" "%BENCH_SUBFLDR%" "%BENCH_DIR%"

ECHO.Deleting ZIP files ...
DEL "%BENCH_ZIPFILE%"

ECHO.Running initialization script ...
CALL .\actions\bench-ctl.cmd initialize

POPD
REM Trick to exit the script before deleting it
(GOTO) 2>nul & DEL "%BENCH_BOOTSTRAP_FILE%"

REM ******* Procedures *******

:DOWNLOAD
REM Arguments
REM 1: URL
REM 2: target file
IF EXIST "%~2" GOTO:EOF
ECHO.Downloading ZIP archive ...
ECHO.  %~1
PUSHD "%ROOT%"
powershell -NoLogo -NoProfile -C "& { (New-Object System.Net.WebClient).DownloadFile('%~1', '%~2') }"
IF %ERRORLEVEL% NEQ 0 (
  ECHO.Download failed.
  ECHO.  %~1
  PAUSE
  EXIT /B 1
)
POPD
GOTO:EOF

:EXTRACT
REM Arguments
REM 1: ZIP file
REM 2: sub-folder
REM 3: target dir
ECHO.Extracting ZIP archive ...
ECHO.  %~1
PUSHD "%~3"
powershell -NoLogo -NoProfile -C "& { $ws = New-Object -ComObject Shell.Application; $zip = $ws.NameSpace('%~1%~2'); $trg = $ws.NameSpace('%~3'); foreach($item in $zip.items()) { $trg.copyhere($item, 0x14) } }"
IF %ERRORLEVEL% NEQ 0 (
  ECHO.Extracting ZIP archive failed.
  ECHO.  %~1
  PAUSE
  EXIT /B 1
)
POPD
GOTO:EOF
