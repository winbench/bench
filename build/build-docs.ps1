$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$rootDir = [IO.Path]::GetDirectoryName($myDir)
$scriptsDir = Resolve-Path "$rootDir\auto\lib"
$docsDir = Resolve-Path "$rootDir\docs"

# Compile the Assembly if necessary

$assemblyPath = "$rootDir\BenchManager\BenchLib\bin\$mode\BenchLib.dll"
if (!(Test-Path $assemblyPath))
{
    & "$myDir\build.ps1" -Mode Debug -MsBuildVerbosity minimal -NoRelease
}

# Load Assembly

& "$scriptsDir\Load-ClrLibs.ps1"

$cfg = New-Object Mastersign.Bench.BenchConfiguration ($rootDir, $true, $true, $true)
$benchEnv = New-Object Mastersign.Bench.BenchEnvironment ($cfg)
$benchEnv.Load()

function check-success() {
  if ($LastExitCode -ne 0)
  {
    popd
    exit $exitCode
  }
}

pushd $docsDir

del .\public -Recurse -Force

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

& "$myDir\build-clr-docs.ps1"

popd
