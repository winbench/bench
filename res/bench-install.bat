@ECHO OFF
SET ROOT=%~dp0

SET VERSION=0.10.3
SET BENCH_ZIPURL=https://github.com/mastersign/bench/archive/v%VERSION%.zip
SET BENCH_ZIPFILE=%ROOT%bench.zip
SET BENCH_SUBFLDR=\bench-%VERSION%
SET BENCH_DIR=%ROOT%
SET BENCHMGR_ZIPURL=https://github.com/mastersign/bench-manager/releases/download/v%VERSION%/BenchManager.zip
SET BENCHMGR_ZIPFILE=%ROOT%BenchManager.zip
SET BENCHMGR_SUBFLDR=
SET BENCHMGR_DIR=%ROOT%auto\bin

PUSHD "%ROOT%"

CALL :DOWNLOAD "%BENCH_ZIPURL%" "%BENCH_ZIPFILE%"
CALL :DOWNLOAD "%BENCHMGR_ZIPURL%" "%BENCHMGR_ZIPFILE%"

CALL :EXTRACT "%BENCH_ZIPFILE%" "%BENCH_SUBFLDR%" "%BENCH_DIR%"
IF NOT EXIST "%BENCHMGR_DIR%\" MKDIR "%BENCHMGR_DIR%"
CALL :EXTRACT "%BENCHMGR_ZIPFILE%" "%BENCHMGR_SUBFLDR%" "%BENCHMGR_DIR%"

ECHO.Deleting ZIP files...
DEL "%BENCH_ZIPFILE%"
DEL "%BENCHMGR_ZIPFILE%"

ECHO.Running initialization script ...
.\actions\bench-ctl.cmd initialize

POPD
EXIT /B 0

:DOWNLOAD
REM 1: URL, 2: target file
IF EXIST "%~2" GOTO:EOF
ECHO.Downloading ZIP archive...
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
REM 1: ZIP file, 2: sub-folder, 3: target dir
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