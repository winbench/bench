$rootDir = [IO.Path]::GetDirectoryName([IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition))
$scriptsDir = Resolve-Path "$rootDir\auto\lib"
$docsDir = Resolve-Path "$rootDir\docs"
& "$scriptsDir\Load-ClrLibs.ps1"

$cfg = New-Object Mastersign.Bench.BenchConfiguration ($rootDir, $true, $true, $true)
$env = New-Object Mastersign.Bench.BenchEnvironment ($cfg)
$env.Load()

cd $docsDir
Start-Process gulp watch
Start-Process "http://localhost:1313/bench/"
hugo -D server --disableFastRender
