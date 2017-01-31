$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$sourceDir = Resolve-Path "$myDir\..\.."

$targetDir = $(Read-Host "Zielverzeichnis")

if (!(Test-Path $targetDir)) { $_ = mkdir $targetDir }
copy "$sourceDir\*" "$targetDir\" -Recurse -Force

Start-Process -FilePath "$targetDir\auto\bin\bench.exe" -ArgumentList @("--verbose", "manage", "initialize")
