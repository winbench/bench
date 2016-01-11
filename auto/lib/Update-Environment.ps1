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
        if ((App-Typ $name) -ne "node-package") {
            Register-AppPaths $name
        }
        Register-AppEnvironment $name
        Run-AppEnvironmentSetup $name
    } else {
        if ((App-Typ $name) -ne "meta") {
            Write-Warning "App $name is activated but was not found."
        }
    }
}
Update-EnvironmentPath
Write-EnvironmentFile
