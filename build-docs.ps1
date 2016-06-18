$rootDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$scriptsDir = Resolve-Path "$rootDir\auto\lib"
$docsDir = Resolve-Path "$rootDir\docs"
& "$scriptsDir\Load-ClrLibs.ps1"

$cfg = New-Object Mastersign.Bench.BenchConfiguration ($rootDir, $true, $false, $false)
$hugo = $cfg.Apps["Hugo"].Exe

pushd $docsDir

& $hugo
$hugoExitCode = $LastExitCode

popd

exit $hugoExitCode