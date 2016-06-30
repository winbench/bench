param (
    [string]$scriptPath,
    [array]$scriptArgs
)

if (!$Script:cfg)
{
	$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
	. "$scriptsLib\bench.lib.ps1"
	. "$scriptsLib\reg.lib.ps1"
}

Write-Host "Running custom script $([IO.Path]::GetFileName($scriptPath))..."

$global:ErrorActionPreference = "Stop"
. $scriptPath @scriptArgs
