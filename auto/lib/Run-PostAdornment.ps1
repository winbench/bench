param (
    $name,
    $executable
)

$Script:scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$Script:scriptsLib\bench.lib.ps1"

$customPostFile = "$Script:scriptsLib\..\apps\$($name.ToLowerInvariant()).post-run.ps1"
if (Test-Path $customPostFile) {
    Write-Host "Post-Execution Adornment for $name ..."
    $old = Set-StopOnError $false
    . $customPostFile
    $_ = Set-StopOnError $old
}
