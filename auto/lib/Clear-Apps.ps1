param ([switch]$debug)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"

Set-Debugging $debug

$libDir = Get-ConfigDir LibDir
$tempDir = Get-ConfigDir TempDir

Purge-Dir $libDir "Removing installed files ..."
Purge-Dir $tempDir "Removing temporary files ..."
