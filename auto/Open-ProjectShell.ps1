param (
    $projectName = $(& ([IO.Path]::Combine([IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition), "Select-Project.ps1")))
)

$autoDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$projectRoot = [IO.Path]::Combine($autoDir, "..", "projects")
if ([IO.Path]::IsPathRooted($projectName)) {
    $projectPath = $projectName
    $projectName = [IO.Path]::GetFileName($projectPath)
} else {
    $projectPath = Resolve-Path ([IO.Path]::Combine($projectRoot, $projectName))
}

cd $projectPath
Clear-Host
Write-Host "Bench PowerShell"
Write-Host "Project: $projectName"
