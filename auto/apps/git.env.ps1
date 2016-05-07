$gitDir = App-Dir Git
$git = App-Exe Git

pushd $gitDir
Write-Host "Running post-install script for Git ..."
copy "post-install.bat.bak" "post-install.bat"
cmd /C "post-install.bat" | Out-Null
popd

if (Get-ConfigBooleanValue UseProxy)
{
    & $git config --global "http.proxy" $(Get-ConfigValue HttpProxy)
    & $git config --global "https.proxy" $(Get-ConfigValue HttpsProxy)
    & $git config --global "url.https://.insteadof" "git://"
}
else
{
    & $git config --global --unset "http.proxy"
    & $git config --global --unset "https.proxy"
    & $git config --global --unset "url.https://.insteadof"
}
