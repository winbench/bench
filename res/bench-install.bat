REM @ECHO OFF
SET ROOT=%~dp0

SET BENCH_ZIPURL=https://github.com/mastersign/bench/archive/bench-lib.zip
SET BENCH_ZIPFILE=%ROOT%bench.zip
SET BENCH_SUBFLDR=\bench-bench-lib
SET BENCH_DIR=%ROOT%
SET BENCHMGR_ZIPURL=http://192.168.0.17:8080/BenchManager.zip
SET BENCHMGR_ZIPFILE=%ROOT%BenchManager.zip
SET BENCHMGR_SUBFLDR=
SET BENCHMGR_DIR=%ROOT%auto\bin

PUSHD "%ROOT%"

CALL :DOWNLOAD "%BENCH_ZIPURL%" "%BENCH_ZIPFILE%"
CALL :DOWNLOAD "%BENCHMGR_ZIPURL%" "%BENCHMGR_ZIPFILE%"

CALL :EXTRACT "%BENCH_ZIPFILE%" "%BENCH_SUBFLDR%" "%BENCH_DIR%"
IF NOT EXIST "%BENCHMGR_DIR%\" MKDIR "%BENCHMGR_DIR%"
CALL :EXTRACT "%BENCHMGR_ZIPFILE%" "%BENCHMGR_SUBFLDR%" "%BENCHMGR_DIR%"

ECHO.Deleting ZIP archive(s) ...
DEL "%BENCH_ZIPFILE%"
DEL "%BENCHMGR_ZIPFILE%"

ECHO.Running setup script ...
.\actions\bench-ctl.cmd setup

POPD
GOTO:EOF

:DOWNLOAD
REM 1: URL, 2: target file
ECHO.Downloading ZIP archive...
ECHO.  %~1
PUSHD "%ROOT%"
powershell -NoLogo -NoProfile -C "& { (New-Object System.Net.WebClient).DownloadFile('%~1', '%~2') }"
IF %ERRORLEVEL% NEQ 0 (
  ECHO.Download failed:
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
powershell -NoLogo -NoProfile -C "& { $ws = New-Object -ComObject Shell.Application; $zip = $ws.NameSpace('%~1%~2'); $trg = $ws.NameSpace('%~3'); foreach($item in $zip.items()) { $trg.copyhere($item) } }"
IF %ERRORLEVEL% NEQ 0 (
  ECHO.Extracting ZIP archive failed.
  PAUSE
  EXIT /B 1
)
POPD