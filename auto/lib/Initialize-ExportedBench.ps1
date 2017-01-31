$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$sourceDir = Resolve-Path "$myDir\..\.."

$targetDir = $(Read-Host "Zielverzeichnis")

copy "$sourceDir\*" "$targetDir\" -Recurse -Force

& "$targetDir\auto\bin\bench.exe" --verbose manage initialize
