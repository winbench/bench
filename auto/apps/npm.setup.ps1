$node = App-Exe Node
$nodeDir = App-Dir Node
if (!$node) { throw "NodeJS not found" }
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

$currentNpmVersion = & $npm --version
if ($currentNpmVersion.Trim() -eq "1.4.12") {
    $targetNpmVersion = App-Version Npm
    & $node "$nodeDir\node_modules\npm\bin\npm-cli.js" install --global "`"npm@$targetNpmVersion`""
}
