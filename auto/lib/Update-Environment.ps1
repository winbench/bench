param ([switch]$debug)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"
. "$scriptsLib\appconfig.lib.ps1"
. "$scriptsLib\env.lib.ps1"
. "$scriptsLib\adornment.lib.ps1"
. "$scriptsLib\launcher.lib.ps1"

Set-Debugging $debug

$libDir = Safe-Dir (Get-ConfigPathValue LibDir)

if (!(Test-Path $libDir)) { return }

Load-Environment
foreach ($name in $Script:apps) {
    Debug "Processing $name ($(App-Typ $name)) ..."
    if ((App-Typ $name) -eq "meta") {
        Register-AppPaths $name
        Register-AppEnvironment $name
        Execute-AppEnvironmentSetup $name
        Create-Launcher $name
    } elseif (Check-App $name) {
        if (((App-Typ $name) -ne "node-package") -and ((App-Typ $name) -ne "python-package")) {
            Register-AppPaths $name
        }
        Register-AppEnvironment $name
        Execute-AppEnvironmentSetup $name
        Setup-ExecutionProxies $name
        Create-Launcher $name
    } else {
        Write-Warning "App $name is activated but was not found."
    }
}
Debug "Processing apps finished."
Update-EnvironmentPath
Write-EnvironmentFile
Create-ActionLaunchers
