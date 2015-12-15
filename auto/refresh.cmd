@ECHO OFF
PUSHD "%~dp0"
CALL clean.cmd
CALL install.cmd
POPD