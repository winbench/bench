param ([switch]$debug)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\common.lib.ps1"
. "$scriptsLib\config.lib.ps1"
. "$scriptsLib\fs.lib.ps1"

Set-Debugging $debug
$_ = Set-StopOnError $True

$libDir = Get-ConfigDir LibDir
$tempDir = Get-ConfigDir TempDir

Purge-Dir $libDir "Removing installed files ..."
Purge-Dir $tempDir "Removing temporary files ..."
