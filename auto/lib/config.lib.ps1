$Script:myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$Script:myDir\common.lib.ps1"
& "$Script:myDir\Load-ClrLibs.ps1"

[string]$Script:autoDir = Resolve-Path ([IO.Path]::Combine($myDir, ".."))
[string]$Script:rootDir = Resolve-Path ([IO.Path]::Combine($autoDir, ".."))
$Script:pathBackup = $env:PATH

$_ = Set-StopOnError $True

Debug "Loading configuration ..."
$global:BenchConfig = New-Object Mastersign.Bench.BenchConfiguration($Script:rootDir)

function Get-ConfigValue([string]$name) { return $global:BenchConfig.GetValue($name) }
function Get-ConfigBooleanValue([string]$name) { return $global:BenchConfig.GetBooleanValue($name) }
function Get-ConfigListValue([string]$name) { return $global:BenchConfig.GetStringListValue($name) }
function Get-AppConfigValue([string]$app, [string]$name) { return $global:BenchConfig.GetGroupValue($app, $name) }
function Get-AppConfigBooleanValue([string]$app, [string]$name) { return $global:BenchConfig.GetBooleanGroupValue($app, $name) }
function Get-AppConfigListValue([string]$app, [string]$name) { return $global:BenchConfig.GetStringListGroupValue($app, $name) }

function App-Typ([string]$name) { return $global:BenchConfig.Apps[$name].Typ }
function App-Version([string]$name) { return $global:BenchConfig.Apps[$name].Version }
function App-Dependencies([string]$name) { return $global:BenchConfig.Apps[$name].Dependencies }
function App-Url([string]$name) { return $global:BenchConfig.Apps[$name].Url }
function App-DownloadHeaders([string]$name) { return $global:BenchConfig.Apps[$name].DownloadHeaders }
function App-DownloadCookies([string]$name) { return $global:BenchConfig.Apps[$name].DownloadCookies }
function App-ResourceFile([string]$name) { return $global:BenchConfig.Apps[$name].ResourceFileName }
function App-ResourceArchive([string]$name) { return $global:BenchConfig.Apps[$name].ResourceArchiveName }
function App-ResourceArchiveTyp([string]$name) { return $global:BenchConfig.Apps[$name].ResourceArchiveTyp }
function App-ResourceArchivePath([string]$name) { return $global:BenchConfig.Apps[$name].ResourceArchivePath }
function App-Force([string]$name) { return $global:BenchConfig.Apps[$name].Force }
function App-PackageName([string]$name) { return $global:BenchConfig.Apps[$name].PackageName }
function App-Dir([string]$name) { return $global:BenchConfig.Apps[$name].Dir }
function App-Paths([string]$name) { return $global:BenchConfig.Apps[$name].Path }
function App-Exe([string]$name, [bool]$checkExist = $true) { return $global:BenchConfig.Apps[$name].Exe }
function App-Register([string]$name) { return $global:BenchConfig.Apps[$name].Register }
function App-Environment([string]$name) { return $global:BenchConfig.Apps[$name].Environment }
function App-AdornedExecutables([string]$name) { return $global:BenchConfig.Apps[$name].AdornedExecutables }
function App-RegistryKeys([string]$name) { return $global:BenchConfig.Apps[$name].RegistryKeys }
function App-Launcher([string]$name) { return $global:BenchConfig.Apps[$name].Launcher }
function App-LauncherExecutable([string]$name) { return $global:BenchConfig.Apps[$name].LauncherExecutable }
function App-LauncherArguments([string]$name) { return $global:BenchConfig.Apps[$name].LauncherArguments }
function App-LauncherIcon([string]$name) { return $global:BenchConfig.Apps[$name].LauncherIcon }
function App-SetupTestFile([string]$name) { return $global:BenchConfig.Apps[$name].SetupTestFile }
function Check-App([string]$name) { return $global:BenchConfig.Apps[$name].IsInstalled }
function App-CustomScript([string]$name, [string]$typ) { return $global:BenchConfig.Apps[$name].GetCustomScript($typ) }
function App-SetupResource([string]$name, [string]$relPath) { return $global:BenchConfig.Apps[$name].GetSetupResource($relPath) }

function App-Path([string]$name) {
    $path = $global:BenchConfig.Apps[$name].Path
    if ($path.Length -gt 0)
    {
        return $path[0]
    }
    else
    {
        return $null
    }
}
