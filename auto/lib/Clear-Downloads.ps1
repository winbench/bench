param ([switch]$debug)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\common.lib.ps1"
. "$scriptsLib\config.lib.ps1"
. "$scriptsLib\fs.lib.ps1"

Set-Debugging $debug
$_ = Set-StopOnError $True

$appDataDir = Get-ConfigDir AppDataDir
$downloadDir = Get-ConfigDir DownloadDir

Purge-Dir "$downloadDir" "Removing downloaded files ..."
Purge-Dir "$appDataDir\npm-cache" "Clearing NPM cache ..."
