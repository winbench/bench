param ([switch]$debug)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"

Set-Debugging $debug

$boostrapFile = "$Script:rootDir\bench.bat"
if (Test-Path $boostrapFile) {
    Debug "Deleting bootstrap file: $bootstrapFile"
    del $boostrapFile
}
