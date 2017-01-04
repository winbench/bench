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

$appLibsDir = Safe-Dir "$rootDir\applibs"
pushd $appLibsDir

$appLibs = @()
Get-Content "$myDir\applibs.txt" | % {
    $parts = $_.Split("=", 2)
    $appLibs += @{ "id"=$parts[0]; "url"=$parts[1] }
}

foreach ($lib in $appLibs)
{
    $p = "$appLibsDir\$($lib.id)"
    if (!(Test-Path $p))
    {
        echo "Cloning app library '$($lib.id)' ..."
        git clone $lib.url $lib.id
    }
    else
    {
        echo "App library '$($lib.id)' is already cloned."
    }
}
echo "Finished cloning app libraries."

popd