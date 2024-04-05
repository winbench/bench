@ECHO OFF
CALL "%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe" "%~dp0\BenchSetup.csproj" -nologo -verbosity:minimal -t:Clean;PrepareResources;Compile -p:Configuration=Release
MOVE "%~dp0\obj\Release\BenchSetup.exe" "%~dp0\BenchSetup.exe"
RMDIR /S /Q "%~dp0\obj"
RMDIR /S /Q "%~dp0\bin"
PAUSE
