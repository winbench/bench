@ECHO OFF
PUSHD "%~dp0\.."
ECHO Removing installed files ...
RMDIR /S /Q .\lib
RMDIR /S /Q .\tmp
POPD