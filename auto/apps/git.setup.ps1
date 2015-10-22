$git = App-Exe Git
if (!$git) { throw "Git not found" }
& $git config --global "push.default" "simple"
if (Get-ConfigValue UseProxy) {
    & $git config --global "http.proxy" $(Get-ConfigValue HttpProxy)
    & $git config --global "https.proxy" $(Get-ConfigValue HttpsProxy)
    & $git config --global "url.https://.insteadof" "git://"
} else {
    & $git config --global --unset "http.proxy"
    & $git config --global --unset "https.proxy"
    & $git config --global --unset "url.https://.insteadof"
}
