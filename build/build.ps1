param (
    $Mode = "Release",
    $MsBuildVerbosity = "minimal",
    [switch]$NoRelease
)

$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$rootDir = [IO.Path]::GetDirectoryName($myDir)
pushd

# To build this project without Visual Studio, install the Visual Studio 2017 Build Tools
# with the .NET Framework 4.6.2 SDK and the .NET 4.6.2 Target Pack.

$projectName = "Bench"
$clrVersion = "4.0.30319"
$toolsVersion = $null
$projectToolsVersion = "15.0"
$compilerPackageVersion = "2.8.2"
$compilerPackageFramework = "net46"
$langVersion = "7.2"
$mode = $Mode
$target = "Clean;Build"
$verbosity = $MsBuildVerbosity
# $msbuild = "$env:SystemRoot\Microsoft.NET\Framework\v$clrVersion\MSBuild.exe"
$msbuild = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\MSBuild.exe"
$nugetUrl = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
$nuget4Url = "https://dist.nuget.org/win-x86-commandline/v4.0.0/nuget.exe"
$solutionDir = "BenchManager" # relative to root dir
$solutionFile = "BenchManager.sln" # relative to solution dir
$buildTargetDir = "auto\bin" # relative to root dir
$releaseDir = "$rootDir\release" # absolute
$releaseFileName = "$projectName"
$stageDir = "$releaseDir\staging" # absolute

$projects = @("BenchLib", "BenchCLI", "BenchDashboard")

# Paths of build artifacts are relative to the solution dir
$buildArtifacts = @(
    "BenchLib\bin\$mode\BenchLib.dll",
    "BenchLib\bin\$mode\Interop.IWshRuntimeLibrary.dll",
    "BenchLib\bin\$mode\DotNetZip.dll",
    "BenchLib\bin\$mode\Mastersign.Sequence.dll",
    "BenchCLI\bin\$mode\bench.exe",
    "BenchCLI\bin\$mode\bench.exe.config",
    "BenchDashboard\bin\$mode\BenchDashboard.exe",
    "BenchDashboard\bin\$mode\BenchDashboard.exe.config",
    "BenchDashboard\bin\$mode\ConEmu.WinForms.dll",
    "packages\HtmlAgilityPack.1.8.11\lib\Net45\HtmlAgilityPack.dll",
    "scripts\bench-cmd.cmd",
    "scripts\bench-ps.cmd",
    "scripts\bench-bash.cmd",
    "scripts\runps.cmd"
)
# Paths of release artifacts are relative to the root dir
$releaseArtifacts = @(
    "auto",
    "res",
    "CHANGELOG.md",
    "LICENSE.md",
    "README.md"
)

function Copy-Artifact($src, $trgDir)
{
    if (Test-Path $src -PathType Container)
    {
        cp $src "$trgDir\" -Recurse
    }
    if (Test-Path $src -PathType Leaf)
    {
        cp $src "$trgDir\"
    }
}

echo "Building Bench ($mode)"

# Add Roslyn compiler to projects
& "$myDir\prepare-project-compiler.ps1" `
    -SourceDir "..\BenchManager" `
    -Projects $projects `
    -ToolsVersion $projectToolsVersion `
    -CompilerPackageVersion $compilerPackageVersion `
    -CompilerPackageFramework $compilerPackageFramework `
    -LangVersion $langVersion

# Download NuGet
$nugetPath = "$rootDir\$solutionDir\nuget.exe"
if (!(Test-Path $nugetPath))
{
    echo "Downloading latest NuGet.exe ..."
    $wc = New-Object System.Net.WebClient
    $wc.DownloadFile($nugetUrl, $nugetPath)
}
[Version]$nugetVersion = [Diagnostics.FileVersionInfo]::GetVersionInfo($nugetPath).FileVersion
if ($nugetVersion.Major -lt 4)
{
    echo "Latest NuGet is earlier then 4.x."
    del $nugetPath
    echo "Downloading NuGet.exe 4.0 ..."
    $wc = New-Object System.Net.WebClient
    $wc.DownloadFile($nuget4Url, $nugetPath)
}

# Restore NuGet packages
echo ""
echo "Restoring NuGet packages ..."
cd "$rootDir\$solutionDir"
.\nuget.exe restore "$solutionFile"
if ($LastExitCode -ne 0)
{
    Write-Error "Restoring NuGet packages failed."
    popd
    return
}

# Create output directory if necessary
if (!(Test-Path "$rootDir\$buildTargetDir"))
{
    mkdir "$rootDir\$buildTargetDir" | Out-Null
}

# Build the Visual Studio solution
echo ""
echo "Building Visual Studio solution $solutionFile ..."
cd "$rootDir\$solutionDir"
if ($toolsVersion)
{
    & $msbuild $solutionFile /v:$verbosity /tv:$toolsVersion /t:$target /p:Configuration=$mode /m /nodereuse:false
}
else
{
    & $msbuild $solutionFile /v:$verbosity /t:$target /p:Configuration=$mode /m /nodereuse:false
}
$buildError = $LastExitCode

# Remove Roslyn compiler from projects
& "$myDir\prepare-project-compiler.ps1" -SourceDir "..\BenchManager" -Projects $projects -Remove

# Canceling after failed build
if ($buildError -ne 0)
{
    Write-Error "Building the solution failed."
    popd
    return
}

# Copy build artifacts
echo ""
echo "Copying build artifacts to $rootDir\$buildTargetDir ..."
if (Test-Path "$rootDir\$buildTargetDir") { del "$rootDir\$buildTargetDir" -Recurse -Force }
$_ = mkdir "$rootDir\$buildTargetDir"
foreach ($artifact in $buildArtifacts)
{
    echo "  $artifact"
    Copy-Artifact "$rootDir\$solutionDir\$artifact" "$rootDir\$buildTargetDir"
}

$today = [DateTime]::Now.ToString("yyyy-MM-dd")

if (!$NoRelease)
{
    # Prepare release names
    $taggedName = "$releaseDir\${releaseFileName}_$today"
    $suffix = 0
    $taggedZipFile = "${taggedName}.zip"
    while (Test-Path $taggedZipFile)
    {
        $suffix++
        $taggedZipFile = "${taggedName}_${suffix}.zip"
    }

    # Prepare release and staging folders
    echo ""
    echo "Preparing release folder ..."
    if (!(Test-Path $releaseDir)) { $_ = mkdir $releaseDir }
    if (Test-Path $stageDir) { Remove-Item $stageDir -Force -Recurse }
    $_ = mkdir $stageDir

    # Copy release artifacts
    echo "Copying release artifacts to $stageDir ..."
    foreach ($artifact in $releaseArtifacts)
    {
        echo "  $artifact"
        Copy-Artifact "$rootDir\$artifact" $stageDir
    }

    # Create release archive
    cd "$rootDir"
    echo ""
    echo "Creating release archive ..."
    $_ = [Reflection.Assembly]::LoadWithPartialName("System.IO.Compression.FileSystem")
    [IO.Compression.ZipFile]::CreateFromDirectory($stageDir, $taggedZipFile, "Optimal", $False)

    $zipFile = "$releaseDir\${releaseFileName}.zip"
    if (Test-Path $zipFile) { del $zipFile }
    cp $taggedZipFile $zipFile

    # Create SFX release
    cd "$rootDir"
    $taggedName = "$releaseDir\${releaseFileName}Setup_$today"
    $taggedSfxFile = "${taggedName}.exe"
    if ($suffix -gt 0)
    {
        $taggedSfxFile = "${taggedName}_${suffix}.exe"
    }
    .\auto\bin\bench.exe --verbose transfer export --include SystemOnly $taggedSfxFile
    if ($?)
    {
        $sfxFile = "$releaseDir\${releaseFileName}Setup.exe"
        cp $taggedSfxFile $sfxFile -Force
    }

    echo ""
    echo "Latest release: `"$zipFile`" ($([IO.Path]::GetFileName($taggedZipFile)))"
    echo "Latest release: `"$sfxFile`" ($([IO.Path]::GetFileName($taggedSfxFile)))"

    # Clean staging folder
    #del $stageDir -Recurse -Force
}

# End
popd
echo ""
echo "Finished."
