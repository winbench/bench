param (
    $Mode = "Release",
    $MsBuildVerbosity = "minimal",
    [switch]$NoSign,
    [switch]$NoRelease
)

$myDir = $PSScriptRoot
$rootDir = [IO.Path]::GetDirectoryName($myDir)
pushd

# To build this project without Visual Studio, install the Visual Studio 2019 Build Tools
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
$msbuildPaths = @(
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe"
)
$msbuild = $msbuildPaths | Where-Object { Test-Path $_ } | Select-Object -First 1
$nugetUrl = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
$nuget4Url = "https://dist.nuget.org/win-x86-commandline/v4.0.0/nuget.exe"
$solutionDir = "BenchManager" # relative to root dir
$solutionFile = "BenchManager.sln" # relative to solution dir
$buildTargetDir = "auto\bin" # relative to root dir
$releaseDir = "$rootDir\release" # absolute
$releaseFileName = "$projectName"
$stageDir = "$releaseDir\staging" # absolute

# Allow TLS 1.1 and 1.2 in downloads
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]'Tls11,Tls12'

if (!$msbuild) {
    Write-Warning "Could not find MSBuild"
    Write-Host "Searched for MSBuild at"
    foreach ($path in $msbuildPaths) {
        Write-Host "  - $path"
    }
    exit 1
} else {
    Write-Host "Using MSBuild at: $msbuild"
}

$projects = @("BenchLib", "BenchLib.Test", "BenchCLI", "BenchDashboard")

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
    "BenchDashboard\Resources\bash.ico",
    "packages\HtmlAgilityPack.1.8.11\lib\Net45\HtmlAgilityPack.dll",
    "scripts\bench-cmd.cmd",
    "scripts\bench-ps.cmd",
    "scripts\bench-pwsh.cmd",
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
mkdir "$rootDir\$buildTargetDir" | Out-Null
foreach ($artifact in $buildArtifacts)
{
    echo "  $artifact"
    Copy-Artifact "$rootDir\$solutionDir\$artifact" "$rootDir\$buildTargetDir"
}

if ($Mode -eq "Debug") {
    cp "$rootDir\res\Invoke-AppSetupTest.ps1" "$rootDir\$buildTargetDir\tas.ps1"
    cp "$rootDir\res\Invoke-AppVersionCheck.ps1" "$rootDir\$buildTargetDir\cav.ps1"
}

# Update setup project
echo ""
echo "Updating setup project sources ..."
& "$myDir\update-setup-sources.ps1"

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

    # Create Setup EXE release
    cd "$rootDir"
    $taggedName = "$releaseDir\${releaseFileName}Setup_$today"
    $taggedSetupExeFile = "${taggedName}.exe"
    if ($suffix -gt 0)
    {
        $taggedSetupExeFile = "${taggedName}_${suffix}.exe"
    }
    .\auto\bin\bench.exe --verbose transfer export --include SystemOnly $taggedSetupExeFile
    if ($?)
    {
        if (!$NoSign) {
            echo ""
            echo "Signing setup program..."
            & "$PSScriptRoot\sign.ps1" $taggedSetupExeFile
        }
        $setupExeFile = "$releaseDir\${releaseFileName}Setup.exe"
        cp $taggedSetupExeFile $setupExeFile -Force
    }

    echo ""
    echo "Latest release: `"$zipFile`" ($([IO.Path]::GetFileName($taggedZipFile)))"
    echo "Latest release: `"$setupExeFile`" ($([IO.Path]::GetFileName($taggedSetupExeFile)))"

    # Clean staging folder
    #del $stageDir -Recurse -Force
}

# End
popd
echo ""
echo "Finished."
