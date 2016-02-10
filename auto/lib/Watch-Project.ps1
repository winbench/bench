param (
    $projectName = $(& ([IO.Path]::Combine([IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition), "Select-Project.ps1"))),
    [switch]$debug
)

if (!$projectName) { return }

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"

trap { Write-TrapError $_ }
Set-Debugging $debug

$projectPath = Get-ProjectPath $projectName
$projectName = Get-ProjectName $projectName

cd $projectPath
if (Test-Path "gulpfile.js" -PathType Leaf) {
    gulp watch
} elseif ((Test-Path "Gruntfile.js" -PathType Leaf) -or (Test-Path "Gruntfile.coffee" -PathType Leaf)) {
    grunt watch
} else {
    Write-Warning "The project is not automated with Gulp or Grunt"
    Pause
}
