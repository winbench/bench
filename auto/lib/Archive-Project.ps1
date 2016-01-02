param (
    $projectName = $(& ([IO.Path]::Combine([IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition), "Select-Project.ps1"))),
    [switch]$debug
)

if (!$projectName) { return }

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"

Set-Debugging $debug

$projectPath = Get-ProjectPath $projectName
$projectName = Get-ProjectName $projectName
$archiveDir = Safe-Dir (Get-ConfigPathValue ProjectArchiveDir)
$archiveFormat = Get-ConfigValue ProjectArchiveFormat "zip"

$timestamp = [DateTime]::Now.ToString("yyyyMMdd_HHmm")
$projectVersion = Get-ProjectVersion $projectName

$nameParts = @($projectName, $timestamp)
if ($projectVersion) {
    $nameParts += "v$projectVersion"
}

$archiveName = [string]::Join("_", $nameParts) + "." + $archiveFormat
$archivePath = [IO.Path]::Combine($archiveDir, $archiveName)

cd $projectPath

7za a -mx $archivePath ".\*"
