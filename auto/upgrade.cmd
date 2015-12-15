@ECHO OFF
PUSHD "%~dp0"
CALL clean.cmd
CALL empty-cache.cmd
CALL install.cmd
POPD