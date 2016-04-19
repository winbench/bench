param (
    [string]$scriptPath,
    $scriptArgs
)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"

Write-Host "Running custom script $([IO.Path]::GetFileName($scriptPath))..."

. $scriptPath @scriptArgs
