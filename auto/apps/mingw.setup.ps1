$mingwGet = App-Exe MinGwGet

$mingwPackages = Get-AppConfigListValue MinGW Packages

foreach ($p in $mingwPackages)
{
    Write-Host "Setting up MinGW package $p ..."
    & $mingwGet install $p
}
