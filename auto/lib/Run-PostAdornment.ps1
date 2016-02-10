param (
    $name,
    $executable
)
$Host.UI.RawUI.WindowTitle = "Bench - $name - Post-Run"

$Script:scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$Script:scriptsLib\bench.lib.ps1"
trap {
    Write-TrapError $_
    Pause
    exit 1
}


$customPostFile = "$Script:scriptsLib\..\apps\$($name.ToLowerInvariant()).post-run.ps1"
if (Test-Path $customPostFile) {
    Debug "Executing custom post-run script ..."
    . $customPostFile
}
