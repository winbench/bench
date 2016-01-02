param ([switch]$debug)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"
. "$scriptsLib\appconfig.lib.ps1"
. "$scriptsLib\env.lib.ps1"

Set-Debugging $debug

$libDir = Safe-Dir (Get-ConfigPathValue LibDir)

if (!(Test-Path $libDir)) { return }

Load-Environment
foreach ($name in $Script:apps) {
    if (Check-App $name) {
        Register-AppPaths $name
    } else {
        Write-Warning "App $name is activated but was not found."
    }
}
Update-EnvironmentPath
Write-EnvironmentFile
