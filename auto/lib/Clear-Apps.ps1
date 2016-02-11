param ([switch]$debug)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"

trap { Write-TrapError $_ }
Set-Debugging $debug

$libDir = Get-ConfigPathValue LibDir
$tempDir = Get-ConfigPathValue TempDir

Purge-Dir $libDir "Removing installed files ..."
Purge-Dir $tempDir "Removing temporary files ..."
