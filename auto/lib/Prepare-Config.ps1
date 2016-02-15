param ([switch]$debug)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"

trap { Write-TrapError $_ }
Set-Debugging $debug

$customConfigDir = Safe-Dir (Get-ConfigPathValue CustomConfigDir)
$customAppIndex = Get-ConfigPathValue CustomAppIndex
$customConfigFile = Get-ConfigPathValue CustomConfigFile

# Migration from < v0.8.0

$legacyCustomAppIndex = [IO.Path]::Combine((Get-ConfigPathValue BenchRoot), "apps.md")
if (Test-Path $legacyCustomAppIndex) {
    Move-Item $legacyCustomAppIndex $customAppIndex
    Write-Host "Moved custom app index to: $customAppIndex"
}
$legacyCustomConfigFile = [IO.Path]::Combine((Get-ConfigPathValue BenchRoot), "config.ps1")
if (Test-Path $legacyCustomConfigFile) {
    Move-Item $legacyCustomConfigFile $customConfigFile
    Write-Host "Moved custom configuration to: $customConfigFile"
}

# Initialize Custom App Index

if (!(Test-Path $customAppIndex)) {
    $customAppIndexTemplate = Get-ConfigPathValue CustomAppIndexTemplate
    copy $customAppIndexTemplate $customAppIndex
}

# Initialize Custom Configuration File

if (!(Test-Path $customConfigFile)) {
    $customConfigTemplate = Get-ConfigPathValue CustomConfigTemplate
    $user = Read-Host "Your Name"
    $email = Read-Host "Your Email"
    $config = Get-Content $customConfigTemplate
    $config += ""
    $config += "Set-ConfigValue UserName `"$user`""
    $config += "Set-ConfigValue UserEmail `"$email`""
    $config | Out-File $customConfigFile -Encoding utf8
    Write-Host "Edit and save the custom configuration, close editor to continue ..."
    Start-Process notepad @($customConfigFile) -Wait
}
