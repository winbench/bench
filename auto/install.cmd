@ECHO OFF
CALL "%~dp0\init.cmd"
CALL runps Download-Apps
CALL runps Setup-Apps
CALL "%~dp0\env.cmd"
CD "%~dp0\.."
runps Shell
