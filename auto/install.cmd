@echo off
pushd %~dp0
call powershell -NoProfile -ExecutionPolicy RemoteSigned -Command "./download.ps1"
call powershell -NoProfile -ExecutionPolicy RemoteSigned -Command "./setup.ps1"
popd
pause