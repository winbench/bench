param (
    $projectName = $(& ([IO.Path]::Combine([IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition), "Select-Project.ps1"))),
    [switch]$debug
)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\common.lib.ps1"
. "$scriptsLib\config.lib.ps1"
. "$scriptsLib\fs.lib.ps1"

Set-Debugging $debug
$_ = Set-StopOnError $True

$projectRoot = Safe-Dir $(Get-ConfigDir ProjectRootDir)

if ([IO.Path]::IsPathRooted($projectName)) {
    $projectPath = $projectName
    $projectName = [IO.Path]::GetFileName($projectPath)
} else {
    $projectPath = Resolve-Path "$projectRoot\$projectName"
}

cd $projectPath
Run-Script Shell "PROJECT $projectName"
