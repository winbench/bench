$rootDir = [IO.Path]::GetDirectoryName([IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition))
$scriptsDir = Resolve-Path "$rootDir\auto\lib"
$docsDir = Resolve-Path "$rootDir\docs"
& "$scriptsDir\Load-ClrLibs.ps1"

$cfg = New-Object Mastersign.Bench.BenchConfiguration ($rootDir, $true, $false, $false)
$hugo = $cfg.Apps["Hugo"].Exe

cd $docsDir
Start-Process "http://localhost:1313/bench/"
& $hugo server -D
