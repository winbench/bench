$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$rootDir = [IO.Path]::GetDirectoryName($myDir)
$scriptsDir = Resolve-Path "$rootDir\auto\lib"
$docsDir = Resolve-Path "$rootDir\docs"
& "$scriptsDir\Load-ClrLibs.ps1"

$cfg = New-Object Mastersign.Bench.BenchConfiguration ($rootDir, $true, $true, $true)
$benchEnv = New-Object Mastersign.Bench.BenchEnvironment ($cfg)
$benchEnv.Load()

function check-success() {
  if ($LastExitCode -ne 0)
  {
    pushd
    exit $exitCode
  }
}

pushd $docsDir

& "$myDir\update-app-list.ps1"
if (!(Test-Path "$docsDir\node_modules"))
{
  npm install
  check-success
}
if (!(Test-Path "$docsDir\bower_components"))
{
  bower install
  check-success
}

gulp
check-success

hugo -D
check-success

popd
