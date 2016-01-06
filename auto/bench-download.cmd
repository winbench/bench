@ECHO OFF
CALL "%~dp0\init.cmd"
CALL runps Prepare-Config
CALL runps Download-Apps %*
CD "%~dp0\.."
PAUSE

