$mingwGet = App-Exe MinGwGet

$mingwPackages = Get-AppConfigListValue MinGW Packages

$ErrorActionPreference = "SilentlyContinue"
foreach ($p in $mingwPackages)
{
    Write-Host "Setting up MinGW package $p ..."
    & $mingwGet install $p
}
