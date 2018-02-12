@ECHO OFF
SetLocal

::
:: Bench Bootstrap file
::
:: https://winbench.org/guide/setup/
::

SET VERSION=0.18.0
SET TAG=v%VERSION%
SET ROOT=%~dp0
IF [%1] NEQ [] SET ROOT=%~dpnx1\
SET BENCH_ZIPURL=https://github.com/winbench/bench/releases/download/%TAG%/Bench.zip
SET BENCH_ZIPFILE=%ROOT%Bench.zip
SET BENCH_BOOTSTRAP_FILE=%~f0
SET BENCH_SUBFLDR=
SET BENCH_DIR=%ROOT%
SET BENCH_MAIN_SHORTCUT=%ROOT%Bench Dashboard.lnk

::
:: Preparations
::

PUSHD "%ROOT%"
ECHO.Starting Bench Installation...

::
:: Download, extract, initialize and setup Bench environment
::

:: Download setup archive
CALL :DOWNLOAD "%BENCH_ZIPURL%" "%BENCH_ZIPFILE%"
IF ERRORLEVEL 1 GOTO:ERROR_EXIT

:: Clean-up Bench folders
IF EXIST "%ROOT%\lib\" (
  ECHO.
  ECHO.Make sure, all programs in the Bench environment are closed.
  PAUSE
)
ECHO.Removing old Bench files ...
FOR %%d IN (actions, auto, res, tmp, lib\apps\bench\conemu, lib\applibs, cache\applibs) DO (
  IF EXIST "%ROOT%\%%d\" RMDIR /S /Q "%ROOT%\%%d"
)
IF EXIST "%BENCH_MAIN_SHORTCUT%" DEL "%BENCH_MAIN_SHORTCUT%"

:: Extract setup archive
CALL :EXTRACT "%BENCH_ZIPFILE%" "%BENCH_SUBFLDR%" "%BENCH_DIR%"
IF ERRORLEVEL 1 GOTO:ERROR_EXIT

:: Detect if ZIP-Extraction was successful
IF NOT EXIST "%ROOT%\auto\bin\bench.exe" (
  CALL:ERROR_MSG "Can not find Bench CLI."
  GOTO:ERROR_EXIT
)

:: Kick-off Bench setup
ECHO.Running initialization command ...
.\auto\bin\bench.exe --verbose manage initialize
IF ERRORLEVEL 1 (
  CALL:ERROR_MSG "Setup of Bench environment failed."
  GOTO:ERROR_EXIT
)

:: Detect if setup was successful
IF NOT EXIST "%BENCH_MAIN_SHORTCUT%" (
  CALL:ERROR_MSG "Setup of Bench environment incomplete."
  GOTO:ERROR_EXIT
)

::
:: Clean-Up
::

POPD

ECHO.Deleting 'bench.zip' ...
DEL "%BENCH_ZIPFILE%"

IF /I "%~dp0"=="%ROOT%" (
  ECHO.Deleting '%BENCH_BOOTSTRAP_FILE%' and exiting ...
  ECHO.A copy can always be found in the 'res' folder.
  :: Trick to exit the script before deleting it
  (GOTO) 2>nul & DEL "%BENCH_BOOTSTRAP_FILE%"
)
GOTO:EOF

:: Print a error message, wait for the user to press a key and then exit with error level 1
:ERROR_MSG
ECHO.
ECHO.%~1
ECHO.
PAUSE
EXIT /B 1

:: Restore working directory and exit with error level 1
:ERROR_EXIT
POPD
EXIT /B 1

:: ======== Procedures ======== ::

::
:: Download a file via HTTP(s)
::
:: Arguments:
::   %1 URL
::   %2 target file
::
:DOWNLOAD
IF EXIST "%~2" GOTO:EOF
ECHO.Downloading ZIP archive ...
ECHO.  %~1
PUSHD "%ROOT%"
powershell -NoLogo -NoProfile -C "try { (New-Object System.Net.WebClient).DownloadFile('%~1', '%~2') } catch { Write-Warning $_.Exception.InnerException.Message; exit 1 }"
IF ERRORLEVEL 1 (
  ECHO.Download failed.
  ECHO.  %~1
  PAUSE
  POPD
  EXIT /B 1
)
POPD
GOTO:EOF

::
:: Extract a ZIP archive into a target folder
::
:: Arguments:
::   %1 ZIP file
::   %2 sub-folder
::   %3 target dir
::
:EXTRACT
ECHO.Extracting ZIP archive ...
ECHO.  %~1
PUSHD "%~3"
powershell -NoLogo -NoProfile -C "try { $ws = New-Object -ComObject Shell.Application; $zip = $ws.NameSpace('%~1%~2'); $trg = $ws.NameSpace('%~3'); foreach($item in $zip.items()) { $trg.copyhere($item, 0x14) } } catch { Write-Warning $_.Exception.InnerException.Message; exit 1 }"
IF ERRORLEVEL 1 (
  ECHO.Extracting ZIP archive failed.
  ECHO.  %~1
  PAUSE
  POPD
  EXIT /B 1
)
POPD
GOTO:EOF
