param (
    $gitURL = $(Read-Host "Git remote repository URL"),
    $projectName = $(Read-Host "Directory name for the project ($(([regex]"^.*[/\\]([^/\\]+).git$").Replace($gitURL, "`$1")))"),
    [switch]$debug
)

if ($projectName -eq "") {
    $projectName = ([regex]"^.*[/\\]([^/\\]+).git`$").Replace($gitURL, "`$1")
}

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"

Set-Debugging $debug

$projectRoot = Safe-Dir (Get-ConfigDir ProjectRootDir)

$projectPath = "$projectRoot\$projectName"

if (Test-Path $projectPath) {
    Write-Error "Project with name '$projectName' allready exists."
    exit 1
}
Empty-Dir $projectPath | Out-Null
Write-Output ""

git clone $gitURL $projectPath

if (!$?) {
    $ec = $LastExitCode
    Purge-Dir $projectPath
    cmd /C PAUSE
    exit $ec
}

pushd $projectPath

if ((Test-Path "bower.json" -PathType Leaf) -and -not (Test-Path "bower_components" -PathType Container)) {
    bower install
}
if ((Test-Path "package.json" -PathType Leaf) -and -not (Test-Path "node_modules" -PathType Container)) {
    npm install
}
if (Test-Path "gulpfile.js" -PathType Leaf) {
    gulp
}
if ((Test-Path "Gruntfile.js" -PathType Leaf) -or (Test-Path "Gruntfile.coffee" -PathType Leaf)) {
    grunt
}

popd

Run-Script Edit-Project -projectName $projectName
Run-Script Shell-Project -projectName $projectName
