param (
    $name,
    $executable
)
$Host.UI.RawUI.WindowTitle = "Bench - $name - Pre-Run"

$Script:scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$Script:scriptsLib\bench.lib.ps1"

$customPreFile = "$Script:scriptsLib\..\apps\$($name.ToLowerInvariant()).pre-run.ps1"
if (Test-Path $customPreFile) {
    $old = Set-StopOnError $false
    . $customPreFile
    $_ = Set-StopOnError $old
}

$Host.UI.RawUI.WindowTitle = "Bench - $name"
