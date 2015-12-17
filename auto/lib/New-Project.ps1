param (
    $projectName = $(Read-Host "Directory name for the new project"),
    [switch]$debug
)

if (!$projectName) { return }

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
    exit 1
}
Empty-Dir $projectPath | Out-Null
Write-Output ""

pushd $projectPath

yo
Exit-OnError

git init
Exit-OnError

git add -A :/
Exit-OnError

git commit -m "Project initialized."
Exit-OnError

if ((Test-Path "package.json" -PathType Leaf) -and -not (Test-Path "node_modules" -PathType Container)) {
    npm install
    Exit-OnError
}
if ((Test-Path "bower.json" -PathType Leaf) -and -not (Test-Path "bower_components" -PathType Container)) {
    bower install
    Exit-OnError
}
if (Test-Path "gulpfile.js" -PathType Leaf) {
    gulp
    Exit-OnError
}
if ((Test-Path "Gruntfile.js" -PathType Leaf) -or (Test-Path "Gruntfile.coffee" -PathType Leaf)) {
    grunt
    Exit-OnError
}

popd

Run-Script Open-Project -projectName $projectName
Run-Script Open-ProjectShell -projectName $projectName
