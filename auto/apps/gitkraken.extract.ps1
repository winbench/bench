param ($archive, $targetDir)

$extractDir = Empty-Dir "$(Get-ConfigValue TempDir)\gitkraken"

$7z = App-Exe 7z

& $7z x "-o$extractDir" "$archive" | Out-Null

if (!(Test-Path "$extractDir\Update.exe")) {
    throw "Did not find the expected content in the setup archive"
}

$nupkg = gci "$extractDir\gitkraken-*-full.nupkg"

& $7z x "-o$targetDir" $nupkg.FullName | Out-Null

Purge-Dir $extractDir
