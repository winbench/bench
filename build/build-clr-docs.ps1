param (
    $Mode = "Debug",
    $MsBuildVerbosity = "minimal"
)

$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$rootDir = [IO.Path]::GetDirectoryName($myDir)
pushd

$clrVersion = "4.0.30319"
$toolsVersion = "4.0"
$mode = $Mode
$target = "Clean;Build"
$verbosity = $MsBuildVerbosity
$msbuild = "$env:SystemRoot\Microsoft.NET\Framework\v$clrVersion\MSBuild.exe"
$solutionDir = "BenchManager" # relative to root dir
$solutionFile = "BenchManager.sln" # relative to solution dir
$helpProject = "BenchLibDocs\BenchLibDocs.shfbproj" # relative to solution dir
$helpOutputDir = "BenchLibDocs\Help\" # relative to solution dir
$helpTargetDir = "docs\clr-api\" # relative to root dir

# Build the Sand Castle Help File Builder project
echo ""
echo "Building SHFB project $solutionFile ..."
cd "$rootDir\$solutionDir"
& $msbuild $helpProject /v:$verbosity /tv:$toolsVersion /t:$target /p:Configuration=$mode /m /nodereuse:false
if ($LastExitCode -ne 0)
{
    Write-Error "Building the help SHFB project failed."
    popd
    return
}

# Copy Help Website

echo "Copying the help website to the target directory ..."

cd $rootDir
robocopy "$solutionDir\$helpOutputDir" "$helpTargetDir" /MIR /NJH /NFL /NP /XF *.log /XF *.php /XF *.aspx /XF *.config

popd

echo ""
echo "Finished building the BenchLib .NET API documentation."
