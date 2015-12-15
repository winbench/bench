@ECHo OFF
CALL "%~dp0\auto\env.cmd"
powershell -NoLogo -NoProfile -NoExit -ExecutionPolicy Unrestricted -Command "& { Write-Host 'Bench PowerShell' }"
