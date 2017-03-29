$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$rootDir = [IO.Path]::GetDirectoryName($myDir)
$scriptsDir = Resolve-Path "$rootDir\auto\lib"

. "$scriptsDir\bench.lib.ps1"
$benchEnv = New-Object Mastersign.Bench.BenchEnvironment ($global:BenchConfig)
$benchEnv.Load()

$appLibsDevDir = Safe-Dir "$rootDir\applibs"
$appLibsDir = Empty-Dir $(Get-ConfigValue "AppLibsInstallDir")

pushd $appLibsDevDir

$appLibs = @()
Get-Content "$myDir\applibs.txt" | % {
    $parts = $_.Split("=", 2)
    $appLibs += @{ "id"=$parts[0]; "url"=$parts[1] }
}

foreach ($lib in $appLibs)
{
    $id = $lib.id
    $p = "$appLibsDevDir\$id"
    if (!(Test-Path $p))
    {
        echo "App library '$id' is not cloned."
        continue
    }
    echo "Loading app library '$id' ..."
    robocopy "$appLibsDevDir\$id" "$appLibsDir\$id" /MIR /XD .git /NJH /NJS
    echo ""
}
echo "Finished loading app libraries."

popd