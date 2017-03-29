$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$rootDir = [IO.Path]::GetDirectoryName($myDir)
$scriptsDir = Resolve-Path "$rootDir\auto\lib"
$docsDir = Resolve-Path "$rootDir\docs"

function check-success()
{
  if ($LastExitCode -ne 0)
  {
    popd
    exit $exitCode
  }
}

# Compile the Assembly if necessary

$assemblyPath = "$rootDir\BenchManager\BenchLib\bin\$mode\BenchLib.dll"
if (!(Test-Path $assemblyPath))
{
    & "$myDir\build.ps1" -Mode Debug -MsBuildVerbosity minimal -NoRelease
}

# Make sure the documentation build tools are available

pushd $docsDir
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
popd

# Load Bench Assemblies

& "$scriptsDir\Load-ClrLibs.ps1"

$cfg = New-Object Mastersign.Bench.BenchConfiguration ($rootDir, $true, $true, $true)
$benchEnv = New-Object Mastersign.Bench.BenchEnvironment ($cfg)
$benchEnv.Load()

pushd $docsDir

# Clean output directory

if (Test-Path .\public)
{
  del .\public -Recurse -Force
}

# Write generated content

& "$myDir\update-bench-cli-docs.ps1"
& "$myDir\update-app-list.ps1"
& "$myDir\update-dependency-graph.ps1"

# Enhance Markdown

gulp
check-success

# Build HTML website

hugo -D
check-success

# Build CLR documenation

& "$myDir\build-clr-docs.ps1"

popd
