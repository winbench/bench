param ([switch]$debug)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"

trap { Write-TrapError $_ }
Set-Debugging $debug

$customAppIndex = Get-ConfigPathValue CustomAppIndex
if (!(Test-Path $customAppIndex)) {
    $customAppIndexTemplate = Get-ConfigPathValue CustomAppIndexTemplate
    copy $customAppIndexTemplate $customAppIndex
}

$customConfigFile = Get-ConfigPathValue CustomConfigFile
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
