param (
    $name,
    $executable
)

$Script:scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$Script:scriptsLib\bench.lib.ps1"

$customPreFile = "$Script:scriptsLib\..\apps\$($name.ToLowerInvariant()).pre-run.ps1"
if (Test-Path $customPreFile) {
    Write-Host "Pre-Execution Adornment for $name ..."
    $old = Set-StopOnError $false
    . $customPreFile
    $_ = Set-StopOnError $old
}
