param (
    $name,
    $executable
)
$Host.UI.RawUI.WindowTitle = "Bench - $name - Pre-Run"

$Script:scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$Script:scriptsLib\bench.lib.ps1"
trap {
    Write-TrapError $_
    Pause
    exit 1
}


$customPreFile = "$Script:scriptsLib\..\apps\$($name.ToLowerInvariant()).pre-run.ps1"
if (Test-Path $customPreFile) {
    Debug "Executing custom pre-run script ..."
    . $customPreFile
}

$Host.UI.RawUI.WindowTitle = "Bench - $name"
