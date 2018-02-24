$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$rootDir = [IO.Path]::GetDirectoryName($myDir)
$scriptsDir = Resolve-Path "$rootDir\auto\lib"

. "$scriptsDir\bench.lib.ps1"
$benchEnv = New-Object Mastersign.Bench.BenchEnvironment ($global:BenchConfig)
$benchEnv.Load()

if (!(Get-Command git -ErrorAction Ignore))
{
    Write-Error "Git is not available."
    return
}

$projectsDir = Safe-Dir "$rootDir\projects"
pushd $projectsDir

$projects = @()
Get-Content "$myDir\sub-projects.txt" | % {
    $parts = $_.Split("=", 2)
    $projects += @{ "id"=$parts[0]; "url"=$parts[1] }
}

foreach ($proj in $projects)
{
    $p = "$projectsDir\$($proj.id)"
    if (!(Test-Path $p))
    {
        echo "Cloning project '$($proj.id)' ..."
        git clone $proj.url $proj.id
    }
    else
    {
        echo "Project '$($proj.id)' is already cloned."
    }
}
echo "Finished cloning projects."

popd