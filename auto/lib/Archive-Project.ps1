﻿param (
    $projectName = $(& ([IO.Path]::Combine([IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition), "Select-Project.ps1"))),
    [switch]$debug
)

if (!$projectName) { return }

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"

$7z = App-Exe 7z

trap { Write-TrapError $_ }
Set-Debugging $debug

$projectPath = Get-ProjectPath $projectName
$projectName = Get-ProjectName $projectName
$archiveDir = Safe-Dir (Get-ConfigValue ProjectArchiveDir)
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

& $7z a -mx $archivePath ".\*"
