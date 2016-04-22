$gitDir = App-Dir Git
$git = App-Exe Git

pushd $gitDir
Debug "Running post-install script for Git ..."
.\git-bash.exe --no-needs-console --hide --no-cd --command=git-post-install.bat
popd

if (Get-ConfigBooleanValue UseProxy) {
    & $git config --global "http.proxy" $(Get-ConfigValue HttpProxy)
    & $git config --global "https.proxy" $(Get-ConfigValue HttpsProxy)
    & $git config --global "url.https://.insteadof" "git://"
} else {
    & $git config --global --unset "http.proxy"
    & $git config --global --unset "https.proxy"
    & $git config --global --unset "url.https://.insteadof"
}