param ([switch]$debug)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"

trap { Write-TrapError $_ }
Set-Debugging $debug

$boostrapFile = "$Script:rootDir\bench-install.bat"
if (Test-Path $boostrapFile) {
    Debug "Deleting bootstrap file: $bootstrapFile"
    del $boostrapFile
}
