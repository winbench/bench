@ECHO OFF
SETLOCAL EnableDelayedExpansion

SET ROOT_DIR=%~dp0\..
SET AUTO_DIR=%ROOT_DIR%\auto
SET /P BENCH_VERSION=<"%ROOT_DIR%\res\version.txt"
SET RUN_SHELL=0
SET VERBOSE=0

CD /D "%ROOT_DIR%"

IF "_%1_" == "__" GOTO:interactive
IF "_%1_" == "_/?_" GOTO:help

SET i=0
FOR %%a IN (download, setup, refresh, renew, upgrade, update-env) DO (
  SET /A i+=1
  SET arg[!i!]=%%a
)
FOR /L %%i IN (1,1,6) DO (
  IF /I "%1" == "!arg[%%i]!" (
    SET ACTION=!arg[%%i]!
    GOTO:silent
  )
)
ECHO.Unknown action: %1
ECHO.
GOTO:help

GOTO:EOF

:switch_verbose
  IF %VERBOSE% == 0 (
    SET VERBOSE=1
  ) ELSE (
    SET VERBOSE=0
  )
GOTO:interactive

:help
  ECHO.Bench Control - Usage
  ECHO.---------------------
  ECHO.bench-clt
  ECHO.  Interactive mode
  ECHO.bench-ctl ^<action^>
  ECHO.  Run one of the following actions:
  ECHO.  download, setup, refresh, renew, upgrade, update-env
  ECHO.bench-ctl /?
  ECHO.  Display this help
GOTO:EOF

:silent
  SET SILENT=1
  2>NUL CALL :action_%ACTION% %*
  IF ERRORLEVEL 1 (
    ECHO.Unknown action: %ACTION%
    ECHO.
    CALL :help
  )
GOTO:EOF

:interactive
  SET SILENT=0
  CLS
  ECHO.Bench Control v%BENCH_VERSION%
  ECHO.---------------------
  IF %VERBOSE% == 1 ECHO.(Verbose)
  ECHO.
  ECHO.The following actions are available:
  ECHO.  E: Update environment after moving Bench (update-env)
  ECHO.  S: Download and install selected apps (setup)
  ECHO.  D: Download missing app resources (download)
  ECHO.  R: Download and reinstall selected apps (reinstall)
  ECHO.  N: Redownload and reinstall selected apps (renew)
  ECHO.  U: Update Bench via Git reset+pull and renew (upgrade)
  IF %VERBOSE% == 0 (
    ECHO.  V: Activate verbose messages
  ) ELSE (
    ECHO.  V: Deactivate verbose messages
  )
  ECHO.  Q: Quit
  ECHO.
  CHOICE /C DSRNUEVQ /M "Select Action"
  IF ERRORLEVEL 8 SET ACTION=quit
  IF ERRORLEVEL 7 GOTO:switch_verbose
  IF ERRORLEVEL 6 SET ACTION=update-env
  IF ERRORLEVEL 5 SET ACTION=upgrade
  IF ERRORLEVEL 4 SET ACTION=renew
  IF ERRORLEVEL 3 SET ACTION=reinstall
  IF ERRORLEVEL 2 SET ACTION=setup
  IF ERRORLEVEL 1 SET ACTION=download
  IF "%ACTION%" == "quit" GOTO:EOF
  ECHO.
  CLS
  2>NUL CALL :action_%ACTION% %*
  PAUSE
  IF %RUN_SHELL% == 1 (
    runps Shell
  )
GOTO:EOF

:action_update-env
  IF %SILENT% == 0 (
    ECHO.Update Bench environment paths and lauchners...
    ECHO.
  )
  CALL "%AUTO_DIR%\init.cmd"
  CALL :runsetup update-env
GOTO:EOF

:action_setup
  IF %SILENT% == 0 (
    ECHO.Download and install selected apps...
    ECHO.
  )
  CALL "%AUTO_DIR%\init.cmd"
  CALL :runsetup setup
  CD /D "%ROOT_DIR%"
  CALL "%AUTO_DIR%\env.cmd"
  SET RUN_SHELL=1
GOTO:EOF

:action_download
  IF %SILENT% == 0 (
    ECHO.Download missing app resources...
    ECHO.
  )
  CALL "%AUTO_DIR%\init.cmd"
  CALL :runsetup download
  CD /D "%ROOT_DIR%"
GOTO:EOF

:action_reinstall
  IF %SILENT% == 0 (
    ECHO.Download and reinstall selected apps...
    ECHO.
    PAUSE
  )
  CALL "%AUTO_DIR%\init.cmd"
  CALL :runsetup reinstall
  CD /D "%ROOT_DIR%"
  CALL "%AUTO_DIR%\env.cmd"
  SET RUN_SHELL=1
GOTO:EOF

:action_renew
  IF %SILENT% == 0 (
    ECHO.Redownload and reinstall selected apps...
    ECHO.
    PAUSE
  )
  CALL "%AUTO_DIR%\init.cmd"
  CALL :runsetup renew
  CD /D "%ROOT_DIR%"
  CALL "%AUTO_DIR%\env.cmd"
  SET RUN_SHELL=1
GOTO:EOF

:action_upgrade
  IF %SILENT% == 0 (
    ECHO.Update Bench via Git reset+pull, redownload, and reinstall selected apps...
    ECHO.
    PAUSE
  )
  CALL "%AUTO_DIR%\init.cmd"
  CALL :runps Update-BenchRepo
  CALL :runsetup renew
  CD /D "%ROOT_DIR%"
  CALL "%AUTO_DIR%\env.cmd"
  SET RUN_SHELL=1
GOTO:EOF

:runsetup
  IF %VERBOSE% == 1 (
    CALL runps Setup-Bench -Action %1 -WithInfo
  ) ELSE (
    CALL runps Setup-Bench -Action %1
  )
GOTO:EOF

:runps
  IF %VERBOSE% == 1 (
    CALL runps %1 -debug "%~2"
  ) ELSE (
    CALL runps %1 "%~2"
  )
GOTO:EOF
