param ([switch]$debug)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"

Set-Debugging $debug

$appDataDir = Get-ConfigDir AppDataDir
$downloadDir = Get-ConfigDir DownloadDir

Purge-Dir "$downloadDir" "Removing downloaded files ..."
Purge-Dir "$appDataDir\npm-cache" "Clearing NPM cache ..."
