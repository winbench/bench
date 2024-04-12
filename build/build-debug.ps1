param (
    $MsBuildVerbosity = "normal"
)
$Script:thisDir = Split-Path $MyInvocation.MyCommand.Path -Parent

& "$thisDir\build.ps1" -Mode Debug -MsBuildVerbosity $MsBuildVerbosity -NoRelease -NoSign
