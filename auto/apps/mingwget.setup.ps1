$mingwDir = App-Dir MinGwGet
$mingwGet = App-Exe MinGwGet

if (!(Test-Path "$mingwDir\var\cache"))
{
    pushd $mingwDir
    Write-Host "Updating MinGW catalog ..."
    & $mingwGet update
    popd
}
