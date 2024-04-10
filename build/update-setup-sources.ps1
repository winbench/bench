$myDir = $PSScriptRoot
$rootDir = [IO.Path]::GetDirectoryName($myDir)
$srcDir = "$rootDir\BenchSetup"
$trgDir = "$rootDir\res\setup"
pushd

if (!(Test-Path $trgDir)) { mkdir $trgDir | Out-Null }

cd $srcDir
[string[]]$fileList = Get-Content "files.txt"
echo "Setup project sources"
foreach ($f in $fileList) {
    if (!$f) { continue }
    echo "- $f"
    cp $f "$trgDir\" -Force
}

popd