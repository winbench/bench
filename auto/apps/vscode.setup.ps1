$codeResourceDir = "$(Get-ConfigPathValue AppResourceBaseDir)\vscode"
$codeAppData = Safe-Dir "$(Get-ConfigPathValue AppDataDir)\Code\User"

$snippetSourceDir = "$codeResourceDir\snippets"
$snippetTargetDir = Safe-Dir "$codeAppData\snippets"

foreach ($f in (Get-ChildItem "$codeResourceDir\*.json")) {
    if (!(Test-Path "$codeAppData\$($f.Name)")) {
        cp $f "$codeAppData\"
    }
}
foreach ($f in (Get-ChildItem "$snippetSourceDir\*.json")) {
    if (!(Test-Path "$snippetTargetDir\$($f.Name)")) {
        cp $f "$snippetTargetDir\"
    }
}
