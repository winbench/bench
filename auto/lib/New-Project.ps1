param (
    $projectName = $(Read-Host "Directory name for the new project"),
    [switch]$debug
)

if (!$projectName) { return }

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"

trap { Write-TrapError $_ }
Set-Debugging $debug

$projectRoot = Safe-Dir (Get-ConfigValue ProjectRootDir)

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

Run-Script Edit-project -projectName $projectName
Run-Script Shell-Project -projectName $projectName
