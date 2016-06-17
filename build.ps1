param (
    $Mode = "Release",
    $MsBuildVerbosity = "minimal",
    [switch]$NoRelease
)

$rootDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
pushd

$projectName = "Bench"
$clrVersion = "4.0.30319"
$toolsVersion = "14.0"
$mode = $Mode
$verbosity = $MsBuildVerbosity
$msbuild = "$env:SystemRoot\Microsoft.NET\Framework\v$clrVersion\MSBuild.exe"
$nugetUrl = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
$solutionDir = "BenchManager" # relative to root dir
$solutionFile = "BenchManager.sln" # relative to solution dir
$buildTargetDir = "auto\bin" # relative to root dir
$releaseDir = "$rootDir\release" # absolute
$releaseFileName = "$projectName"
$stageDir = "$releaseDir\staging" # absolute

# Paths of build artifacts are relative to the solution dir
$buildArtifacts = @(
    "BenchLib\bin\$mode\BenchLib.dll",
    "BenchLib\bin\$mode\Interop.IWshRuntimeLibrary.dll",
    "BenchLib\bin\$mode\Ionic.Zip.dll",
    "BenchDashboard\bin\$mode\BenchDashboard.exe",
    "BenchDashboard\bin\$mode\BenchDashboard.exe.config",
    "BenchDashboard\bin\$mode\ConEmu.WinForms.dll"
)
# Paths of release artifacts are relative to the root dir
$releaseArtifacts = @(
    "actions",
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

# Download NuGet
$nugetPath = "$rootDir\$solutionDir\nuget.exe"
if (!(Test-Path $nugetPath))
{
    echo "Downloading NuGet.exe ..."
    $wc = New-Object System.Net.WebClient
    $wc.DownloadFile($nugetUrl, $nugetPath)
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

# Build the Visual Studio solution
echo ""
echo "Building Visual Studio solution $solutionFile ..."
cd "$rootDir\$solutionDir"
& $msbuild $solutionFile /v:$verbosity /tv:$toolsVersion /p:Configuration=$mode
if ($LastExitCode -ne 0)
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

if (!$NoRelease)
{
    # Prepare release names
    $taggedzipName = "$releaseDir\${releaseFileName}_$([DateTime]::Now.ToString("yyyy-MM-dd"))"
    $suffix = 0
    $taggedZipFile = "${taggedZipName}.zip"
    while (Test-Path $taggedZipFile)
    {
        $suffix++
        $taggedZipFile = "${taggedZipName}_${suffix}.zip"
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

    $zipFile = "$releaseDir\$releaseFileName.zip"
    if (Test-Path $zipFile) { del $zipFile }
    cp $taggedZipFile $zipFile
    echo ""
    echo "Latest release: `"$zipFile`" ($([IO.Path]::GetFileName($taggedZipFile)))"

    # Clean staging folder
    #del $stageDir -Recurse -Force
}

# End
popd
echo ""
echo "Finished."
