param (
    $projectName = $(Read-Host "Directory name for the new project"),
    [switch]$debug
)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\common.lib.ps1"
. "$scriptsLib\config.lib.ps1"
. "$scriptsLib\fs.lib.ps1"

Set-Debugging $debug
$_ = Set-StopOnError $True

$projectRoot = Safe-Dir $(Get-ConfigDir ProjectRootDir)

$projectPath = "$projectRoot\$projectName"

if (Test-Path $projectPath) {
    Write-Error "Project with name '$projectName' allready exists."
    return
}
Empty-Dir $projectPath

pushd $projectPath
yo mdproc
git init
git add -A :/
git commit -m "Project initialized."
gulp autobuild
popd

Run-Script Open-Project -projectName $projectName
Run-Script Open-ProjectShell -projectName $projectName
