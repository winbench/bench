@ECHO OFF
CALL "%~dp0\lib\init.cmd"
CALL runps Clear-Apps
CALL runps Download-Apps
CALL runps Setup-Apps
CALL "%~dp0\env.cmd"
CALL "%~dp0\lib\init.cmd"
runps Shell