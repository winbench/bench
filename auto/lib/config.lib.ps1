$Script:myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$Script:myDir\common.lib.ps1"

[string]$Script:autoDir = Resolve-Path ([IO.Path]::Combine($myDir, ".."))
[string]$Script:rootDir = Resolve-Path ([IO.Path]::Combine($autoDir, ".."))
$Script:pathBackup = $env:PATH

$_ = Set-StopOnError $True

$benchLibFile = [IO.Path]::Combine($Script:myDir, "BenchLib.dll")
$_ = [Reflection.Assembly]::LoadFile($benchLibFile)

Debug "Loading configuration ..."
$Script:cfg = New-Object Mastersign.Bench.BenchConfiguration($Script:rootDir)

function Set-ConfigValue([string]$name, $value) {
    if ($Script:debug) {
        Debug "Config: $name = $value"
    }
    $Script:cfg.SetValue($name, $value)
}

function Get-ConfigValue([string]$name) {
    return $Script:cfg.GetValue($name)
}

function Get-ConfigListValue([string]$name) {
    return $Script:cfg.GetStringListValue($name)
}

function Get-ConfigPathValue([string]$name) {
    return $Script:cfg.GetStringValue($name)
}

function Set-AppConfigValue([string]$app, [string]$name, $value) {
	$Script:cfg.SetGroupValue($app, $name, $value)
}

function Get-AppConfigValue([string]$app, [string]$name) {
    return $Script:cfg.GetGroupValue($app, $name)
}

function Get-AppConfigListValue([string]$app, [string]$name) {
    return $Script:cfg.GetStringListGroupValue($app, $name)
}

function Get-AppConfigPathValue([string]$app, [string]$name) {
    return $Script:cfg.GetStringGroupValue($app, $name)
}

. "$Script:myDir\appconfig.lib.ps1"
