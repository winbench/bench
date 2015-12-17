param (
    $projectName = $(& ([IO.Path]::Combine([IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition), "Select-Project.ps1"))),
    [switch]$debug
)

if (!$projectName) { return }

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\common.lib.ps1"
. "$scriptsLib\config.lib.ps1"
. "$scriptsLib\fs.lib.ps1"

Set-Debugging $debug
$_ = Set-StopOnError $True

if ([IO.Path]::IsPathRooted($projectName)) {
    $projectPath = $projectName
    $projectName = [IO.Path]::GetFileName($projectPath)
} else {
    Debug "Resolving project dir for: $projectName"
    $projectRoot = Safe-Dir $(Get-ConfigDir ProjectRootDir)
    $projectPath = Resolve-Path "$projectRoot\$projectName"
}

cd $projectPath
if (Test-Path "gulpfile.js" -PathType Leaf) {
    gulp watch
} elseif ((Test-Path "Gruntfile.js" -PathType Leaf) -or (Test-Path "Gruntfile.coffee" -PathType Leaf)) {
    grunt watch
} else {
    Write-Warning "The project is not automated with Gulp or Grunt"
    Pause
}
