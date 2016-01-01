@ECHO OFF
CALL "%~dp0\init.cmd"
CALL runps Prepare-Config
CALL runps Download-Apps
CALL runps Setup-Apps
CALL "%~dp0\env.cmd"
CD "%~dp0\.."
PAUSE
runps Shell
