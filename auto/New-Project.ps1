$autoDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$projectRoot = [IO.Path]::Combine($autoDir, "..", "projects")

$projectName = Read-Host "Directory name for new project"

if (!(Test-Path $projectRoot)) {
    mkdir $projectRoot | Out-Null
}

$projectPath = "$projectRoot\$projectName"
if (Test-Path $projectPath) {
    Write-Error "Project with name '$projectName' allready exists."
    return
}
mkdir $projectPath | Out-Null

pushd $projectPath
yo mdproc
git init
git add -A :/
git commit -m "Project initialized."
gulp autobuild
popd

& "$autoDir\Open-Project.ps1" $projectName
