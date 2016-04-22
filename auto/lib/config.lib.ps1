$Script:myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$Script:myDir\common.lib.ps1"

[string]$Script:autoDir = Resolve-Path ([IO.Path]::Combine($myDir, ".."))
[string]$Script:rootDir = Resolve-Path ([IO.Path]::Combine($autoDir, ".."))
$Script:pathBackup = $env:PATH

$_ = Set-StopOnError $True

$benchLibFile = [IO.Path]::Combine($Script:autoDir, "bin\BenchLib.dll")
$_ = [Reflection.Assembly]::LoadFile($benchLibFile)

Debug "Loading configuration ..."
$Script:cfg = New-Object Mastersign.Bench.BenchConfiguration($Script:rootDir)

function Get-ConfigValue([string]$name) {
    return $Script:cfg.GetValue($name)
}

function Get-ConfigBooleanValue([string]$name) {
    return $Script:cfg.GetBooleanValue($name)
}

function Get-ConfigListValue([string]$name) {
    return $Script:cfg.GetStringListValue($name)
}

function Get-AppConfigValue([string]$app, [string]$name) {
    return $Script:cfg.GetGroupValue($app, $name)
}

function Get-AppConfigBooleanValue([string]$app, [string]$name) {
    return $Script:cfg.GetBooleanGroupValue($app, $name)
}

function Get-AppConfigListValue([string]$app, [string]$name) {
    return $Script:cfg.GetStringListGroupValue($app, $name)
}

. "$Script:myDir\appconfig.lib.ps1"
