@ECHO OFF
PUSHD "%~dp0\.."
ECHO Remove downloaded files ...
RMDIR /S /Q .\res\download
RMDIR /S /Q .\home\AppData\npm-cache
POPD