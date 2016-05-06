$node = App-Exe Node
$nodeDir = App-Dir Node
if (!$node) { throw "NodeJS not found" }
$npm = App-Exe Npm
if (!$npm) { throw "Node Package Manager not found" }

& $npm config set registry "http://registry.npmjs.org/"
if (Get-ConfigBooleanValue UseProxy) {
    & $npm config set "proxy" $(Get-ConfigValue HttpProxy)
    & $npm config set "https-proxy" $(Get-ConfigValue HttpsProxy)
} else {
    & $npm config delete "proxy"
    & $npm config delete "https-proxy"
}
