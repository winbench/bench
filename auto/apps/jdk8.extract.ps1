param ($archive, $targetDir)

$jdkexDir = Empty-Dir "$(Get-ConfigValue TempDir)\jdk8ex"

$7z = App-Exe SvZ

& $7z x "-o$jdkexDir" "$archive" | Out-Null

if (!(Test-Path "$jdkexDir\tools.zip")) {
    throw "Did not find the expected content in the JDK archive"
}

& $7z x "-o$targetDir" "-x!lib\missioncontrol*" "-x!bin\jmc.exe" "-x!javafx-src.zip" "$jdkexDir\tools.zip" | Out-Null

Purge-Dir $jdkexDir

foreach ($f in (Get-ChildItem $targetDir -Include "*.pack" -Recurse)) {
    & "$targetDir\bin\unpack200.exe" -r $f.FullName ([IO.Path]::ChangeExtension($f.FullName, ".jar")) | Out-Null
}
