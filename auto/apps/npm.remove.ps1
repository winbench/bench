$node = App-Exe Node
$nodeDir = App-Dir Node
if (!$node) { throw "NodeJS not found" }
$npm = App-Exe Npm
if (!$npm) { throw "Node Package Manager not found" }

& $node "$nodeDir\node_modules\npm\bin\npm-cli.js" remove --global "npm"
