@ECHO OFF
CALL "%~dp0\init.cmd"
CALL runps Clear-Apps
CALL runps Clear-Downloads
CALL runps Update-BenchRepo
CALL runps Download-Apps
CALL runps Setup-Apps
CALL "%~dp0\env.cmd"
CD "%~dp0\.."
runps Shell
