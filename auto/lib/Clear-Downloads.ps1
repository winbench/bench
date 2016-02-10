param ([switch]$debug)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"

trap { Write-TrapError $_ }
Set-Debugging $debug

$appDataDir = Get-ConfigPathValue AppDataDir
$downloadDir = Get-ConfigPathValue DownloadDir

Purge-Dir "$downloadDir" "Removing downloaded files ..."
Purge-Dir "$appDataDir\npm-cache" "Clearing NPM cache ..."
