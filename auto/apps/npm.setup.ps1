$npm = App-Exe Npm
if (!$npm) { throw "Node Package Manager not found" }
& $npm config set registry "http://registry.npmjs.org/"
if (Get-ConfigValue UseProxy) {
    & $npm config set "proxy" $(Get-ConfigValue HttpProxy)
    & $npm config set "https-proxy" $(Get-ConfigValue HttpsProxy)
} else {
    & $npm config delete "proxy"
    & $npm config delete "https-proxy"
}
